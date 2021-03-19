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
        GameObject fx = (GameObject)Resources.Load("E_Illusion");
        if (target.humans.Count == 0) return false;
        AudioManager.Instance.Play("Cast");
        GameObject.Instantiate(fx, target.GetMiddleFloor(), fx.transform.rotation);
        int nbHumans = target.humans.Count;
        for (int i = 0; i < nbHumans; i++)
        {
            target.humans[i].Sanity -= power / nbHumans;
        }
        GameManager.Instance.Mana -= cost;
        return true;
    }
}