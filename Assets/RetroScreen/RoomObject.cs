using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    public System.Action onRoomActivated;
    public System.Action onRoomDeactivated;

    public int room { get { return _room; } }

    protected int _room = -1;

    protected virtual void Start()
    {
        RoomCell c = RoomManager.Instance.GetCellFromWorld(transform.position);

        if(c == null)
        {
            Debug.LogError("This room object is outside of a room, disabling", gameObject);
        }
        else
        {
            _room = c.room;
            RoomManager.Instance.RegisterRoomObject(_room, this);
        }

        //disabling the room object, it will be reenabled when the room become active
        gameObject.SetActive(false);
    }

    protected virtual void OnDestroy()
    {
        RoomManager.Instance.UnregisterRoomObject(this);
    }
}
