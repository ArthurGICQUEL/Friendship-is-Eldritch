using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool win = false;

    [HideInInspector] public GameObject instMoon;
    public int nbStartHuman = 5;
    public Room startRoom;
    public float timeOfNight, tTNight = 60f, timerTime = 1;
    public bool gameHasEnded = false;

    [SerializeField] GameObject moonPrefab = null;
    [SerializeField] RuntimeAnimatorController[] characterControllers = null;
    [SerializeField] int totalNightSteps = 3;
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
        if (!gameHasEnded)
        {
            AudioManager.Instance.Play("BG");
            Bfs.InitGraph();
            instMoon = Instantiate(moonPrefab, transform.position, Quaternion.identity);
            timeOfNight = 0;
            for (int i = 0; i < nbStartHuman; i++)
            {
                Human human = Instantiate((GameObject)Resources.Load("Human"), Vector3.Lerp(startRoom.floorLimits[0], startRoom.floorLimits[1], (i + 1) / (float)(nbStartHuman + 1)), Quaternion.identity).GetComponent<Human>();
                human.GetComponentInChildren<Animator>().runtimeAnimatorController = characterControllers?[i % characterControllers.Length];
            }
        }
    }
    private void Update()
    {
        if (!gameHasEnded)
        {
            if (timeOfNight < tTNight)
            {
                timeOfNight += Time.deltaTime;
            }
            else
            {
                EndGame(true);
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

    public void EndGame(bool nightEnded = false)
    {
        if (gameHasEnded) { return; }
        Human[] humans = FindObjectsOfType<Human>();
        bool notAllPossessed = false;
        for (int i = 0; i < humans.Length; i++)
        {
            if (humans[i].State != MindState.Enlightened)
            {
                notAllPossessed = true;
            }
        }
        if (!notAllPossessed)
        {
            gameHasEnded = true;
            win = true;
            LoadWinLoseScene();
        }
        else if (nightEnded)
        {
            gameHasEnded = true;
            win = false;
            LoadWinLoseScene();
        }
    }

    void LoadWinLoseScene()
    {
        SceneManager.LoadScene(3);
    }
}
