using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(PlayerCont))]
public class PlayerEditor : Editor
{
    PlayerCont player;
    private void OnSceneGUI()
    {
        player = (PlayerCont)target;
        Handles.color = Handles.xAxisColor;
        Handles.SphereHandleCap(0, player.transform.position + new Vector3(0, player.topOfPlayer), player.transform.rotation, player.widthOfPlayer * 2, EventType.Repaint);
    }
}
