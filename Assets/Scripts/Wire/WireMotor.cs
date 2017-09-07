using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireMotor : MonoBehaviour
{
    public int requiredPower = 1;

    [HideInInspector]
    public WireGenerator[] connectedGenerator;

    protected int _currentPower = 0;
    protected bool _poweredUp = false;

    protected virtual void Powered() { }
    protected virtual void Shutdown() { }

    public void ConnectPower()
    {
        _currentPower += 1;

        if(_currentPower >= requiredPower && !_poweredUp)
        {
            _poweredUp = true;
            Powered();
        }
    }

    public void DisconnectPower()
    {
        _currentPower -= 1;

        if(_poweredUp && _currentPower < requiredPower)
        {
            _poweredUp = false;
            Shutdown();
        }
    }
}
