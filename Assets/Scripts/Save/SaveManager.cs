using UnityEngine;
using UnityEngine.SceneManagement;


public static class SaveManager
{
    // Keys
    const string K_HAS       = "save_has";
    const string K_SCENE     = "save_scene";
    
    //Positioning 
    const string K_SPAWN_X = "save_spawn_x";
    const string K_SPAWN_Y   = "save_spawn_y";
    const string K_SPAWN_Z   = "save_spawn_z";
    
    const string K_POINT_ID = "save_point_id"; // for different flowas in one scene

    public static bool HasSave => PlayerPrefs.GetInt(K_HAS, 0) == 1;

    public static void SaveAt(string savePointId, Vector3 spawnPos)
    {
        PlayerPrefs.SetInt(K_HAS, 1);
        PlayerPrefs.SetString(K_SCENE, SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat(K_SPAWN_X, spawnPos.x);
        PlayerPrefs.SetFloat(K_SPAWN_Y, spawnPos.y);
        PlayerPrefs.SetFloat(K_SPAWN_Z, spawnPos.z);
        PlayerPrefs.SetString(K_POINT_ID, savePointId);
        PlayerPrefs.Save();
    }

    public static bool TryGet(out string scene, out Vector3 pos, out string pointId)
    {
        scene   = PlayerPrefs.GetString(K_SCENE, "");
        pos     = new Vector3(
                    PlayerPrefs.GetFloat(K_SPAWN_X, 0),
                    PlayerPrefs.GetFloat(K_SPAWN_Y, 0),
                    PlayerPrefs.GetFloat(K_SPAWN_Z, 0));
        pointId = PlayerPrefs.GetString(K_POINT_ID, "");
        return HasSave;
    }

    public static void Clear()
    {
        PlayerPrefs.DeleteKey(K_HAS);
        PlayerPrefs.DeleteKey(K_SCENE);
        PlayerPrefs.DeleteKey(K_SPAWN_X);
        PlayerPrefs.DeleteKey(K_SPAWN_Y);
        PlayerPrefs.DeleteKey(K_SPAWN_Z);
        PlayerPrefs.DeleteKey(K_POINT_ID);
        PlayerPrefs.Save();
    }
}
