using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    [SerializeField] GameObject[] targets = null;
    
    float angle = 30f;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(targets[0], targets[0].transform.position, targets[0].transform.rotation);
        Instantiate(targets[1], targets[1].transform.position, targets[1].transform.rotation);
        transform.position = targets[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveMoon(GameManager.Instance.instMoon);
    }
    public void MoveMoon(GameObject objectToMove)
    {
        Transform m = objectToMove.transform;
        Vector3 center = (targets[0].transform.position + targets[1].transform.position) * 0.5F;
        Vector3 riseRelCenter = targets[0].transform.position - center;
        Vector3 setRelCenter = targets[1].transform.position - center;
        m.position = Vector3.Slerp(riseRelCenter, setRelCenter, GameManager.Instance.timeOfNight/GameManager.Instance.tTNight);
        transform.position += center;
    }
}
