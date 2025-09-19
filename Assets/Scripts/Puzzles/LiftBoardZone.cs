
using UnityEngine;

public class LiftBoardZone : MonoBehaviour
{
    [Header("Who counts as onboard")]
    public Transform mom;
    public Transform fella;

    int _momInside = 0;
    int _fellaInside = 0;

    public bool HasMom   => _momInside   > 0;
    public bool HasFella => _fellaInside > 0;
    public bool BothOnboard => HasMom && HasFella;

    [Header("Tight to the lift")]
    public Transform platformRoot; // set to the Elevator root to parent riders
    public bool parentRiders = true;

    void Reset()
    {
        var box = GetComponent<BoxCollider2D>();
        box.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var tr = other.attachedRigidbody ? other.attachedRigidbody.transform : other.transform;

        if (tr == mom) _momInside++;
        if (tr == fella) _fellaInside++;

        if (parentRiders && platformRoot)
        {
            if (tr == mom || tr == fella)
                tr.SetParent(platformRoot, worldPositionStays: true);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Keep enforcing parenting while inside
        if (!parentRiders || !platformRoot) return;
        var tr = other.attachedRigidbody ? other.attachedRigidbody.transform : other.transform;
        if (tr == mom || tr == fella)
            tr.SetParent(platformRoot, true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var tr = other.attachedRigidbody ? other.attachedRigidbody.transform : other.transform;

        if (tr == mom) _momInside = Mathf.Max(0, _momInside - 1);
        if (tr == fella) _fellaInside = Mathf.Max(0, _fellaInside - 1);

        if (parentRiders && platformRoot)
        {
            if (tr == mom || tr == fella)
                tr.SetParent(null, worldPositionStays: true);
        }
    }
}

