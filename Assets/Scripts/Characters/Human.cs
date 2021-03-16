using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Character {
    public float Sanity {
        get { return _sanity; }
        set { _sanity = Mathf.Clamp(value, 0, sanityMax); }
    }

    [SerializeField] float sanityMax = 1;
    float _sanity = 1;

    protected override Door GetNextDoor() {
        //
        return null;
    }
}