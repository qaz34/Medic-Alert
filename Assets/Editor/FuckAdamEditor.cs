
using UnityEditor;
using UnityEngine;

public static class FuckAdamEditor
{
    [MenuItem("Seriously Adam/Fix It %f")]
    private static void FixAdamsShit()
    {
        var allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (var obj in allObjects)
        {
            if (obj.GetComponent<Collider>())
            {
                if (obj.transform.localScale.x < 0 || obj.transform.localScale.y < 0 || obj.transform.localScale.z < 0)
                {
                    Undo.RecordObject(obj, "Fuck Adam, you make my life so much more difficult");
                    obj.transform.localScale = new Vector3(Mathf.Abs(obj.transform.localScale.x), Mathf.Abs(obj.transform.localScale.y), Mathf.Abs(obj.transform.localScale.z));
                    Debug.Log("Seriously Adam, " + obj.name + " is sick of your shit, just like me");
                }
            }
        }
    }
}

