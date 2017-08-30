using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{

    public RopeEditor spline;
    public List<GameObject> gameobjs;
    public GameObject ropeJoint;
    public Rigidbody connetionRB;
    public float resolution;
    [HideInInspector]
    public LineRenderer lr;
    private void Start()
    {
        if (gameobjs.Count == 0)
        {
            float progress = 0;
            for (float i = 0; i <= resolution; i++)
            {
                progress = i / resolution;
                GameObject go = GameObject.Instantiate<GameObject>(ropeJoint, spline.GetPoint(progress), new Quaternion(), transform);
                gameobjs.Add(go);
                if (i == 0)
                {
                    go.GetComponent<ConfigurableJoint>().connectedBody = connetionRB;
                }
                else
                {
                    go.GetComponent<ConfigurableJoint>().connectedBody = gameobjs[(int)i - 1].GetComponent<Rigidbody>();
                }
                if (i == resolution)
                    go.GetComponent<Rigidbody>().isKinematic = true;
            }
            lr = GetComponent<LineRenderer>();
            lr.positionCount = gameobjs.Count;
        }
    }
    void Update()
    {
        for (int i = 0; i < gameobjs.Count; i++)
        {
            lr.SetPosition(i, gameobjs[i].transform.position);
        }
    }
}

