using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [HideInInspector] public Door[] doors;
    [HideInInspector] public List<Human> humans;
    [HideInInspector] public List<Minion> minions;
    [HideInInspector] public Vector3[] floorLimits;
    [SerializeField] Vector3[] floorLimitsOffset = null;
    public Bounds bounds;
    public bool isStartRoom;


    private void Awake()
    {
        doors = GetComponentsInChildren<Door>();

        floorLimits = new Vector3[floorLimitsOffset.Length];
        for (int i = 0; i < floorLimitsOffset.Length; i++)
        {
            floorLimits[i] = floorLimitsOffset[i] + transform.position;
        }

        if (isStartRoom) GameManager.Instance.startRoom = this;
    }

    private void Update() {
        Debug.LogWarning($"{name} has {humans.Count} humans.");
    }

    public Vector3 GetMiddleFloor()
    {
        return Vector3.Lerp(floorLimits[0], floorLimits[1], 0.5f);
    }

    /*
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
    }*/
}
