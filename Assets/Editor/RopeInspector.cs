using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RopeEditor))]
public class RopeInspector : Editor
{
    private RopeEditor curve;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private const int lineSteps = 10;
    private const float directionScale = 0.5f;
    private const int stepsPerCurve = 10;
    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;
    private int selectedIndex = -1;
    private static Color[] modeColors = {
        Color.white,
        Color.yellow,
        Color.cyan
    };
    Vector3 VecXVec(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    public void MakeMesh(RopeEditor rope)
    {
        List<Vector3> newVerts = new List<Vector3>();
        List<Vector2> newUV = new List<Vector2>();

        Mesh mesh = new Mesh();
        mesh.name = "Hose Mesh";
        if (!rope.GetComponent<MeshFilter>())
            return;

        rope.GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();
        Vector2[] uv = new Vector2[(rope.resolution + 1) * (rope.resolution + 1)];

        float progress = 0;
        foreach (var joint in rope.gameobjs)
            DestroyImmediate(joint);
        rope.gameobjs.Clear();
        int xRes = curve.xRes;
        for (int i = 0; i <= rope.resolution; i++)
        {
            progress = (float)i / rope.resolution;
            Vector3 pos = rope.GetPoint(progress);
            for (int x = 0; x < xRes; x++)
            {
                float thing = ((float)x / xRes) * (2 * Mathf.PI);

                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(rope.RopePrefab);
                go.transform.forward = rope.GetDirection(progress);
                go.transform.position = pos;
                go.transform.Translate(new Vector3(Mathf.Cos(thing) * curve.width * 0.01f, Mathf.Sin(thing) * curve.width * 0.01f));
                go.transform.parent = rope.transform;
                rope.gameobjs.Add(go);
                rope.GetDirection(progress);
                newVerts.Add(go.transform.position - rope.transform.position);
                newUV.Add(new Vector2(progress * curve.NumberOfUvs, ((float)x / (xRes) > .5f) ? (1 - ((float)x / (xRes))) * 2 : 2 * (((float)x / (xRes)))));
            }
        }
        mesh.vertices = newVerts.ToArray();
        mesh.uv = newUV.ToArray();
        int[] triangles = new int[(curve.resolution * (curve.xRes)) * 6];
        int ring = xRes;
        int t = 0, v = 0;
        for (int i = 0; i < rope.resolution; i++, v++)
        {
            for (int q = 0; q < xRes - 1; q++, v++)
            {
                t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
            }
            t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
    }
    private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v10;
        triangles[i + 2] = triangles[i + 3] = v01;
        triangles[i + 5] = v11;
        return i + 6;
    }
    public void MakeRope(RopeEditor rope)
    {
        if (!rope.lr)
            return;
        foreach (var joint in rope.gameobjs)
            DestroyImmediate(joint);
        rope.gameobjs.Clear();

        float progress = 0;
        for (float i = 0; i <= rope.resolution; i++)
        {
            progress = i / rope.resolution;
            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(rope.RopePrefab);
            go.transform.position = rope.GetPoint(progress);
            go.transform.parent = rope.transform;
            rope.gameobjs.Add(go);
            if (i == 0)
            {
                go.GetComponent<ConfigurableJoint>().connectedBody = rope.connetionRB;
            }
            else
            {
                go.GetComponent<ConfigurableJoint>().connectedBody = rope.gameobjs[(int)i - 1].GetComponent<Rigidbody>();
            }
            if (i == rope.resolution)
                go.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (rope.DrawLines)
        {
            rope.lr.enabled = true;
            rope.lr = rope.GetComponent<LineRenderer>();
            rope.lr.positionCount = rope.gameobjs.Count;
            for (int i = 0; i < rope.gameobjs.Count; i++)
            {
                rope.lr.SetPosition(i, rope.gameobjs[i].transform.position);
            }
        }
        else
        {
            rope.lr.enabled = false;
        }
    }
    private void OnSceneGUI()
    {
        curve = target as RopeEditor;
        // Debug.Log(PrefabUtility.GetPrefabType(curve.gameobjs[0]));
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < curve.ControlPointCount; i += 3)
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }
        ShowDirections();

    }

    public override void OnInspectorGUI()
    {
        curve = target as RopeEditor;

        EditorGUI.BeginChangeCheck();
        GameObject ropeFab = (GameObject)EditorGUILayout.ObjectField("Rope Prefab", curve.RopePrefab, typeof(GameObject), false);
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(curve);
            curve.RopePrefab = ropeFab;
            MakeRope(curve);
        }
        //  public GameObject ropeJoint;
        //  public Rigidbody connetionRB;
        EditorGUI.BeginChangeCheck();
        Rigidbody connectedHead = (Rigidbody)EditorGUILayout.ObjectField("Rope connectionPoint", curve.connetionRB, typeof(Rigidbody), true);
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(curve);
            curve.connetionRB = connectedHead;
            MakeMesh(curve);
        }

        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", curve.Loop);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Toggle Loop");
            EditorUtility.SetDirty(curve);
            curve.Loop = loop;
            MakeMesh(curve);
        }
        EditorGUI.BeginChangeCheck();
        bool Lines = EditorGUILayout.Toggle("Lines", curve.DrawLines);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Toggle Lines");
            EditorUtility.SetDirty(curve);
            curve.DrawLines = Lines;
            MakeMesh(curve);
        }
        if (selectedIndex >= 0 && selectedIndex < curve.ControlPointCount)
        {
            DrawSelectedPointInspector();

        }
        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(curve, "Add Curve");
            curve.AddCurve();
            EditorUtility.SetDirty(curve);
            MakeMesh(curve);
        }
        EditorGUI.BeginChangeCheck();
        int res = EditorGUILayout.IntField("Length Resolution", curve.Res);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Change Resolution");
            EditorUtility.SetDirty(curve);
            curve.Res = res;
            MakeMesh(curve);
        }
        EditorGUI.BeginChangeCheck();
        int xRes = EditorGUILayout.IntField("Perimiter Resolution", curve.xRes);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Change xRes");
            EditorUtility.SetDirty(curve);
            if (xRes % 2 == 0)
                curve.xRes = xRes;
            else
                curve.xRes = xRes + 1;
            MakeMesh(curve);
        }
        EditorGUI.BeginChangeCheck();
        float width = EditorGUILayout.FloatField("Width", curve.width);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Change width");
            EditorUtility.SetDirty(curve);
            curve.width = width;
            MakeMesh(curve);
        }
        EditorGUI.BeginChangeCheck();
        int pulse = EditorGUILayout.IntField("Pulses", curve.NumberOfUvs);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Change uv");
            EditorUtility.SetDirty(curve);
            curve.NumberOfUvs = pulse;
            MakeMesh(curve);
        }
        if (GUILayout.Button("Bake Mesh"))
        {
            Undo.RecordObject(curve, "Bake Mesh");
            MakeMesh(curve);
            EditorUtility.SetDirty(curve);
        }
    }
    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = curve.GetPoint(0f);
        Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
        int steps = stepsPerCurve * curve.CurveCount;
        for (int i = 1; i <= steps; i++)
        {
            point = curve.GetPoint(i / (float)steps);
            Handles.DrawLine(point, point + curve.GetDirection(i / (float)steps) * directionScale);
        }
    }
    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(curve.GetControlPoint(index));
        float size = HandleUtility.GetHandleSize(point);
        if (index == 0)
        {
            size *= 2f;
        }
        Handles.color = modeColors[(int)curve.GetControlPointMode(index)];
        if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotHandleCap))
        {
            selectedIndex = index;
            Repaint();
        }
        if (selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(curve, "Move Point");
                EditorUtility.SetDirty(curve);
                curve.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
            }
        }
        return point;
    }
    private void DrawSelectedPointInspector()
    {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", curve.GetControlPoint(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.SetControlPoint(selectedIndex, point);
        }
        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)
            EditorGUILayout.EnumPopup("Mode", curve.GetControlPointMode(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Change Point Mode");
            curve.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(curve);
        }
    }
}
