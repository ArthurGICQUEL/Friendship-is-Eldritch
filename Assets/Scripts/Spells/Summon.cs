using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : Spell
{
    public const int summonCost = 7;
    public Summon() : base()
    {
        cost = summonCost;
    }
    public override bool Cast()
    {
        if (target.humans.Count > 0) return false;
        //AudioManager.Instance.Play("Summon");

        GameObject.Instantiate(Resources.Load("Villain"), target.GetMiddleFloor(), Quaternion.identity);
        GameManager.Instance.Mana -= cost;
        return true;
    }
}