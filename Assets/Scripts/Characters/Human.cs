using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        set { _sanity = Mathf.Clamp(value, 0, _sanityMax); OnSanityChange(); }
    }
    public bool IsEnlightened {
        get { return State == MindState.Enlightened; }
    }
    public bool IsPossessed {
        get { return State == MindState.Enlightened; }
    }

    [SerializeField] float _speedRatioIdle = 0.5f;
    [SerializeField] float _speedRatioExploring = 1f;
    [SerializeField] float _speedRatioPanicking = 1.5f;
    [SerializeField] float _speedRatioChased = 2f;
    [SerializeField] float _idleDuration = 10;
    [SerializeField] float _idleStillDuration = 1;
    [SerializeField] float _sanityMax = 1;
    float _possessedDuration;
    float _sanity = 1;
    Slider _sanitySlider;
    MindState _state = MindState.Idle;
    [HideInInspector] public Room _targetRoom = null;
    Vector3 _inRoomTarget;
    float _stateTimer = 0, _idleStillTimer = 0, _possessedTimer = 0;

    protected override void Awake() {
        base.Awake();
        _sanitySlider = GetComponentInChildren<Slider>();
        Sanity = _sanityMax;

        //_lastNode = Bfs.GetNode(CurrentRoom.GetMiddleFloor());
    }

    private void Start()
    {
        State = MindState.Idle;
        _lastNode = Bfs.GetNode(CurrentRoom.GetMiddleFloor());
        //CurrentRoom = FindCurrentRoom();
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
            case MindState.Possessed:
                ActPossessed();
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
            // start moving
            _inRoomTarget = GetTargetInRoom(); // get next Target
            _anim.SetInteger("MindState", (int)MindState.Exploring);
        }
        if (_idleStillTimer <= 0)
        {
            if (LerpToward(_inRoomTarget))
            {
                // stop moving for now
                _idleStillTimer = _idleStillDuration;
                _anim.SetInteger("MindState", (int)MindState.Idle);
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
            Human[] group = CurrentRoom.GetAvailableHumans().ToArray();
            //Debug.LogWarning($"GroupSize = {group.Length}; NextRoom: {nextRoom}");
            for (int i = 0; i < group.Length; i++)
            {
                if (group[i] != this) {
                    group[i].State = MindState.Exploring;
                }
                group[i]._targetRoom = nextRoom;
                group[i]._targetNode = group[i].FindNextNode(true);
            }
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
            _targetNode = FindNextNode(true);
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
                _targetNode = FindNextNode(true);
            }
        }
        MoveToTargetRoom();
    }

    void ActEnlightened()
    {
        // ???
    }

    void ActPossessed() {
        _possessedTimer -= Time.deltaTime;
        if (_possessedTimer <= 0) {
            AudioManager.Instance.Stop("Possession");
            State = MindState.Idle;
        }
    }

    void MoveToTargetRoom()
    {
        if(_targetRoom == null) return;
        if (MoveToTargetNode()) {
            // if arrived in the target room, become idle
            if (CurrentRoom != null && CurrentRoom == _targetRoom)
            {
                State = MindState.Idle;
                return;
            }
            // if not the target room, then find the next node
            _targetNode = FindNextNode();
        }
    }

    public void Possess(float duration) {
        _possessedDuration = duration;
        State = MindState.Possessed;
        AudioManager.Instance.Play("Possession");
    }

    Vector3 GetTargetInRoom()
    {
        if(CurrentRoom == null) Debug.LogError("CurrentRoom is null");
        return Vector3.Lerp(CurrentRoom.floorLimits[0], CurrentRoom.floorLimits[1], Random.Range(0f, 1f));
    }

    Node FindNextNode(bool fromRoom = false)
    {
        if (fromRoom) {
            _lastNode = Bfs.GetNode(CurrentRoom.GetMiddleFloor());
        }
        //Debug.Log($"lastNode: {_lastNode}; targetRoom: {_targetRoom}");
        return Bfs.GetNextNode(_lastNode.position, _targetRoom.GetMiddleFloor());
    }

    List<Room> GetAvailableRooms(List<Door> availableDoors = null)
    {
        Door[] roomDoors = availableDoors != null ? availableDoors.ToArray() : CurrentRoom.doors;
        List<Room> availableRooms = new List<Room>();
        for (int i = 0; i < roomDoors.Length; i++)
        {
            if (!availableRooms.Contains(roomDoors[i].targetDoor.room))
            {
                availableRooms.Add(roomDoors[i].targetDoor.room);
            }
        }
        return availableRooms;
    }

    void OnSanityChange()
    {
        _sanitySlider.value = Sanity;
        if (Sanity == 0)
        {
            State = MindState.Enlightened;
        }
    }

    protected override void EnterRoom(Room room)
    {
        if (room == null) { return; }
        if (!room.humans.Contains(this))
        {
            room.humans.Add(this);
            _currentRoom = room;
        }
    }

    protected override void ExitRoom()
    {
        if (CurrentRoom == null) { return; }
        if (CurrentRoom.humans.Contains(this))
        {
            CurrentRoom.humans.Remove(this);
            _currentRoom = null;
            //Debug.LogWarning($"a human left {room.name}");
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
                if (State == MindState.Chased) { break; }
                _speedRatio = _speedRatioChased;
                break;
            case MindState.Enlightened:
                GameManager.Instance.EndGame();
                break;
            case MindState.Possessed:
                _possessedTimer = _possessedDuration;
                break;
            default:
                break;
        }
        _anim.SetInteger("MindState", (int)newState);
        _anim.speed = _baseSpeed * _speedRatio;
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
    Idle = 0, Exploring = 1, Panicking = 2, Chased = 3, Enlightened = 4, Possessed = 5
}