using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class ObjectSpawner : NetworkBehaviour
{
    public GameObject objectToSpawn;
    public int lowTime, highTime;
    // Use this for initialization
    //void Start()
    //{
    //    if (isServer)
    //        StartCoroutine(Spawn());
    //}
    void OnEnable()
    {
        //test comment for collab

        if (isServer)
            StartCoroutine(Spawn());
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
