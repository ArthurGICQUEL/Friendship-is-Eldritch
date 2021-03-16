using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : Spell
{
    public Room target;
    public override void Cast()
    {
        AudioManager.Instance.Play("Summon");

        GameObject.Instantiate<Minion>(Resources.Load(""), target.transform.position, Quaternion.identity);
    }
}