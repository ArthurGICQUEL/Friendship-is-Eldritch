using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [HideInInspector] public Door[] doors;
    [HideInInspector] public List<Human> humans;
    public Vector3[] floorLimits;
    public Bounds bounds;
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
            human.currentRoom = this;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Human>(out Human human))
        {
            human.currentRoom = null;
            humans.Remove(human);
            human.currentRoom = null;
        }
    }
}
