using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFacingCamera : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Camera cam = Camera.main;
        if (cam)
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);

    }
}
