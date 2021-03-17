using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [HideInInspector] public Room room;
    public Door targetDoor;
    public Vector2 pos;

    private void Awake() {
        room = GetComponentInParent<Room>();
    }
}
