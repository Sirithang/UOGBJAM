using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WireGeneratorFloorButton : WireGenerator
{
    int _currentPressing = 0;

    void Reset()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IFloorButtonActivator activator = other.GetComponent<IFloorButtonActivator>();
        if (activator != null)
        {
            _currentPressing += 1;
            SwitchOn();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IFloorButtonActivator activator = other.GetComponent<IFloorButtonActivator>();
        if (activator != null)
        {
            _currentPressing += 1;
            SwitchOn();
        }
    }
}

public interface IFloorButtonActivator
{

}