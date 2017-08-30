using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
[System.Serializable]
public struct Sounds
{
    public AudioClip fire;
    public AudioClip reload;
    public AudioSource audioSource;
}
public struct NetworkPosition
{
    public NetworkPosition(Vector3 forward, Vector3 pos)
    {
        forwards = forward;
        position = pos;
    }
    public Vector3 forwards;
    public Vector3 position;
}

[RequireComponent(typeof(AudioSource))]
public class WeaponBase : MonoBehaviour
{
    public string gunName;
    public Sounds sounds;
    public int healAmount;
    public float reloadTime;
    public float triggerDelay;
    protected float lastFired;
    public int curBullets;
    public bool infiniteAmmo;
    public int ammoRemaining;
    public int maxAmmo;
    public float speed;
    public int capacity;
    public GameObject ammunition;
    protected bool reloading;
    public GameObject bulletSpawn;

    public float closeness = 1;
    void Awake()
    {
        curBullets = capacity;
    }
    void Update()
    {


    }

    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadTime);
        if (ammoRemaining >= capacity - curBullets)
        {
            ammoRemaining -= capacity - curBullets;
            curBullets = capacity;
        }
        else if (ammoRemaining != 0)
        {
            curBullets += ammoRemaining;
            ammoRemaining = 0;
        }
        reloading = false;
    }
    public void Reload()
    {
        if (!reloading)
        {
            if (sounds.audioSource)
                sounds.audioSource.PlayOneShot(sounds.reload, .05f);
            reloading = true;
            StartCoroutine(Reloading());
        }
    }
    public virtual void FireNoBullet(bool pressed)
    {
        if (sounds.audioSource)
            sounds.audioSource.PlayOneShot(sounds.fire, .05f);
        if (Time.time - lastFired > triggerDelay)
        {
            lastFired = Time.time;
            curBullets--;
        }
    }
    public virtual GameObject Fire(NetworkPosition pos, bool pressed)
    {
        if (Time.time - lastFired > triggerDelay)
        {
            if (sounds.audioSource)
                sounds.audioSource.PlayOneShot(sounds.fire, .05f);
            GameObject bullet = Instantiate(ammunition);
            bullet.transform.position = bulletSpawn.transform.position;
            lastFired = Time.time;
            curBullets--;

            return bullet;
        }
        return null;
    }
    public void GainAmmo(int amount)
    {
        ammoRemaining = Mathf.Clamp(ammoRemaining + amount, 0, maxAmmo);
    }
}
