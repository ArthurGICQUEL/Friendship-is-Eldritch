using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [HideInInspector] public Door[] doors;
    [HideInInspector] public List<Human> humans;
    [HideInInspector] public List<Minion> minions;
    public Vector3[] floorLimits;
    public Bounds bounds;
    public bool isStartRoom;

    private void Awake()
    {
        doors = GetComponentsInChildren<Door>();
        if (isStartRoom) GameManager.Instance.startRoom = this;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Character character)) {
            character.currentRoom = this;
            if (character is Human) {
                humans.Add((Human)character);
            } else if (character is Minion) {
                minions.Add((Minion)character);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.TryGetComponent(out Character character)) {
            character.currentRoom = null;
            if(character is Human) {
                humans.Remove((Human)character);
            } else if(character is Minion) {
                minions.Remove((Minion)character);
            }
        }
    }
}
