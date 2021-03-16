using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : Spell
{
    public override bool Cast()
    {
        if (target.humans.Count > 0) return false;
        //AudioManager.Instance.Play("Summon");

        GameObject.Instantiate(Resources.Load("Villain"), target.transform.position, Quaternion.identity);
        return true;
    }
}