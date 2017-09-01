using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
[CustomEditor(typeof(InteractiableObjectGroup))]
public class InteractiableObjectsInspector : Editor
{
    InteractiableObjectGroup inter;
    public override void OnInspectorGUI()
    {
        inter = (InteractiableObjectGroup)target;
        DrawDefaultInspector();
        var objects = inter.GetComponentsInChildren<InteractiableObject>();
        inter.objs = objects.ToList();
        EditorGUILayout.TextArea("There are " + inter.objs.Count.ToString() + " winches");
    }


}
