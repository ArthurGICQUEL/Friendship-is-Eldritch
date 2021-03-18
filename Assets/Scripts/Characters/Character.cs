using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Room CurrentRoom
    {
        get { return _currentRoom; }
        set { 
            OnExitRoom(_currentRoom); OnEnterRoom(value); 
        }
    }
    [HideInInspector] public Node _targetNode = null, _lastNode = null;

    protected float Speed { 
        get { return _baseSpeed * _speedRatio; } 
    }
    [SerializeField] protected float _baseSpeed = 1;
    protected float _speedRatio = 1;
    protected Room _currentRoom = null;
    protected Vector3 _targetPos;
    protected Room _lastRoom = null;
    protected Animator _anim;

    SpriteRenderer _sr;
    Vector3 _lastPos;

    protected virtual void Awake()
    {
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
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
        // Find current room
        Room room = FindCurrentRoom();
        if(room != CurrentRoom) { _lastRoom = CurrentRoom; }
        CurrentRoom = room;
        // act on behavior
        ChooseNextAction();
    }

    /// <summary>
    /// Move smoothly and linearly to the <b>targetPoint</b>.
    /// </summary>
    /// <param name="targetPoint">The position to reach.</param>
    /// <returns><b>True</b> if the targetPoint has been reached, <b>False</b> if it hasn't.</returns>
    protected bool Move(Vector3 targetPoint)
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

    protected abstract void ChooseNextAction();

    protected abstract void OnEnterRoom(Room room);

    protected abstract void OnExitRoom(Room room);
}