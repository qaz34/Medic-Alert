using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class InteractiableObjectGroup : MonoBehaviour
{
    [HideInInspector]
    public List<InteractiableObject> objs;
    public GameObject Piano;
    public float TimeDelay;
    int NumberOfWinchesOn;
    public void Interact(InteractiableObject used)
    {
        if (!used.activated)
        {
            used.activated = true;
            used.GetComponent<MeshRenderer>().material.color = Color.blue;
            NumberOfWinchesOn++;
            if (NumberOfWinchesOn >= objs.Count)
                PianoFall();
        }
    }
    public void PianoFall()
    {
        foreach (var obj in objs)
        {
            obj.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        StartCoroutine(PianoDelay());
    }
    IEnumerator PianoDelay()
    {
        yield return new WaitForSeconds(TimeDelay);
        Piano.GetComponent<PianoFall>().falling = true;
    }
}
