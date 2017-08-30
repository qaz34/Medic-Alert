using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeaderBoard : MonoBehaviour
{
    public GameObject panel;
    public float offset = 2.5f;
    [HideInInspector]
    public List<GameObject> panels;
    public enum SortBy
    {
        Health,
        Score
    }
    public SortBy sortBy;
    public void NewPlayer(GameObject player)
    {
        GameObject go = Instantiate(panel, transform, false);
        PlayerPanel goP = go.GetComponent<PlayerPanel>();
        goP.player = player;
        goP.health = player.GetComponent<PlayerHealth>().health;
        goP.usernameText.text = player.GetComponent<PlayerCont>().username;
        goP.SubTo();
        panels.Add(go);
        Sort();
    }
    public void UserNameSet()
    {
        Sort();
    }
    public void Sort()
    {

        panels.Sort(delegate (GameObject x, GameObject y)
        {
            switch (sortBy)
            {
                case SortBy.Health: return x.GetComponent<PlayerPanel>().health.CompareTo(y.GetComponent<PlayerPanel>().health);
                case SortBy.Score: return x.GetComponent<PlayerPanel>().score.CompareTo(y.GetComponent<PlayerPanel>().score);
                default: return x.GetComponent<PlayerPanel>().health.CompareTo(y.GetComponent<PlayerPanel>().health);
            }
        });
        int i = (int)Mathf.Floor(panels.Count / 2);
        foreach (var pan in panels)
        {
            Vector3 tempPos = pan.transform.localPosition;
            float currentOffset;
            if (i == 0) currentOffset = 0;
            else currentOffset = offset;
            tempPos.y = (panels.Count % 2 == 0) ? i * (pan.GetComponent<RectTransform>().rect.height) - pan.GetComponent<RectTransform>().rect.height / 2 + offset : i * (pan.GetComponent<RectTransform>().rect.height) + offset;
            pan.transform.localPosition = tempPos;
            i--;
        }
    }
}
