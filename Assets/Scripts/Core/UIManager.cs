using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    Spell selectedSpell;
    public Slider slider;
    public Image fill;

    [SerializeField] GameObject canvasSpellCasting = null;

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
        }
    }

    public void UpdateUI()
    {

    }

    public void OnRoomClick(Room room)
    {
        selectedSpell.target = room;
        if (selectedSpell.Cast()) DeselectSpell();
    }

    public void OnSpellClick(int spell)
    {
        selectedSpell = Spell.GetSpell((SpellType)spell);
        canvasSpellCasting.SetActive(true);
    }

    public void DeselectSpell()
    {
        selectedSpell = null;
        canvasSpellCasting.SetActive(false);
    }
    public void SetMaxTime(float time, float maxTime)
    {
        slider.maxValue = maxTime;
        slider.value = time;
    }
    public void SetTime(float time)
    {
        slider.value = time;
    }
}
