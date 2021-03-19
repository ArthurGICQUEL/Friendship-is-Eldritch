using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Illusion : Spell
{
    public const int illusionCost = 2;
    public float power = 1 / 3f;

    public Illusion() : base()
    {
        cost = illusionCost;
    }

    public override bool Cast()
    {
        if (target.humans.Count == 0) return false;
        //AudioManager.Instance.Play("Illusion");
        int nbHumans = target.humans.Count;
        for (int i = 0; i < nbHumans; i++)
        {
            target.humans[i].Sanity -= power / nbHumans;
        }
        GameManager.Instance.Mana -= cost;
        return true;
    }
}