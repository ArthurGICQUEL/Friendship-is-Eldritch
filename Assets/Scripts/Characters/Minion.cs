using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Character
{
    Human _prey = null;
    Queue<Node> _targetTrail = new Queue<Node>();

    private void Start()
    {
        Human[] humans = FindObjectsOfType<Human>();
        AssignTarget(humans[Random.Range(0, humans.Length)]);
    }

    protected override void ChooseNextAction()
    {
        if (_prey == null) { return; }
        /*Vector3 vPrey = _prey.transform.position - transform.position;
        Vector3 vNode = _targetNode.position - transform.position;
        if(vPrey.normalized == vNode.normalized && vPrey.magnitude < vNode.magnitude) {
            _targetPos = _prey.transform.position;
        }*/
        if (Move(_targetPos))
        {
            CurrentRoom = FindCurrentRoom();
            _targetNode = Bfs.GetNextNode(CurrentRoom.GetMiddleFloor(), _prey.CurrentRoom.GetMiddleFloor());
            _targetPos = _targetNode.position;
        }
    }

    protected override void OnEnterRoom(Room room)
    {
        if (room == null) { return; }
        if (!room.minions.Contains(this))
        {
            room.minions.Add(this);
            _currentRoom = room;
        }
    }

    protected override void OnExitRoom(Room room)
    {
        if (room == null) { return; }
        if (room.minions.Contains(this))
        {
            room.minions.Remove(this);
        }
    }

    public void AssignTarget(Human target)
    {
        _prey = target;
        _targetPos = CurrentRoom.GetMiddleFloor();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Human human))
        {
            if (human == _prey)
            {
                Debug.Log("Crunchy crunch, the prey has been eaten!"); // TODO: eat the human
            }
        }
    }
}