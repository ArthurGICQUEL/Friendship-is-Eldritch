using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Room CurrentRoom
    {
        get { return _currentRoom; }
        set
        {
            ExitRoom();
            if (value != null)
            {
                EnterRoom(value);
            }
        }
    }
    [HideInInspector] public Node _targetNode = null, _lastNode = null;

    protected float Speed
    {
        get { return _baseSpeed * _speedRatio; }
    }
    [SerializeField] protected float _baseSpeed = 1;
    protected float _speedRatio = 1;
    protected Room _currentRoom = null;
    protected Vector3 _targetPos;
    protected Room _lastRoom = null;
    protected Animator _anim;
    protected Vector3 _lastPos;

    SpriteRenderer _sr;

    protected virtual void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        CurrentRoom = FindCurrentRoom();
        _lastPos = transform.position;
    }

    protected virtual void Update()
    {
        // sprite orientation
        Vector3 dir = transform.position - _lastPos;
        if (dir.x > 0) { _sr.flipX = true; }
        else if (dir.x < 0) { _sr.flipX = false; }

        _lastPos = transform.position;
        // act on behavior
        ChooseNextAction();

    }

    protected bool MoveToTargetNode()
    {
        if (_targetNode == null) { return false; }
        if (LerpToward(_targetNode.position))
        {
            _lastNode = _targetNode;
            // determine if entering or exiting a room
            Door door = FindCurrentDoor();
            if (door != null)
            {
                if (CurrentRoom == null)
                {
                    CurrentRoom = door.room;
                }
                else if (CurrentRoom == door.room)
                {
                    _lastRoom = CurrentRoom;
                    CurrentRoom = null;
                }
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Move smoothly and linearly to the <b>targetPoint</b>.
    /// </summary>
    /// <param name="targetPoint">The position to reach.</param>
    /// <returns><b>True</b> if the targetPoint has been reached, <b>False</b> if it hasn't.</returns>
    protected bool LerpToward(Vector3 targetPoint)
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime * Speed / Vector3.Distance(targetPoint, transform.position));
        return transform.position == targetPoint;
    }

    protected Room FindCurrentRoom()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.TryGetComponent(out Room room))
            {
                return room;
            }
        }
        return null;
    }

    protected Door FindCurrentDoor()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.TryGetComponent(out Door door))
            {
                return door;
            }
        }
        return null;
    }

    protected abstract void ChooseNextAction();

    protected abstract void EnterRoom(Room room);

    protected abstract void ExitRoom();
}