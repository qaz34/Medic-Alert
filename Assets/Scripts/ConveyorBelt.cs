using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed = 1;
    private void FixedUpdate()
    {

    }
    private void OnTriggerStay(Collider collider)
    {

        if (collider.GetComponent<Rigidbody>())
        {
            Vector3 vel = collider.GetComponent<Rigidbody>().velocity;
            Vector3 dir = (Vector3.one - new Vector3(Mathf.Abs(transform.forward.x), Mathf.Abs(transform.forward.y), Mathf.Abs(transform.forward.z)));
            collider.GetComponent<Rigidbody>().velocity = transform.forward * speed;
            collider.GetComponent<Rigidbody>().velocity += new Vector3(vel.x * dir.x, vel.y * dir.y, vel.z * dir.z);
        }
        if (collider.transform.tag == "Player")
        {
            collider.GetComponent<PlayerCont>().outSideForce = transform.forward * speed;
        }
    }

}
