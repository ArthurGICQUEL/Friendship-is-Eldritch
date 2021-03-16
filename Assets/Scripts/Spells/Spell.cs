using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell
{
    public float cost;
    public Room target;
    public abstract bool Cast();

    public static Spell GetSpell(SpellType spell)
    {
        switch (spell)
        {
            case SpellType.Illusion:
                return new Illusion();
            case SpellType.Possesion:
                return new Possession();
            case SpellType.Summon:
                return new Summon();
            default:
                return null;
        }
    }
}

public enum SpellType
{
    Illusion = 0, Possesion = 1, Summon = 2
}