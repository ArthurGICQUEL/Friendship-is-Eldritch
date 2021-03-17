using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Character
{
    Human _target;
    Queue<Node> _targetTrail = new Queue<Node>();

    protected override void ChooseNextAction() {
        // hunting behaviour
    }

    protected override void OnEnterRoom() {
        // check if human is there
    }
}