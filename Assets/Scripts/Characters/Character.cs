﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Room currentRoom {
        get { return _currentRoom; }
        set { _currentRoom = value; OnEnterRoom(); }
    }
    public bool isStuned = false;

    [SerializeField] protected float _baseSpeed = 1;
    protected float _speedRatio = 1;
    protected Room _currentRoom = null;

    private void Start() {
        _currentRoom = GameManager.Instance.startRoom;
    }

    private void Update()
    {
        if (!isStuned)
        {
            ChooseNextAction();
        }
    }

    protected abstract void ChooseNextAction();

    protected abstract void OnEnterRoom();

    void UnStun()
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

    public void Stun(float duration)
    {
        isStuned = true;
        Invoke(nameof(UnStun), duration);
    }
}