using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Character {
    public MindState State {
        get { return _state; }
        set { SetMindState(value); }
    }
    public float Sanity {
        get { return _sanity; }
        set { _sanity = Mathf.Clamp(value, 0, sanityMax); }
    }

    [SerializeField] float _idleDuration = 10;
    [SerializeField] float _idleStillDuration = 1;
    [SerializeField] float sanityMax = 1;
    float _sanity = 1;
    MindState _state = MindState.Idle;
    Door _targetDoor = null, _lastDoor = null;
    Vector3 _inRoomTarget;
    float _stateTimer = 0, _idleStillTimer = 0;


    protected override void ChooseNextAction() {
        switch(State) {
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

    void ActIdle() {
        _stateTimer -= Time.deltaTime;
        if(_stateTimer <= 0) { // if the idleTimer expires, switch to Exploring
            State = MindState.Exploring;
            return;
        }
        // while the timer is ticking, wait a little bit then move to random positions in the room and repeat
        if (_idleStillTimer > 0) {
            _idleStillTimer -= Time.deltaTime;
        }
        if (_idleStillTimer <= 0) {
            if(Move(_inRoomTarget)) {
                _inRoomTarget = GetTargetInRoom();
                _idleStillTimer = _idleStillDuration;
            }
        }
    }

    void ActExploring() {
        if(_targetDoor == null) {
            // get the available doors in the currentRoom without the last door used, unless it's the only door
            List<Door> availableDoors = new List<Door>(currentRoom.doors);
            if(_lastDoor != null && availableDoors.Count > 1) {
                availableDoors.Remove(_lastDoor);
            }
            // get a random door among the available ones
            Door nextDoor = availableDoors[Random.Range(0, availableDoors.Count)];
            // pass the chosen door to all the humans in the room
            Human[] group = currentRoom.humans.ToArray();
            for(int i = 0; i < group.Length; i++) {
                group[i]._targetDoor = nextDoor;
            }
        }
        // move to the target door
        if(Move(_targetDoor.transform.position)) {
            if(_lastDoor == null) {
                _lastDoor = _targetDoor;
            }
            if(_lastDoor == _targetDoor.targetDoor) {
                State = MindState.Idle;
                return;
            }
            _lastDoor = _targetDoor;
            _targetDoor = _targetDoor.targetDoor;
        }
    }

    void ActPanicking() {

    }

    void ActChased() {

    }

    void ActEnlightened() {

    }

    protected override void OnEnterRoom() {

    }

    protected override Door GetNextDoor() {
        //
        return null;
    }

    Vector3 GetTargetInRoom() {
        return Vector3.Lerp(currentRoom.floorLimits[0], currentRoom.floorLimits[1], Random.Range(0f, 1f));
    }

    void SetMindState(MindState newState) {
        switch(newState) {
            case MindState.Idle:
                _stateTimer = _idleDuration;
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
}

public enum MindState {
    Idle = 0, Exploring = 1, Panicking = 2, Chased = 3, Enlightened = 4, Hunting = 10
}