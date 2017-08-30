using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int numAmmoGive;
    public float replenishTime;
    public GameObject ammoType;

    //enum AmmoType { Syringe }
    void SetActive(bool active)
    {
        GetComponent<Collider>().enabled = active;
        GetComponent<MeshRenderer>().enabled = active;
    }

    void Start()
    {
        StartCoroutine(replenishing());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInChildren<WeaponController>().equipWep.GetComponent<WeaponBase>().GainAmmo(numAmmoGive);
            StartCoroutine(replenishing());
            SetActive(false);
        }
    }

    IEnumerator replenishing()
    {
        yield return new WaitForSeconds(replenishTime);
        SetActive(true);
    }
}