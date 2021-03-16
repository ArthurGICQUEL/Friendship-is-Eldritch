using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public MindState State
    {
        get { return _state; }
        set { SetMindState(value); }
    }
    public Room currentRoom;
    public bool isStuned = false;

    [SerializeField] float _speed = 1;
    [SerializeField] float _idleTime = 1;
    MindState _state = MindState.Idle;
    Door _targetDoor = null, _lastDoor = null;
    Vector3 _inRoomTarget;
    float _stateTimer = 0;

    private void Update()
    {
        if (!isStuned)
        {
            ChooseNextAction();
        }
    }

    void ChooseNextAction()
    {
        switch (State)
        {
            case MindState.Idle:
                ActIdle();
                break;
            case MindState.Exploring:
                ActExploring();
                break;
            case MindState.Panicking:
                break;
            case MindState.Chased:
                break;
            case MindState.Enlightened:
                break;
            case MindState.Hunting:
                break;
            default:
                break;
        }
    }

    void ActIdle()
    {
        _stateTimer += Time.deltaTime;
        if (_stateTimer >= _idleTime)
        { // if the idleTimer expires, switch to Exploring
            State = MindState.Exploring;
            return;
        }
        // while the timer is ticking, move to random positions in the room
        if (Move(_inRoomTarget))
        {
            _inRoomTarget = GetTargetInRoom();
        }
    }

    void ActExploring()
    {
        if (_targetDoor == null) {
            // get the available doors in the currentRoom without the last door used, unless it's the only door
            List<Door> availableDoors = new List<Door>(currentRoom.doors);
            if (_lastDoor != null && availableDoors.Count > 1) {
                availableDoors.Remove(_lastDoor);
            }
            // get a random door among the available ones
            Door nextDoor = availableDoors[Random.Range(0, availableDoors.Count)];
            // pass the chosen door to all the humans in the room
            Human[] group = currentRoom.humans.ToArray();
            for (int i=0; i<group.Length; i++) {
                group[i]._targetDoor = nextDoor;
            }
        }
        if (Move(_targetDoor.transform.position)) {
            if (_lastDoor != null) {
                if (_lastDoor == _targetDoor.targetDoor) {
                    State = MindState.Idle;
                    return;
                }
            } 
            _lastDoor = _targetDoor;
        }
    }

    void UnStun()
    {
        isStuned = false;
    }

    Vector3 GetTargetInRoom()
    {
        return Vector3.Lerp(currentRoom.floorLimits[0], currentRoom.floorLimits[1], Random.Range(0f, 1f));
    }

    void SetMindState(MindState newState)
    {
        switch (newState)
        {
            case MindState.Idle:
                _stateTimer = 0;
                _inRoomTarget = transform.position;
                break;
            case MindState.Exploring:
                break;
            case MindState.Panicking:
                break;
            case MindState.Chased:
                break;
            case MindState.Enlightened:
                break;
            case MindState.Hunting:
                break;
            default:
                break;
        }
        _state = newState;
    }

    /// <summary>
    /// Move smoothly and linearly to the <b>targetPoint</b>.
    /// </summary>
    /// <param name="targetPoint">The position to reach.</param>
    /// <returns><b>True</b> if the targetPoint has been reached, <b>False</b> if it hasn't.</returns>
    protected bool Move(Vector3 targetPoint)
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime * _speed / Vector3.Distance(targetPoint, transform.position));
        return transform.position == targetPoint;
    }

    protected abstract Door GetNextDoor();

    public void Stun(float duration)
    {
        isStuned = true;
        Invoke(nameof(UnStun), duration);
    }
}

public enum MindState
{
    Idle = 0, Exploring = 1, Panicking = 2, Chased = 3, Enlightened = 4, Hunting = 10
}