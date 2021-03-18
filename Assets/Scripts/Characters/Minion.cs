using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Character
{
    Human _prey = null;
    //Queue<Node> _targetTrail = new Queue<Node>();

    private void Start()
    {
        Human[] humans = FindObjectsOfType<Human>();
        AssignTarget(humans[Random.Range(0, humans.Length)]);
    }

    protected override void ChooseNextAction()
    {
        if (_prey == null) { return; }
        // frighten the humans in the room its in
        if (CurrentRoom != null) {
            List<Human> humans = CurrentRoom.GetAvailableHumans();
            for (int i=0; i<humans.Count; i++) {
                if (humans[i].State != MindState.Chased) {
                    humans[i].State = MindState.Chased;
                }
            }
        }
        if (_prey.CurrentRoom == CurrentRoom 
            || (_prey._targetNode == _targetNode && _prey._lastNode == _lastNode) 
            || (_prey._targetNode == _lastNode && _prey._lastNode == _targetNode)) {
            _targetPos = _prey.transform.position;
        }
        if (Move(_targetPos))
        {
            CurrentRoom = FindCurrentRoom();
            _targetNode = Bfs.GetNextNode(_targetPos, _prey._targetNode.position);
            //Debug.Log($"targetNode: {_targetNode}");
            if (_targetNode != null) {
                _targetPos = _targetNode.position;
            } else {
                _targetPos = _prey.transform.position;
            }
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
            _currentRoom = null;
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
                Debug.LogWarning("Crunchy crunch, the prey has been eaten!"); // TODO: eat the human
            }
        }
    }
}