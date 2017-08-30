using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DamageBorder : MonoBehaviour
{
    public Color damaged;
    public Color Healed;
    public void Owch()
    {
        StopAllCoroutines();
        GetComponent<Image>().color = damaged;
        StartCoroutine(Flash());
    }
    public void OhhhYea()
    {
        StopAllCoroutines();
        GetComponent<Image>().color = Healed;
        StartCoroutine(Flash());
    }
    IEnumerator Flash()
    {
        Color temp = GetComponent<Image>().color;
        while (temp.a < .9f)
        {
            temp.a = Mathf.MoveTowards(temp.a, 1, .1f);
            GetComponent<Image>().color = temp;
            yield return new WaitForSeconds(.01f);
        }
        while (temp.a > .1f)
        {
            temp.a = Mathf.MoveTowards(temp.a, 0, .01f);
            GetComponent<Image>().color = temp;
            yield return new WaitForSeconds(.01f);
        }
        temp.a = 0;
        GetComponent<Image>().color = temp;
    }
}
