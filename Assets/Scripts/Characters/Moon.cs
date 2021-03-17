using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    [SerializeField] GameObject[] targets = null;
    [SerializeField] GameObject moonPrefab = null, instMoon;
    float angle = 30f;
    // Start is called before the first frame update
    void Start()
    {
        instMoon = Instantiate(moonPrefab, targets[0].transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveMoon(GameObject instMoon)
    {
        //instMoon.transform.rotation = Mathf.Lerp(-angle, angle, );
        //instMoon.transform.position = Mathf.Lerp(targets[0].transform.position, targets[1].transform.position, 0.1f * GameManager.Instance.timeOfNight);
    }
}
