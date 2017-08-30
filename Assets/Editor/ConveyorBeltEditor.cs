using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ConveyorBelt))]
public class ConveyorBeltEditor : Editor
{
    void OnEnable()
    {
        SceneView.onSceneGUIDelegate -= OnScene;
        SceneView.onSceneGUIDelegate += OnScene;
    }
    ConveyorBelt belt;
    private void OnScene(SceneView sceneview)
    {
        belt = (ConveyorBelt)target;
        if (belt)
        {
            Handles.color = Color.red;
            Handles.ArrowHandleCap(0, belt.transform.position + new Vector3(0, 1, 0), Quaternion.LookRotation(belt.transform.forward, Vector3.up), 2, EventType.Repaint);
        }
        else
        {
            SceneView.onSceneGUIDelegate -= OnScene;
        }
    }
}
