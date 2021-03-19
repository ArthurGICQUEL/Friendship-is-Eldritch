using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    Spell selectedSpell;
    public Slider sliderTimeOfNight, sliderVolume;
    public Image fillTimeOfNight;
    Room[] rooms;

    [SerializeField] GameObject mainCanvas = null;
    [SerializeField] GameObject canvasSpellCasting = null, buttonIllusion, buttonPossession, buttonSummon;
    [SerializeField] Text manaTxt;
    bool uiIsHidden = false;

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
        
        Mathf.Clamp(sliderVolume.value, 0f, 1f);
        GetComponent<AudioSource>().volume = sliderVolume.value;
        // Hide UI elements
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.W)) {
            if (!uiIsHidden) {
                DeselectSpell();
            }
            uiIsHidden = !uiIsHidden;
            mainCanvas.SetActive(!uiIsHidden);
        }
        if (uiIsHidden) { return; }

        SetTime(GameManager.Instance.timeOfNight);
        if (selectedSpell != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                for (int i=0; i<hits.Length; i++) {
                    if(hits[i].collider.TryGetComponent(out Room room)) OnRoomClick(room);
                }
            }

            if (selectedSpell is Illusion || selectedSpell is Possession)
            {
                for (int i = 0; i < rooms.Length; i++)
                {
                    rooms[i].hilightedBorders.SetActive(rooms[i].humans.Count > 0);
                }
            }
            if (selectedSpell is Summon)
            {
                for (int i = 0; i < rooms.Length; i++)
                {
                    rooms[i].hilightedBorders.SetActive(rooms[i].humans.Count == 0);
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
        //Debug.Log("Clicked room: " + room?.name);
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
            rooms[i].hilightedBorders.SetActive(false);
        }
    }
    public void SetMaxTime(float time, float maxTime)
    {
        sliderTimeOfNight.maxValue = maxTime;
        sliderTimeOfNight.value = time;
    }
    public void SetTime(float time)
    {
        sliderTimeOfNight.value = time;
    }
}
