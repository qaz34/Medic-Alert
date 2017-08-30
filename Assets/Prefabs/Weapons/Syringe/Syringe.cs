using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(Rigidbody))]
public class Syringe : NetworkBehaviour
{
    [Tooltip("How long ammo exists")]
    public float existTime;
    [Tooltip("Does the ammo stay alive after collision")]
    public bool permanence;
    [Tooltip("Does the ammo stick to other GameObjects")]
    public bool stickTo;
    public bool stuck = false;
    public float stickTime;
    public int healAmount;
    public string owner;
    [SyncVar]
    public float speed;
    [Tooltip("approx 1/2 width")]
    public float offset = .001f;
    Rigidbody rb;
    // Use this for initialization
    void Start()
    {
        // Destroy(gameObject, existTime);
        rb = GetComponent<Rigidbody>();
    }

    void Stuck(Transform stickTransform)
    {
        if (!stuck)
            if (stickTo)
            {
                Transform thing = GetComponentsInChildren<Transform>()[1];
                if (stickTransform.localScale == Vector3.one)
                {
                    thing.parent = stickTransform;
                }
                else
                {
                    var go = new GameObject(stickTransform.name);
                    stickTransform.parent = go.transform;
                    thing.parent = go.transform;
                }

            }
    }
    Vector3 vecTimes(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        if (rb.SweepTest(transform.forward, out hitInfo, speed * Time.deltaTime * 2) && hitInfo.transform.gameObject.layer != LayerMask.NameToLayer("NoStick"))
        {
            if (!stuck)
            {
                if (hitInfo.transform.GetComponent<Syringe>() && isServer)
                {
                    Destroy(hitInfo.rigidbody.gameObject, Time.deltaTime);
                    Destroy(gameObject, Time.deltaTime);
                }
                else if (hitInfo.transform.tag == "Player" && isServer)
                {
                    hitInfo.transform.GetComponent<PlayerHealth>().health += healAmount;
                    hitInfo.transform.GetComponent<PlayerCont>().AmmoStuck.Add(GetComponentsInChildren<Transform>()[1].gameObject);
                }
                transform.position += transform.forward * hitInfo.distance;
                Stuck(hitInfo.transform.root);
                stuck = true;
                existTime = stickTime;
                if (isServer)
                    Destroy(gameObject);
            }

        }
        else
            transform.position += transform.forward * speed * Time.deltaTime;

    }
}