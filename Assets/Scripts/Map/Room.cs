using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    [HideInInspector] public Door[] doors;
    [HideInInspector] public List<Human> humans;
    [HideInInspector] public List<Minion> minions;
    [HideInInspector] public Vector3[] floorLimits;
    [HideInInspector] public GameObject hilightedBorders;
    //[SerializeField] Vector3[] floorLimitsOffset = null;
    //public Bounds bounds;
    //public bool isStartRoom;

    private void Awake()
    {
        BoxCollider2D _coll = GetComponent<BoxCollider2D>();
        doors = GetComponentsInChildren<Door>();
        hilightedBorders = transform.GetChild(0).gameObject;

        Vector2 offset = (_coll.size - Vector2.one * 2) * 0.5f;
        floorLimits = new Vector3[2];
        floorLimits[0] = new Vector3(-offset.x, -offset.y) + transform.position;
        floorLimits[1] = new Vector3(offset.x, -offset.y) + transform.position;
    }

    public List<Human> GetAvailableHumans() {
        List<Human> listHumans = new List<Human>(humans);
        for(int i = 0; i < humans.Count; i++) {
            if(humans[i].IsPossessed || humans[i].IsEnlightened) {
                listHumans.Remove(humans[i]);
            }
        }
        return listHumans;
    }

    public Vector3 GetMiddleFloor()
    {
        return Vector3.Lerp(floorLimits[0], floorLimits[1], 0.5f);
    }
}
