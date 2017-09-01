using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PianoFall : NetworkBehaviour
{
    [SyncVar]
    public Vector3 velocity;
    [SyncVar]
    public Vector3 position;
    [SyncVar]
    public bool falling;
    // Use this for initialization
    void FixedUpdate()
    {
        if (!falling)
            return;
        if (isServer)
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, velocity.y * Time.deltaTime * 2))
            {
                if (hit.transform.tag == "Player")
                {
                    if (hit.transform.GetComponent<CharacterController>().isGrounded)
                    {
                        //damage
                    }
                }
                else
                {
                    transform.position = hit.point + new Vector3(0, .5f, 0);
                    falling = false;
                }
            }
            else
            {
                transform.position += velocity * Time.deltaTime;
                position = transform.position;
            }
        }
        else if (isClient)
        {
            transform.position = Vector3.Lerp(position, transform.position, .5f);
            transform.position += velocity;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            if (other.gameObject.GetComponent<CharacterController>().isGrounded)
            {
                //damage
            }
        }
        else
        {
            transform.position += other.impulse;
            falling = false;
        }
    }
}
