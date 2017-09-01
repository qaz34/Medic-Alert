using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InteractiableObject : MonoBehaviour
{
    public UnityEvent myEvent;
    // Use this for initialization
    public bool activated;
    public bool playerIn;
    void OnInteract()
    {
        myEvent.Invoke();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            playerIn = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            playerIn = false;
    }
    void Update()
    {
        if (playerIn && Input.GetButtonDown("Interact"))
        {
            Debug.Log("pressed");
            OnInteract();
        }
    }
}
