using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Character
{
    Human _target;

    protected override void ChooseNextAction() {
        // hunting behaviour
    }

    protected override Door GetNextDoor()
    {
        //
        return null;
    }

    protected override void OnEnterRoom() {
        //
    }
}