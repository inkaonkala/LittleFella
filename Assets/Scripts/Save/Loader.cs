using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    [Header("Characters")]
    public Transform mom;
    public Transform fella;

    [Header("Offsets")]
    public Vector2 momOffset = new Vector2(-0.3f, 0f);
    public Vector2 fellaOffset = new Vector2(0.3f, 0f);

    void Start()
    {
        if (!SaveManager.TryGet(out var scene, out var pos, out var _))
            return;

        if (!string.IsNullOrEmpty(scene) && scene != SceneManager.GetActiveScene().name)
            return;

        StartCoroutine(ApplyNextFrame(pos));
    }

    private IEnumerator ApplyNextFrame(Vector3 pos)
    {
        yield return null; // let tilemap/spawners/physics initialize

        Teleport(mom, pos + (Vector3)momOffset);
        Teleport(fella, pos + (Vector3)fellaOffset);
    }

    private void Teleport(Transform t, Vector3 pos)
    {
        if (!t) return;

        var rb = t.GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.position = pos;
        }
        else
        {
            t.position = pos;
        }
    }
}
