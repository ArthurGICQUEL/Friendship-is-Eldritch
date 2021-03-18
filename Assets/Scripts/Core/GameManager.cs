using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject moonPrefab = null;
    [SerializeField] int totalNightSteps = 3;
    [HideInInspector] public GameObject instMoon;
    public int nbStartHuman = 5;
    public Room startRoom;
    public float timeOfNight, tTNight = 60f, timerTime = 1;
    float timer;
    int nightStep = 0;


    public int Mana
    {
        get { return _mana; }
        set
        {
            _mana = value;
            UIManager.Instance.UpdateUI();
        }
    }
    int _mana;
    public int ManaMax
    {
        get { return _manaMax; }
        set
        {
            _manaMax = value;
            UIManager.Instance.UpdateUI();
        }
    }
    int _manaMax;


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

    }

    private void Start()
    {
        Bfs.InitGraph();
        instMoon = Instantiate(moonPrefab, transform.position, Quaternion.identity);
        timeOfNight = 0;
        for (int i = 0; i < nbStartHuman; i++)
        {
            Instantiate(Resources.Load("Human"), Vector3.Lerp(startRoom.floorLimits[0], startRoom.floorLimits[1], (i + 1) / (float)(nbStartHuman + 1)), Quaternion.identity);
        }
    }
    private void Update()
    {
        if (timeOfNight < tTNight)
        {
            timeOfNight += Time.deltaTime;
        }
        else
        {
            //EndGame();
        }


        if (Mana < ManaMax && timer >= timerTime)
        {
            Mana += 1;
            timer = 0;
        }
        timer += Time.deltaTime;

        if (timeOfNight / tTNight >= nightStep / (float)totalNightSteps)
        {
            ManaMax += 3;
            Mana = ManaMax;
            nightStep++;
        }
    }
}
