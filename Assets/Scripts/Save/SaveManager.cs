using UnityEngine;
using UnityEngine.SceneManagement;


public static class SaveManager
{
    // Keys
    const string K_HAS = "save_has";
    const string K_SCENE = "save_scene";

    //Positioning 
    const string K_SPAWN_X = "save_spawn_x";
    const string K_SPAWN_Y = "save_spawn_y";
    const string K_SPAWN_Z = "save_spawn_z";

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
	    if (!HasSave)
	    {
	        scene = "";
	        pos = default;
	        pointId = "";
	        return false;
	    }

	    scene = PlayerPrefs.GetString(K_SCENE, "");
	    pointId = PlayerPrefs.GetString(K_POINT_ID, "");

	    pos = new Vector3(
	        PlayerPrefs.GetFloat(K_SPAWN_X, float.NaN),
	        PlayerPrefs.GetFloat(K_SPAWN_Y, float.NaN),
	        PlayerPrefs.GetFloat(K_SPAWN_Z, 0f)
		    );

	    // If x or y are NaN, something wasn't saved correctly
	    if (float.IsNaN(pos.x) || float.IsNaN(pos.y))
	        return false;

    return true;
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

    public static bool FlowerFirstTime(string flowerId)
        => PlayerPrefs.GetInt($"flower_first_{flowerId}", 0) == 0;

    public static void MarkFlowaUsed(string flowerId)
    {
        PlayerPrefs.SetInt($"flower_first_{flowerId}", 1);
        PlayerPrefs.Save();
    }
}
