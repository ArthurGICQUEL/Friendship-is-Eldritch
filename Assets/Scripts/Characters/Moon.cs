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
        m.rotation = new Quaternion(0,0,Mathf.Lerp(-angle, angle, GameManager.Instance.timeOfNight / GameManager.Instance.tTNight),0);
        m.position = new Vector3( Mathf.Lerp(targets[0].transform.position.x, targets[1].transform.position.x, GameManager.Instance.timeOfNight / GameManager.Instance.tTNight),0,0);
    }
}
