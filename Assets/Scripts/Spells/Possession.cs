using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : Spell
{
    public Room target;
    public float stunTime = 3f;
    public override void Cast()
    {
        AudioManager.Instance.Play("Possesion");
        List<Human> humans = target.humans;
        Human possessed = humans[Random.Range(0, humans.Count)];
        possessed.Stun(stunTime);
        humans.Remove(possessed);
        for (int i = 0; i < humans.Count; i++)
        {
            humans[i].State = MindState.Panicking;
        }
    }
}