using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;

public class ObjectSpawnRandomizer : NetworkBehaviour
{
    List<Transform> objectSpawners;
    public float randomizeTime;
    public GameObject spawners;
    // Use this for initialization
    void Start()
    {
        if (isServer)
        {
            objectSpawners = spawners.GetComponentsInChildren<Transform>().ToList();
            objectSpawners.RemoveAt(0);
            StartCoroutine(Change());
        }
    }

    public GameObject randomSpawner()
    {
        int randomindex = Random.Range(0, objectSpawners.Count - 1);
        return objectSpawners[randomindex].gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Change()
    {
        while (true)
        {
            foreach (var go in objectSpawners)
            {
                go.gameObject.SetActive(false);
            }
            randomSpawner().SetActive(true);
            randomSpawner().SetActive(true);
            yield return new WaitForSeconds(randomizeTime);

        }

    }

}
