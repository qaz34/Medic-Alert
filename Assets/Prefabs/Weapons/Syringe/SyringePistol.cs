using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyringePistol : WeaponBase
{
    public override void FireNoBullet(bool pressed)
    {
        if (curBullets > 0 || infiniteAmmo)
        {
            base.FireNoBullet(pressed);

        }
        else
        {
            Reload();
        }

    }


    public override GameObject Fire(NetworkPosition pos, bool pressed)
    {

        if (curBullets > 0 || infiniteAmmo)
        {
            RaycastHit hit;
            if (Physics.Raycast(pos.position, pos.forwards, out hit, Mathf.Infinity))
            {
                GameObject bullet = base.Fire(pos, pressed);
                if (bullet)
                {
                    bullet.transform.position = bulletSpawn.transform.position;
                    if (Vector3.Distance(pos.position, hit.point) > closeness)
                    {
                        bullet.transform.LookAt(hit.point);
                    }
                    else
                    {
                        bullet.transform.forward = bulletSpawn.transform.forward;
                    }
                    bullet.GetComponent<Syringe>().speed = speed;

                    return bullet;
                }
            }
            else
            {
                Debug.Log("aiming at nothing");
            }

        }
        else
        {
            Reload();
        }
        return null;
    }
}
