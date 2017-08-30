using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedExplosion : MonoBehaviour
{

    public float boomForce = 0.4f;
    [Range(0, 1)]
    public float boomVariance = 1.0f;
    [Tooltip("inSecs")]
    public float destroyAfterTime = 1;
    public float destroyAfterDeviation = 1;
    void Start()
    {
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs)
        {
            float totalBoomForce = Random.Range(boomForce - (boomForce * boomVariance), boomForce + (boomForce * boomVariance));
            rb.AddExplosionForce(totalBoomForce, transform.position, 10, -.1f, ForceMode.Impulse);
        }
    }
}
