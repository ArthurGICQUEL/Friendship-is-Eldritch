using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Character
{
    public MindState State
    {
        get { return _state; }
        set { SetMindState(value); }
    }
    public float Sanity
    {
        get { return _sanity; }
        set { _sanity = Mathf.Clamp(value, 0, sanityMax); OnSanityChange(); }
    }

    [SerializeField] float _speedRatioIdle = 0.5f;
    [SerializeField] float _speedRatioExploring = 1f;
    [SerializeField] float _speedRatioPanicking = 1.5f;
    [SerializeField] float _speedRatioChased = 2f;
    [SerializeField] float _idleDuration = 10;
    [SerializeField] float _idleStillDuration = 1;
    [SerializeField] float sanityMax = 1;
    float _sanity = 1;
    MindState _state = MindState.Idle;
    //Minion _hunter = null;
    Door _targetDoor = null, _lastDoor = null;
    Vector3 _inRoomTarget;
    float _stateTimer = 0, _idleStillTimer = 0;

    protected override void ChooseNextAction()
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
                ActPanicking();
                break;
            case MindState.Chased:
                ActChased();
                break;
            case MindState.Enlightened:
                ActEnlightened();
                break;
            default:
                break;
        }
    }

    void ActIdle()
    {
        _stateTimer -= Time.deltaTime;
        if (_stateTimer <= 0)
        { // if the idleTimer expires, switch to Exploring
            State = MindState.Exploring;
            return;
        }
        // while the timer is ticking, wait a little bit then move to random positions in the room and repeat
        if (_idleStillTimer > 0)
        {
            _idleStillTimer -= Time.deltaTime;
        }
        if (_idleStillTimer <= 0)
        {
            if (Move(_inRoomTarget))
            {
                _inRoomTarget = GetTargetInRoom();
                _idleStillTimer = _idleStillDuration;
            }
        }
    }

    void ActExploring()
    {
        if (_targetDoor == null)
        {
            // get the available doors in the currentRoom without the last door used, unless it's the only door
            List<Door> availableDoors = new List<Door>(CurrentRoom.doors);
            if(_lastDoor != null && availableDoors.Count > 1) {
                availableDoors.Remove(_lastDoor);
            }
            // get a random door among the available ones
            Door nextDoor = availableDoors[Random.Range(0, availableDoors.Count)];
            // pass the chosen door to all the humans in the room
            Human[] group = CurrentRoom.humans.ToArray();
            for(int i = 0; i < group.Length; i++) {
                group[i]._targetDoor = nextDoor;
            }
        }
        MoveToTargetDoor();
    }

    void ActPanicking()
    {
        if (_targetDoor == null)
        {
            // get the available doors in the currentRoom
            List<Door> availableDoors = new List<Door>(CurrentRoom.doors);
            // get a random door among the available ones
            _targetDoor = availableDoors[Random.Range(0, availableDoors.Count)];
        }
        MoveToTargetDoor();
    }

    void ActChased()
    {
        if (_targetDoor == null)
        {
            // get the available doors in the currentRoom that are opposite to the minions
            List<Door> availableDoors = new List<Door>();
            for (int i=0; i< CurrentRoom.doors.Length; i++) {
                if (!CheckIfMinionBlocksDoor(CurrentRoom.doors[i])) {
                    availableDoors.Add(CurrentRoom.doors[i]);
                }
            }
            // get a random door among the available ones or stay frightened
            if (availableDoors.Count > 0)
            {
                _targetDoor = availableDoors[Random.Range(0, availableDoors.Count)];
            }
        }
        if (_targetDoor == null) { return; }
        MoveToTargetDoor();
    }

    void ActEnlightened()
    {
        // ???
    }

    void MoveToTargetDoor()
    {
        if (Move(_targetDoor.transform.position))
        {
            if (_lastDoor == null)
            {
                _lastDoor = _targetDoor;
            }
            // if the room of the door is different than the current one, then a new room has been reached
            if(_targetDoor.room != CurrentRoom) {
                CurrentRoom = _targetDoor.room;
                State = MindState.Idle;
                return;
            }
            _lastDoor = _targetDoor;
            _targetDoor = _targetDoor.targetDoor;
        }
    }

    Vector3 GetTargetInRoom() {
        return Vector3.Lerp(CurrentRoom.floorLimits[0], CurrentRoom.floorLimits[1], Random.Range(0f, 1f));
    }

    void OnSanityChange()
    {
        // if sanity becomes zero or smth
    }

    protected override void OnEnterRoom(Room room) {
        if(room == null) { return; }
        if(!room.humans.Contains(this)) {
            room.humans.Add(this);
            _currentRoom = room;
        }
    }

    protected override void OnExitRoom(Room room) {
        if(room == null) { return; }
        if (room.humans.Contains(this)) {
            room.humans.Remove(this);
        }
    }

    void SetMindState(MindState newState)
    {
        switch (newState)
        {
            case MindState.Idle:
                _stateTimer = _idleDuration;
                _inRoomTarget = transform.position;
                _speedRatio = _speedRatioIdle;
                break;
            case MindState.Exploring:
                _speedRatio = _speedRatioExploring;
                break;
            case MindState.Panicking:
                _speedRatio = _speedRatioPanicking;
                break;
            case MindState.Chased:
                _speedRatio = _speedRatioChased;
                break;
            case MindState.Enlightened:
                // ???
                break;
            default:
                break;
        }
        _targetDoor = null;
        _state = newState;
    }

    /// <summary>
    /// Raycast from self to the door and check if a minion is in the way.
    /// </summary>
    /// <param name="door">The Door towards which raycasting.</param>
    /// <returns><b>True</b> if a minion is in the way.</returns>
    bool CheckIfMinionBlocksDoor(Door door)
    {
        Vector2 doorDir = door.transform.position - transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, doorDir.normalized, doorDir.magnitude);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.TryGetComponent(out Minion minion))
            {
                return true;
            }
        }
        return false;
    }
}

public enum MindState
{
    Idle = 0, Exploring = 1, Panicking = 2, Chased = 3, Enlightened = 4, Hunting = 10
}