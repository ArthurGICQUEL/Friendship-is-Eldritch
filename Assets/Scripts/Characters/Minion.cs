using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Character
{
    Human _target;

    private void Awake()
    {
        State = MindState.Hunting;
    }

    protected override Door GetNextDoor()
    {
        //
        return null;
    }
}