using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : Spell
{
    public const int summonCost = 7;
    public const float summonPower = 0.5f;
    public Summon() : base()
    {
        cost = summonCost;
    }
    public override bool Cast()
    {
        GameObject fx = (GameObject)Resources.Load("E_Summon");
        GameObject fxFollow = (GameObject)Resources.Load("E_Minion");
        if (target.humans.Count > 0) return false;
        AudioManager.Instance.Play("Cast");

        Minion minion = ((GameObject)GameObject.Instantiate(Resources.Load("Villain"), target.GetMiddleFloor(), Quaternion.identity)).GetComponent<Minion>();
        GameObject.Instantiate(fx, minion.transform.position, fx.transform.rotation, minion.transform);
        GameObject.Instantiate(fxFollow, minion.transform.position, fxFollow.transform.rotation, minion.transform);
        minion.insanityPower = summonPower;
        GameManager.Instance.Mana -= cost;
        return true;
    }
}