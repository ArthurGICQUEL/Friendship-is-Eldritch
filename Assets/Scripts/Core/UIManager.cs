using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    Spell selectedSpell;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    
    [SerializeField] GameObject canvasSpellCasting;

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
        SetMaxHealth(GameManager.Instance.timeOfNight, GameManager.Instance.tTNight);
    }

    private void Update()
    {
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
        SetHealth(GameManager.Instance.timeOfNight);
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
    public void SetMaxHealth(float time, float maxTime)
    {
        slider.maxValue = maxTime;
        slider.value = time / maxTime;
        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(float time)
    {
        slider.value = time;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
