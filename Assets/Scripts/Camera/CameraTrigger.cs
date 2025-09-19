using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraTrigger : MonoBehaviour
{
    [Header("New Camera Settings in this Zone")]
    public Vector2 zoneDeadZone = new Vector2(4f, 2f);
    public float zoneOrthoSize = 6.5f;
    public float zoneSmoothTime = 0.25f;

    [Header("Blending")]
    public float enterBlend = 0.35f;
    public float exitBlend  = 0.35f;

    [Header("Trigger")]
    public string triggeringTag = "FellaTag";

    [Header("Override target")]
    public Transform overrideTarget;
    public Vector2 overrideOffset = Vector2.zero; // e.g., (0, +3) to aim higher
    public bool revertOnExit = true;

    private void Reset()
    {
        var c = GetComponent<BoxCollider2D>();
        c.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(triggeringTag))
            return;

        var camFollow = Camera.main ? Camera.main.GetComponent<CameraFollow>() : null;
        if (!camFollow)
            return;

        camFollow.SetProfile(zoneDeadZone, zoneOrthoSize, zoneSmoothTime, enterBlend);

        if (overrideTarget)
            camFollow.SetTarget(overrideTarget);

        camFollow.SetOffset(overrideOffset);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(triggeringTag))
            return;

        var camFollow = Camera.main ? Camera.main.GetComponent<CameraFollow>() : null;
        if (!camFollow) return;

        if (revertOnExit)
        {
            camFollow.ResetProfile(exitBlend);
            camFollow.ResetTarget();   // back to the default (Fella you assigned on the camera)
            camFollow.ResetOffset();   // back to default offset
        }
    }
}
