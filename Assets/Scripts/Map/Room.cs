﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Door[] doors;
    public Vector3[] floorLimits;
    public Bounds bounds;
    public List<Human> humans;
    public bool isStartRoom;

    private void Awake()
    {
        doors = GetComponentsInChildren<Door>();
        if (isStartRoom) GameManager.Instance.startRoom = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Human>(out Human human))
        {
            humans.Add(human);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Human>(out Human human))
        {
            humans.Remove(human);
        }
    }
}
