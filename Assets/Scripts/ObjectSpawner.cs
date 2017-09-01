using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class ObjectSpawner : NetworkBehaviour
{
    public GameObject objectToSpawn;
    public int lowTime, highTime;

    void OnEnable()
    {
        if (isServer)
        {
            StopAllCoroutines();
            StartCoroutine(Spawn());
        }

    }
    void Update()
    {

    }
    IEnumerator Spawn()
    {
        for (;;)
        {
            GameObject Te = Instantiate(objectToSpawn, transform.position, transform.rotation);
            NetworkServer.Spawn(Te);
            yield return new WaitForSeconds(Random.Range(lowTime, highTime));
        }
    }
}
