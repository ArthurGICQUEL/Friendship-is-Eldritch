using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    Spell selectedSpell;
    public Slider sliderNight;
    public Image fillNight;
    Room[] rooms;

    [SerializeField] GameObject canvasSpellCasting = null, buttonIllusion, buttonPossession, buttonSummon;
    [SerializeField] Text manaTxt;

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
        SetMaxTime(GameManager.Instance.timeOfNight, GameManager.Instance.tTNight);
        rooms = FindObjectsOfType<Room>();
    }

    private void Update()
    {
        SetTime(GameManager.Instance.timeOfNight);
        if (selectedSpell != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.TryGetComponent(out Room room)) OnRoomClick(room);
            }

            if (selectedSpell is Illusion || selectedSpell is Possession)
            {
                for (int i = 0; i < rooms.Length; i++)
                {
                    rooms[i].HilightedBorders.SetActive(rooms[i].humans.Count > 0);
                }
            }
            if (selectedSpell is Summon)
            {
                for (int i = 0; i < rooms.Length; i++)
                {
                    rooms[i].HilightedBorders.SetActive(rooms[i].humans.Count == 0);
                }
            }
        }
    }

    public void UpdateUI()
    {
        manaTxt.text = "Power : " + GameManager.Instance.Mana + " / " + GameManager.Instance.ManaMax;
        if (GameManager.Instance.Mana < Illusion.illusionCost)
        {
            buttonIllusion.transform.GetChild(1).gameObject.SetActive(true);
        }
        else buttonIllusion.transform.GetChild(1).gameObject.SetActive(false);
        if (GameManager.Instance.Mana < Possession.possessionCost)
        {
            buttonPossession.transform.GetChild(1).gameObject.SetActive(true);
        }
        else buttonPossession.transform.GetChild(1).gameObject.SetActive(false);
        if (GameManager.Instance.Mana < Summon.summonCost)
        {
            buttonSummon.transform.GetChild(1).gameObject.SetActive(true);
        }
        else buttonSummon.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void OnRoomClick(Room room)
    {
        selectedSpell.target = room;
        if (selectedSpell.cost <= GameManager.Instance.Mana && selectedSpell.Cast()) DeselectSpell();
    }

    public void OnSpellClick(int spell)
    {
        {
            Spell tryToselect = Spell.GetSpell((SpellType)spell);
            if (tryToselect.cost <= GameManager.Instance.Mana)
            {
                selectedSpell = tryToselect;
                canvasSpellCasting.SetActive(true);
            }
        }
    }

    public void DeselectSpell()
    {
        selectedSpell = null;
        canvasSpellCasting.SetActive(false);
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].HilightedBorders.SetActive(false);
        }
    }
    public void SetMaxTime(float time, float maxTime)
    {
        sliderNight.maxValue = maxTime;
        sliderNight.value = time;
    }
    public void SetTime(float time)
    {
        sliderNight.value = time;
    }
}
