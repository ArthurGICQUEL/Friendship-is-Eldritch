using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : Spell
{
    public const int possessionCost = 5;
    public float possessTime = 3f;
    public float possessPower = 1 / 10f;

    public Possession() : base()
    {
        cost = possessionCost;
    }

    public override bool Cast()
    {
        if (target.humans.Count == 0) return false;
        //AudioManager.Instance.Play("Possesion");
        List<Human> humans = target.GetAvailableHumans();
        if(humans.Count == 0) return false;

        Human possessed = humans[Random.Range(0, humans.Count)];
        possessed.Possess(possessTime);
        possessed.Sanity -= possessPower;
        humans.Remove(possessed);
        for (int i = 0; i < humans.Count; i++)
        {
            humans[i].State = MindState.Panicking;
        }
        GameManager.Instance.Mana -= cost;
        return true;
    }
}