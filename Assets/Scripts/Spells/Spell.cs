using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell
{
    public float cost;
    public Room target;
    public abstract bool Cast();
}

public enum SpellType
{
    Illusion = 0, Possesion = 1, Summon = 2
}