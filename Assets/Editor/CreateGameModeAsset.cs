using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakeGameModeAsset : MonoBehaviour
{
    [MenuItem("Assets/Create/GameMode")]
    public static void CreateGameModeAsset()
    {
        GameModes gamemode = ScriptableObject.CreateInstance<GameModes>();
        AssetDatabase.CreateAsset(gamemode, "Assets/Resources/GameMode.asset");
        AssetDatabase.SaveAssets();
    }

}
