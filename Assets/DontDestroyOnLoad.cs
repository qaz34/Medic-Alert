using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    static DontDestroyOnLoad dontDestroy;
    // Use this for initialization
    void Start()
    {
        if (dontDestroy)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        dontDestroy = this;
    }

}
