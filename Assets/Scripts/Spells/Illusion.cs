using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Illusion : Spell
{
    public float power = 1f;
    public override bool Cast()
    {
        if (target.humans.Count == 0) return false;
        //AudioManager.Instance.Play("Illusion");
        int nbHumans = target.humans.Count;
        for (int i = 0; i < nbHumans; i++)
        {
            target.humans[i].Sanity += power / nbHumans;
        }
        return true;
    }
}