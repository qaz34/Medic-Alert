using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    DestroyedExplosion temp;
    public bool parent = false;
    // Use this for initialization
    void Start()
    {
        temp = GetComponentInParent<DestroyedExplosion>();
        Destroy(gameObject, (!parent) ? Random.Range(temp.destroyAfterTime - temp.destroyAfterDeviation, temp.destroyAfterTime + temp.destroyAfterDeviation) : temp.destroyAfterTime + temp.destroyAfterDeviation);
    }
}
