using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : RoomObject, IPhysicObjectContactReceiver
{
    protected static Color s_GizmoCubeColor = new Color(0, 1, 0, 0.7f);

    protected readonly float PUSH_TIME = 0.5f;
    protected readonly float TIMEOUT_TIME = 0.2f;

    [System.Flags]
    public enum DIRECTION
    {
        DOWN = 1,
        RIGHT = 2,
        UP = 4,
        LEFT = 8
    }

    [EnumFlags]
    public DIRECTION pushableDirection;
    public int amount = 1;
    public float moveSpeed = 1.0f;

    protected float _sinceLastPush = 0.0f;
    protected float _sinceFirstPush = 0.0f;
    protected Vector2 _pushingDirection;

    protected bool _inMove = false;
    protected Vector3 _moveTarget;
    protected bool _isLocked = false;
    protected int _currentAmount;
    protected Vector3 _originalPosition;

    protected override void Start()
    {
        base.Start();

        _originalPosition = transform.position;
    }

    public override void OnRoomEntered()
    {
        if (!_isLocked)
        {
            transform.position = _originalPosition;
            _currentAmount = amount;
        }
    }

    void Update()
    {
        if (_inMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, _moveTarget, moveSpeed * Time.deltaTime);
            if (transform.position == _moveTarget)
                _inMove = false;
        }
        else
        {
            if (_sinceLastPush > 0.0f)
            {
                _sinceLastPush -= Time.deltaTime;
                _sinceFirstPush += Time.deltaTime;

                if (_sinceFirstPush > PUSH_TIME)
                {
                    _sinceFirstPush = 0.0f;
                    _sinceLastPush = 0.0f;
                    _inMove = true;
                    _currentAmount -= 1;
                    _moveTarget = transform.position + (Vector3)_pushingDirection;
                }
                else if(_sinceLastPush <= 0.0f)
                {
                    _pushingDirection = Vector2.zero;
                    _sinceFirstPush = 0.0f;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 start = transform.position - new Vector3(0.5f,0.5f);
        Vector3 end = transform.position + new Vector3(0.5f, 0.5f);

        if((pushableDirection & DIRECTION.DOWN) != 0)
            start.y -= amount;

        if ((pushableDirection & DIRECTION.LEFT) != 0)
            start.x -= amount;

        if ((pushableDirection & DIRECTION.UP) != 0)
            end.y += amount;

        if ((pushableDirection & DIRECTION.RIGHT) != 0)
            end.x += amount;

        Vector3 size = (end - start);
        Vector3 center = start + size * 0.5f;

        Gizmos.color = s_GizmoCubeColor;
        Gizmos.DrawCube(center, size);
    }

    public void Contact(Vector2 direction)
    {
        if (_currentAmount == 0)
            return;

        if(direction.x < 0 && (pushableDirection & DIRECTION.LEFT) != 0)
        {
            _sinceLastPush = TIMEOUT_TIME;

            if (_pushingDirection.x != direction.x)
            {
                _sinceFirstPush = 0.0f;
                _pushingDirection = direction;
            }
        }

        if (direction.x > 0 && (pushableDirection & DIRECTION.RIGHT) != 0)
        {
            _sinceLastPush = TIMEOUT_TIME;

            if (_pushingDirection.x != direction.x)
            {
                _sinceFirstPush = 0.0f;
                _pushingDirection = direction;
            }
        }

        if (direction.y < 0 && (pushableDirection & DIRECTION.DOWN) != 0)
        {
            _sinceLastPush = TIMEOUT_TIME;

            if (_pushingDirection.y != direction.y)
            {
                _sinceFirstPush = 0.0f;
                _pushingDirection = direction;
            }
        }

        if (direction.y > 0 && (pushableDirection & DIRECTION.UP) != 0)
        {
            _sinceLastPush = TIMEOUT_TIME;

            if (_pushingDirection.y != direction.y)
            {
                _sinceFirstPush = 0.0f;
                _pushingDirection = direction;
            }
        }

    }
}
