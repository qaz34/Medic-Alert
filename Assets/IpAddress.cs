using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;
public class IpAddress : MonoBehaviour
{
    Text text;

    // Use this for initialization
    void OnEnable()
    {
        text = GetComponent<Text>();
        text.text = text.text.Replace("$IP", Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString());

    }

    // Update is called once per frame
    void Update()
    {

    }
}
