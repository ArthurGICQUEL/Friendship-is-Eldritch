using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [HideInInspector] public Room room;
    public Door targetDoor;
    public Vector3 floorPos;

    void Awake() {
        room = GetComponentInParent<Room>();
    }
}
