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
    Room _targetRoom = null, _lastRoom = null;
    Vector3 _inRoomTarget;
    float _stateTimer = 0, _idleStillTimer = 0;

    private void Start()
    {
        State = MindState.Idle;
    }

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
        if (_targetRoom == null)
        {
            // get the available rooms accessible from the currentRoom without the last room used, unless it's the only available one
            List<Room> availableRooms = GetAvailableRooms();
            if (_lastRoom != null && availableRooms.Count > 1)
            {
                availableRooms.Remove(_lastRoom);
            }
            // get a random room among the available ones
            Room nextRoom = availableRooms[Random.Range(0, availableRooms.Count)];
            // pass the chosen room to all the humans in the room
            Human[] group = CurrentRoom.humans.ToArray();
            Debug.LogWarning($"GroupSize = {group.Length}");
            for (int i = 0; i < group.Length; i++)
            {
                group[i].State = MindState.Exploring;
                group[i]._targetRoom = nextRoom;
                group[i]._targetNode = Bfs.GetNode(group[i].CurrentRoom.GetMiddleFloor());
            }
        }
        if (_targetRoom == null) {
            Debug.LogWarning($"{name} doesn't have a target door.");
        }
        MoveToTargetRoom();
    }

    void ActPanicking()
    {
        if (_targetRoom == null)
        {
            // get the available rooms in the currentRoom
            List<Room> availableRooms = GetAvailableRooms();
            // get a random room among the available ones
            _targetRoom = availableRooms[Random.Range(0, availableRooms.Count)];
            _targetNode = Bfs.GetNode(CurrentRoom.GetMiddleFloor());
        }
        MoveToTargetRoom();
    }

    void ActChased()
    {
        if (_targetRoom == null)
        {
            // get the available doors in the currentRoom that are opposite to the minions
            List<Door> availableDoors = new List<Door>();
            for (int i = 0; i < CurrentRoom.doors.Length; i++)
            {
                if (!CheckIfMinionBlocksDoor(CurrentRoom.doors[i]))
                {
                    availableDoors.Add(CurrentRoom.doors[i]);
                }
            }
            // get the available rooms based on those doors
            List<Room> availableRooms = GetAvailableRooms(availableDoors);
            // get a random door among the available ones or stay frightened
            if (availableRooms.Count > 0)
            {
                _targetRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                _targetNode = Bfs.GetNode(CurrentRoom.GetMiddleFloor());
            }
        }
        MoveToTargetRoom();
    }

    void ActEnlightened()
    {
        // ???
    }

    void MoveToTargetRoom()
    {
        if (_targetNode == null) { return; }
        CurrentRoom = FindCurrentRoom();
        if (Move(_targetNode.position)) {
            if (_targetRoom == null || _targetNode == null) {
                Debug.LogWarning($"_targetRoom: {_targetRoom}; _targetNode: {_targetNode}");
            }
            if (_targetRoom.GetMiddleFloor() == _targetNode.position) {
                _lastRoom = CurrentRoom;
                //CurrentRoom = _targetRoom;
                State = MindState.Idle;
                return;
            }
            _lastNode = _targetNode;
            _targetNode = FindNextNode();
        }
    }

    Vector3 GetTargetInRoom()
    {
        return Vector3.Lerp(CurrentRoom.floorLimits[0], CurrentRoom.floorLimits[1], Random.Range(0f, 1f));
    }

    protected Node FindNextNode() {
        return Bfs.GetNextNode(_targetNode.position, _targetRoom.GetMiddleFloor());
    }

    List<Room> GetAvailableRooms(List<Door> availableDoors = null) {
        Door[] roomDoors = availableDoors != null ? availableDoors.ToArray() : CurrentRoom.doors;
        List<Room> availableRooms = new List<Room>();
        for(int i = 0; i < roomDoors.Length; i++) {
            if(!availableRooms.Contains(roomDoors[i].targetDoor.room)) {
                availableRooms.Add(roomDoors[i].targetDoor.room);
            }
        }
        return availableRooms;
    }

    void OnSanityChange()
    {
        // if sanity becomes zero or smth
    }

    protected override void UnStun() {
        base.UnStun();
        State = MindState.Idle;
    }

    protected override void OnEnterRoom(Room room)
    {
        if (room == null) { return; }
        if (!room.humans.Contains(this))
        {
            room.humans.Add(this);
            _currentRoom = room;
        }
    }

    protected override void OnExitRoom(Room room)
    {
        if (room == null) { return; }
        if (room.humans.Contains(this))
        {
            room.humans.Remove(this);
            Debug.LogWarning($"a human left {room.name}");
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
        _targetRoom = null;
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