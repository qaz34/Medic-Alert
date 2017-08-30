using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Linq;
public class RespawnSettings : NetworkBehaviour
{
    [Tooltip("True will respawn after time, False will enter spectate")]
    public bool Respawns;
    [Tooltip("in seconds")]
    public float resTime = 1;

    List<GameObject> playToRes = new List<GameObject>(2);
    [Tooltip("empty that contains the spawnPoints")]
    public GameObject spawnPoints;
    public GameObject playerPrefab;
    List<Transform> spawns;

    private void Start()
    {
        spawns = spawnPoints.GetComponentsInChildren<Transform>().ToList();
        spawns.RemoveAt(0);

    }
    public void Respawn(GameObject playerToRespawn)
    {
        if (playToRes.Contains(playerToRespawn))
            return;
        playToRes.Add(playerToRespawn);

        //playToRes.Last().SetActive(false);
        //playToRes.Last().transform.position = new Vector3(1000, 1000, 1000);

        playerToRespawn.GetComponentInChildren<Camera>().enabled = false;
        playerToRespawn.GetComponent<PlayerCont>().enabled = false;
        playerToRespawn.GetComponent<PlayerHealth>().enabled = false;
        if (Respawns)
            StartCoroutine(ResDelay());
    }
    IEnumerator ResDelay()
    {
        yield return new WaitForSeconds(resTime);

        if (playToRes.First().GetComponent<PlayerCont>().isLocalPlayer)
        {
            playToRes.First().transform.position = spawns[Random.Range(0, spawns.Count - 1)].position;

        }
        Camera cam = playToRes.First().GetComponentInChildren<Camera>();

        cam.enabled = true;
        playToRes.First().GetComponent<PlayerCont>().enabled = true;
        playToRes.First().GetComponent<PlayerHealth>().enabled = true;
        playToRes.First().SetActive(true);
        playToRes.First().GetComponent<PlayerHealth>().Reset();
        foreach (var ammo in playToRes.First().GetComponent<PlayerCont>().AmmoStuck)
            Destroy(ammo);
        playToRes.First().GetComponent<PlayerCont>().AmmoStuck = new List<GameObject>();
        playToRes.RemoveAt(0);

    }


}
