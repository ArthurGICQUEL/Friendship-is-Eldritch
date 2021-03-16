using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Door> doors;
    public Vector4 bounds;
    public List<Human> humans;
    // Start is called before the first frame update
    void Start()
    {
        GetDoors();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public List<Door> GetDoors()
    {
        int count = transform.childCount;
        List<Door> portes = new List<Door>();
        for (int i = 0; i > count; i++)
        {
            Door newChild = transform.GetChild(i).GetComponent<Door>();
            portes.Add(newChild);
        }
        return portes;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Human>(out Human human))
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
