using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Room CurrentRoom
    {
        get { return _currentRoom; }
        set { OnExitRoom(_currentRoom); OnEnterRoom(value); }
    }
    public bool isStuned = false;

    [SerializeField] protected float _baseSpeed = 1;
    protected float _speedRatio = 1;
    protected Room _currentRoom = null;

    private void Awake()
    {
        CurrentRoom = FindCurrentRoom();
    }

    protected virtual void Update()
    {
        if (!isStuned)
        {
            ChooseNextAction();
        }
    }

    public void Stun(float duration)
    {
        isStuned = true;
        Invoke(nameof(UnStun), duration);
    }

    protected virtual void UnStun()
    {
        isStuned = false;
    }

    /// <summary>
    /// Move smoothly and linearly to the <b>targetPoint</b>.
    /// </summary>
    /// <param name="targetPoint">The position to reach.</param>
    /// <returns><b>True</b> if the targetPoint has been reached, <b>False</b> if it hasn't.</returns>
    protected bool Move(Vector3 targetPoint)
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime * _baseSpeed / Vector3.Distance(targetPoint, transform.position));
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