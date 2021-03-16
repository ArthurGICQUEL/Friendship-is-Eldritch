using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    Spell selectedSpell;

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

    private void Update()
    {
        if (selectedSpell != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.GetComponent<Room>() != null) OnRoomClick(hit.collider.GetComponent<Room>());
            }
        }
    }

    public void OnRoomClick(Room room)
    {
        selectedSpell.target = room;
        if (selectedSpell.Cast()) DeselectSpell();
    }

    public void OnSpellClick(int spell)
    {
        selectedSpell = GetSpell(spell);
        canvasSpellCasting.SetActive(true);
    }

    public void DeselectSpell()
    {
        selectedSpell = null;
        canvasSpellCasting.SetActive(false);
    }

    public Spell GetSpell(int spell)
    {
        switch (spell)
        {
            case 0:
                return new Illusion();
            case 1:
                return new Possession();
            case 2:
                return new Summon();
            default:
                return null;
        }
    }
}
