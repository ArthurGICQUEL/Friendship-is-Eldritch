using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [HideInInspector] public Room room;
    [HideInInspector] public Vector3 floorPos;
    public Door targetDoor;

    void Awake() {
        room = GetComponentInParent<Room>();
        floorPos = transform.position - new Vector3(0, 0.5f);
    }
}
