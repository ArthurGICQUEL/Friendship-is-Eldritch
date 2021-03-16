﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Illusion : Spell
{
    public Room target;
    public float power;
    public override void Cast()
    {
        AudioManager.Instance.Play("Illusion");
        int nbHumans = target.humans.Count;
        for (int i = 0; i < nbHumans; i++)
        {
            target.humans[i].Sanity += power / nbHumans;
        }
    }
}