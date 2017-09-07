using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WireGenerator : MonoBehaviour
{
    [HideInInspector]
    public WireMotor[] connectedMotor;

    protected bool _powered = false;

    public void SwitchOn()
    {
        if (_powered)
            return;

        _powered = true;

        for(int i = 0; i < connectedMotor.Length; ++i)
        {
            connectedMotor[i].ConnectPower();
        }
    }

    public void SwitchOff()
    {
        if (!_powered)
            return;

        _powered = false;

        for(int i = 0; i < connectedMotor.Length; ++i)
        {
            connectedMotor[i].DisconnectPower();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(WireGenerator), true)]
class WireGeneratorEditor : Editor
{
    WireGenerator _generator;

    void OnEnable()
    {
        _generator = target as WireGenerator;
    }

    public override void OnInspectorGUI()
    {
        //do the normal editor, usefull for subclass
        base.OnInspectorGUI();
    }

    void OnSceneGUI()
    {
        Vector3 screenPos = GUIUtility.ScreenToGUIPoint(SceneView.currentDrawingSceneView.camera.WorldToScreenPoint(_generator.transform.position));
        Rect position = new Rect(screenPos.x - 25, screenPos.y - 8, 50, 16);

        Handles.BeginGUI();

        GUI.Button(position, "+");

        Handles.EndGUI();
    }
}
#endif