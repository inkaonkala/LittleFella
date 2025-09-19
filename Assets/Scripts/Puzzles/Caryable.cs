using UnityEngine;
//THIS CODE IS NOT USED RIHT NOW!!
public class Carryable : MonoBehaviour
{
    public Vector2 holdLocalOffset = Vector2.zero;

    protected Rigidbody2D rb;
    protected Collider2D col;

    // cached physics state
    protected bool baseSimulated;
    protected float baseGravity;
    protected RigidbodyType2D baseBodyType;

    public bool isCarried { get; private set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public virtual void PickUp(Transform holder)
    {
        if (isCarried) return;
        isCarried = true;

        // cache current state
        baseSimulated = rb.simulated;
        baseGravity = rb.gravityScale;
        baseBodyType = rb.bodyType;

        // stabilize while carried
        rb.linearVelocity = Vector2.zero;   // use rb.velocity if your version prefers that
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;  // replaces isKinematic
        rb.simulated = true;                 // keep the body active
        rb.gravityScale = 0f;
        if (col) col.enabled = false;

        // parent & align
        transform.SetParent(holder, worldPositionStays: false);
        transform.localPosition = (Vector3)holdLocalOffset;
        transform.localRotation = Quaternion.identity;
    }

    public virtual void Drop()
    {
        if (!isCarried) return;
        isCarried = false;

        transform.SetParent(null);

        // restore physics
        rb.bodyType = baseBodyType;
        rb.simulated = baseSimulated;
        rb.gravityScale = baseGravity;
        if (col) col.enabled = true;
    }
}
