using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int nbStartHuman = 5;
    public Room startRoom;
    public float timeOfNight;


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

        Bfs.InitGraph();
    }
    private void Start()
    {
        for (int i = nbStartHuman; i == 0; i--)
        {
            Instantiate(Resources.Load("Human"), Vector3.Lerp(startRoom.floorLimits[0], startRoom.floorLimits[1], (i + 1) / (nbStartHuman + 2)), Quaternion.identity);
        }
    }
}
