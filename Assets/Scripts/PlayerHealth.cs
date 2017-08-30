using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerHealth : NetworkBehaviour
{
    [HideInInspector]
    public RespawnSettings resSettings;
    public int maxHealth = 100;
    [HideInInspector]
    public Image fillBar;
    [SyncVar]
    int currentHealth;
    [SyncVar(hook = "Dead"), HideInInspector]
    public bool isDead = false;
    public delegate void HealthChanged(int health);
    public event HealthChanged healthChange;
    [ClientRpc]
    void RpcHealthChanged(int value)
    {
        currentHealth = value;
        if (healthChange != null)
            healthChange(currentHealth);
        if (currentHealth == 0)
        {
            isDead = true;
            Dead(true);
        }

    }


    public void Dead(bool value)
    {
        isDead = value;
        if (isLocalPlayer && isDead)
        {
            resSettings.Respawn(gameObject);
        }
    }
    void SetStuff(Scene scene, LoadSceneMode mode)
    {
        if (GameObject.FindGameObjectWithTag("UI"))
        {
            GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>().player = gameObject;
        }
        resSettings = GameObject.FindGameObjectWithTag("GameController").GetComponent<RespawnSettings>();
    }
    public override void OnStartLocalPlayer()
    {
        CmdReset();
        SceneManager.sceneLoaded += SetStuff;
    }
    public void Reset()
    {
        currentHealth = maxHealth;
        isDead = false;
        CmdReset();
        if (healthChange != null)
            healthChange(currentHealth);
    }
    [Command]
    void CmdReset()
    {
        currentHealth = maxHealth;
        isDead = false;
        RpcHealthChanged(currentHealth);
    }

    public int health
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (!isServer || isDead)
                return;

            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            RpcHealthChanged(currentHealth);
            if (currentHealth == 0)
            {
                isDead = true;
                Dead(true);
            }
        }
    }
}

