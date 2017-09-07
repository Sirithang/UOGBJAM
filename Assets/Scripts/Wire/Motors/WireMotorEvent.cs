using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WireMotorEvent : WireMotor
{
    public UnityEvent onPowerUp;
    public UnityEvent onPowerDown;

    protected override void Powered()
    {
        base.Powered();

        onPowerDown.Invoke();
    }

    protected override void Shutdown()
    {
        base.Shutdown();

        onPowerDown.Invoke();
    }
}
