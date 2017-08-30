using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;
public class PlayerPanel : MonoBehaviour
{
    public int health;
    public int score;
    public GameObject player;
    public Text healthText;
    public Text usernameText;
    public Image fillBar;
    public void SubTo()
    {
        player.GetComponent<PlayerHealth>().healthChange += HealthChangedHandle;
        player.GetComponent<PlayerCont>().nameChanged += NameChanged;
    }
    void HealthChangedHandle(int newHealth)
    {
        health = newHealth;
        healthText.text = health.ToString();
        fillBar.fillAmount = (float)health / player.GetComponent<PlayerHealth>().maxHealth;
        GetComponentInParent<PlayerLeaderBoard>().Sort();
    }
    void NameChanged(string name)
    {
        usernameText.text = name;
    }
}
