using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OverheadNamePlates : MonoBehaviour
{
    public Text usernameText;
    public Text healthText;

    public Image overheadHealthbar;

    // Update is called once per frame
    void Update()
    {
        overheadHealthbar.fillAmount = (float)GetComponentInParent<PlayerHealth>().health / GetComponentInParent<PlayerHealth>().maxHealth;
        usernameText.text = GetComponentInParent<PlayerCont>().username;
        healthText.text = GetComponentInParent<PlayerHealth>().health.ToString();
    }
}
