using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    [Tooltip("Characters")]
    public Transform mom;
    public Transform fella;

    [Tooltip("Offset for players")]
    public Vector2 momOffset = new Vector2(-0.3f, 0f);
    public Vector2 FellaOffset = new Vector2(0.3f, 0f);

    void Start()
    {
        if (!SaveManager.TryGet(out var scene, out var pos, out var _))
            return;

        if (!string.IsNullOrEmpty(scene) && scene != SceneManager.GetActiveScene().name)
            return;

        if (mom)
            mom.position = pos + (Vector3)momOffset;
        if (fella)
            fella.position = pos + (Vector3)FellaOffset;
    }
    
}
