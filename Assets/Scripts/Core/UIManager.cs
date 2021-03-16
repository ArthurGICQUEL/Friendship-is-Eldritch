using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    
    private Spell _selectedSpell = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        bool leftClick = Input.GetMouseButtonDown(0);
        bool rightClick = Input.GetMouseButtonDown(1);

        if (rightClick)
        {
            DeselectSpell(true);
        }
        if (_selectedSpell != null)
        {
            if (leftClick)
            {
                PlaceSelectedSpell();
            }
        }
    }

    public void SelectSpell(int spellType)
    {
        if (_selectedSpell != null)
        {
            //Spell.Type lastSpellType = _selectedSpell.type;
            DeselectSpell(true);
        }
    }

    public void DeselectSpell(bool destroy = false)
    {
        if(destroy)
        {

        }
        else
        {
            
        }
    }

    void PlaceSelectedSpell()
    {
        //if (_selectedSpell.Place())
        {
            DeselectSpell();
        }
    }
}
