﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject moonPrefab = null;
    public GameObject instMoon;
    public int nbStartHuman = 5;
    public Room startRoom;
    public float timeOfNight, tTNight = 0f;


    public float Mana
    {
        get { return _mana; }
        set
        {
            _mana = value;
            UIManager.Instance.UpdateUI();
        }
    }
    float _mana;
    public float ManaMax
    {
        get { return _manaMax; }
        set
        {
            _manaMax = value;
            UIManager.Instance.UpdateUI();
        }
    }
    float _manaMax;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Bfs.InitGraph();
    }
    private void Start()
    {
        instMoon = Instantiate(moonPrefab, transform.position, Quaternion.identity);
        tTNight = 5f;
        timeOfNight = 0;
        for (int i = 0; i < nbStartHuman; i++)
        {
            //Instantiate((Human)Resources.Load("Human"), Vector3.Lerp(startRoom.floorLimits[0], startRoom.floorLimits[1], (i + 1) / (float)(nbStartHuman + 1)), Quaternion.identity);
        }
    }
    private void Update()
    {
        if (timeOfNight < tTNight)
        {
            IncMoonTime();
        }
        else
        {
            //Time.timeScale = 0;
        }
    }
    void IncMoonTime()
    {
<<<<<<< HEAD
        timeOfNight = Time.time * 1;
=======
        //timeOfNight = Time.time * 1;
        //Debug.Log(timeOfNight);
>>>>>>> 144166b5da9a5cf3142fe8034c19e2affa04ec27
    }
}
