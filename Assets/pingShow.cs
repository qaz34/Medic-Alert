using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class pingShow : MonoBehaviour
{

    void Update()
    {
        GetComponent<Text>().text = NetworkManager.singleton.client.GetRTT().ToString();
    }
}
