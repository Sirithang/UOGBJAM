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
[CustomEditor(typeof(WireGenerator),true)]
class WireGeneratorEditor : Editor
{
    WireGenerator _generator;

    bool _isLinking = false;

    void OnEnable()
    {
        _generator = target as WireGenerator;
        _isLinking = false;
    }

    public override void OnInspectorGUI()
    {
        //do the normal editor, usefull for subclass
        base.OnInspectorGUI();
    }

    void OnDisable()
    {
        if (_isLinking)
        {
            for(int i = 0; i < SceneView.sceneViews.Count; ++i)
                ((SceneView)SceneView.sceneViews[i]).SetSceneViewFiltering(true);

            SceneModeUtility.SearchForType(null);
        }
    }

    void OnSceneGUI()
    {
        SceneView currentSceneView = SceneView.currentDrawingSceneView;

        Vector3 screenPos = currentSceneView.camera.WorldToScreenPoint(_generator.transform.position);
        screenPos.y = SceneView.currentDrawingSceneView.position.height - screenPos.y;

        Rect position = new Rect(screenPos.x, screenPos.y, 50, 16);

        if (!_isLinking)
        {
            Handles.BeginGUI();

            if (GUI.Button(position, "+"))
            {
                _isLinking = true;
                currentSceneView.SetSceneViewFiltering(true);
                SceneModeUtility.SearchForType(typeof(WireMotor));
            }

            int toRemove = -1;
            for(int i = 0; i < _generator.connectedMotor.Length; ++i)
            {
                screenPos = currentSceneView.camera.WorldToScreenPoint(_generator.connectedMotor[i].transform.position);
                screenPos.y = SceneView.currentDrawingSceneView.position.height - screenPos.y;


                Handles.DrawLine(_generator.transform.position, _generator.connectedMotor[i].transform.position);

                if (GUI.Button(new Rect(screenPos, new Vector2(50,16)), "-"))
                {
                    toRemove = i;
                }
            }

            if(toRemove != -1)
            {
                WireMotor motor = _generator.connectedMotor[toRemove];
                ArrayUtility.Remove(ref _generator.connectedMotor, motor);
                ArrayUtility.Remove(ref motor.connectedGenerator, _generator);

                EditorUtility.SetDirty(_generator);
                EditorUtility.SetDirty(motor);
            }

            Handles.EndGUI();

            Handles.color = Color.green;
            for (int i = 0; i < _generator.connectedMotor.Length; ++i)
            {
                Handles.DrawLine(_generator.transform.position, _generator.connectedMotor[i].transform.position);
            }
        }
        else
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            if(Event.current.type == EventType.mouseDown)
            {
                GUIUtility.hotControl = controlID;
                Event.current.Use();
            }
            else if(Event.current.type == EventType.mouseUp)
            {
                GameObject obj = HandleUtility.PickGameObject(Event.current.mousePosition, true);
                WireMotor motor = obj == null? null : obj.GetComponent<WireMotor>();
                
                if(motor != null)
                {

                    if (!ArrayUtility.Contains(_generator.connectedMotor, motor))
                    {
                        ArrayUtility.Add(ref _generator.connectedMotor, motor);
                        ArrayUtility.Add(ref motor.connectedGenerator, _generator);

                        EditorUtility.SetDirty(_generator);
                        EditorUtility.SetDirty(motor);
                    }
                }

                _isLinking = false;
                currentSceneView.SetSceneViewFiltering(false);
                SceneModeUtility.SearchForType(null);
            }
        }
    }
}
#endif