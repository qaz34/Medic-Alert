using UnityEngine;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{

    public string weapName
    {
        get
        {
            return weapons[equipWeapon].GetComponent<WeaponBase>().gunName;
        }
    }
    public List<GameObject> weapons = new List<GameObject>();
    public int equipWeapon = 0;
    public GameObject equipWep;
    // Use this for initialization
    void Start()
    {
        equipWep = Instantiate(weapons[equipWeapon], transform.position, transform.rotation);
        equipWep.transform.parent = transform;

    }
    public GameObject Fire(NetworkPosition pos, bool pressed)
    {
        return equipWep.GetComponent<WeaponBase>().Fire(pos, pressed);
    }
    public void FireNoBullet(bool pressed)
    {
        equipWep.GetComponent<WeaponBase>().FireNoBullet(pressed);
    }
    // Update is called once per frame
    public void Equip(int weapon)
    {
        equipWeapon = weapon;
        Destroy(equipWep);
        equipWep = Instantiate(weapons[weapon]);
        equipWep.transform.parent = transform;
    }
}
