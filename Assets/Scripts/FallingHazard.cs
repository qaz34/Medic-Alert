using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(Rigidbody))]
public class FallingHazard : NetworkBehaviour
{
    float max;
    Rigidbody rb;

    public GameObject brokenPrefab;
    [Space(10)]
    public AnimationCurve curve;
    public float minFall = 1;
    public float maxFall = 10;
    public int relitiveDamage = 100;
    float fall = 0;
    bool grounded;
    bool destroyed = false;
    float sinceLastPacket;
    [Space(10)]
    public float SyncDelay;
    public struct RbValues
    {
        public Vector3 velocity;
        public Vector3 pos;
        public Quaternion rot;
        public RbValues(Rigidbody ridg)
        {
            velocity = ridg.velocity;
            pos = ridg.position;
            rot = ridg.rotation;

        }
        public void SetRB(Rigidbody rb)
        {
            rb.velocity = velocity;
            rb.position = pos;
            rb.rotation = rot;
        }

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    //void FixedUpdate()
    //{
    //    if (sinceLastPacket - Time.time > SyncDelay)
    //        if (isServer)
    //        {
    //            sinceLastPacket = SyncDelay;
    //            RpcSync(new RbValues(rb));
    //        }
    //}
    private void FixedUpdate()
    {
        if (!isServer)
            return;

        if (Physics.Raycast(transform.position, Vector3.down, transform.lossyScale.y + .1f))
        {
            grounded = true;
            max = 0;
        }
        else
            grounded = false;

        if (grounded == false)
            if (rb.velocity.y < 0 && max == 0)
                max = transform.position.y;
            else
                fall = max - transform.position.y;

    }
    [ClientRpc]
    void RpcDestroyBox(Vector3 pos)
    {
        rb.MovePosition(pos);
        DestroyCrate();
    }
    //[ClientRpc]
    //void RpcSync(RbValues sRb)
    //{
    //    if (rb && !isServer)
    //        sRb.SetRB(rb);
    //}
    private void OnCollisionEnter(Collision collision)
    {
        if (isServer)
        {
            max = 0;
            if (collision.transform.tag == "Player")
            {
                if (fall > minFall)
                {
                    float damagePercent = curve.Evaluate((fall / maxFall));
                    collision.gameObject.GetComponent<PlayerHealth>().health -= (int)Mathf.Ceil(relitiveDamage * damagePercent);
                    fall = 0;
                    //RpcDestroyBox(transform.position);
                    DestroyCrate();
                }
            }
            else if (fall > minFall)
            {
                //RpcDestroyBox(transform.position);
                DestroyCrate();
            }
        }
    }
    private void DestroyCrate()
    {
        if (!destroyed)
        {
            try
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<BoxCounter>().boxesForInstance++;
            }
            catch
            {

            }
            var go = Instantiate(brokenPrefab, transform.position, transform.rotation);
            foreach (Rigidbody rb in go.GetComponentsInChildren<Rigidbody>())
            {
                rb.velocity = GetComponent<Rigidbody>().velocity;
                rb.angularVelocity = GetComponent<Rigidbody>().angularVelocity;
            }
            NetworkServer.Spawn(go);
            NetworkServer.Destroy(gameObject);
            //Destroy(gameObject);
            destroyed = true;

        }
    }
}
