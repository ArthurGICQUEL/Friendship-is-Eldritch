using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Character
{
    [HideInInspector] public float insanityPower;

    [SerializeField] float _lifeTime = 1;
    Human _prey = null;
    bool _isMoving, _isCloseToPrey = false;
    float _timerLife;

    private void Start()
    {
        Human[] humans = FindObjectsOfType<Human>();
        AssignTarget(humans[Random.Range(0, humans.Length)]);
        _timerLife = _lifeTime;
    }

    protected override void ChooseNextAction()
    {
        //update lifetime
        _timerLife -= Time.deltaTime;
        if (_timerLife <= 0) { Destroy(gameObject); }

        // animation moving/idle
        bool tempMoving = _lastPos != transform.position;
        if (tempMoving != _isMoving) {
            _isMoving = tempMoving;
            _anim.SetBool("IsMoving", _isMoving);
        }

        if (_prey == null) { return; }
        // frighten the humans in the room its in
        if (CurrentRoom != null) {
            List<Human> humans = CurrentRoom.GetAvailableHumans();
            for (int i=0; i<humans.Count; i++) {
                humans[i].State = MindState.Chased;
            }
        }
        _speedRatio = _isCloseToPrey ? 1.5f : 1f;
        if (_isCloseToPrey) {
            if (LerpToward(_prey.transform.position)) {
                OnPreyReached();
            }
        } else {
            if(MoveToTargetNode()) {
                _isCloseToPrey = CurrentRoom != null && _prey.CurrentRoom != null && CurrentRoom == _prey.CurrentRoom;

                Room targetRoom = _prey.CurrentRoom ?? _prey._targetRoom;
                Node targetDoorNode = Bfs.GetNextNode(targetRoom.GetMiddleFloor(), _lastNode.position);

                _targetNode = Bfs.GetNextNode(_lastNode.position, targetDoorNode.position);
                //Debug.Log("targetRoom: " + targetRoom + "; targetDoorNode: " + targetDoorNode + "; _targetNode: " + _targetNode);
            }
        }
    }

    public void AssignTarget(Human target) {
        _prey = target;
        _targetNode = Bfs.GetNode(CurrentRoom.GetMiddleFloor());
    }

    void OnPreyReached() {
        Debug.LogWarning("Crunchy crunch, the prey has been reached!");
        _prey.Sanity -= insanityPower;
        Destroy(gameObject);
    }

    protected override void EnterRoom(Room room)
    {
        if (room == null) { return; }
        if (!room.minions.Contains(this))
        {
            room.minions.Add(this);
            _currentRoom = room;
        }
    }

    protected override void ExitRoom()
    {
        if (CurrentRoom == null) { return; }
        if (CurrentRoom.minions.Contains(this))
        {
            CurrentRoom.minions.Remove(this);
            _currentRoom = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Human human))
        {
            if (human == _prey)
            {
                OnPreyReached();
            }
        }
    }
}