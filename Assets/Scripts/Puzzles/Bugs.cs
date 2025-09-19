using UnityEngine;

/*
public class Bugs : Carryable
{
    public ColorType color;

    // Keep your old flags so the puzzle logic stays intact
    public bool isCarried;            // kept for compatibility; mirrored from base
    public bool isPlacedOnShroom;

    // Ground check (unchanged)
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.15f;

    protected override void Awake()
    {
        base.Awake();

        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    public override void PickUp(Transform holder)
    {
        if (isPlacedOnShroom) return;

        base.PickUp(holder);
        isCarried = true;
    }

    public override void Drop()
    {
        if (!isCarried) return;

        base.Drop();
        isCarried = false;

        // Tiny downward nudge like before so it settles
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -0.5f);
    }

    public void LockToShroom(Transform runPoint)
    {
        isPlacedOnShroom = true;
        isCarried = false;

        transform.SetParent(null);

        rb.simulated = false;
        if (col) col.enabled = false;

        transform.position = runPoint.position;
    }

    void FixedUpdate()
    {
        if (groundCheck == null) return;

        bool grounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // Keep your old static/dynamic swap, but NEVER while carried or placed
        if (!isCarried && !isPlacedOnShroom)
        {
            if (grounded)
                rb.bodyType = RigidbodyType2D.Static;
            else
                rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    // (Optional) visualize pickup/ground radius in editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}

*/


public class Bugs : MonoBehaviour
{
    public ColorType color;

    public bool isCarried;
    public bool isPlacedOnShroom;

    //Groound check
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.15f;

    Rigidbody2D _bu;
    Collider2D _col;

    void Awake()
    {
        _bu = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();

        // Safer physics defaults for tiny objects
        _bu.freezeRotation = true;
        _bu.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    public void PickUp(Transform holder)
    {
        if (isPlacedOnShroom)
            return;

        isCarried = true;

        // Disable physics & collisions while carried
        _bu.simulated = false;
        _col.enabled = false;

        transform.SetParent(holder);
        transform.localPosition = Vector3.zero;
    }

    public void Drop()
    {
        if (!isCarried)
            return;

        isCarried = false;
        transform.SetParent(null);

        _col.enabled = true;
        _bu.bodyType = RigidbodyType2D.Dynamic;
        _bu.simulated = true;

        _bu.linearVelocity = new Vector2(_bu.linearVelocity.x, -0.5f);
    }

    public void LockToShroom(Transform runPoint)
    {
        isPlacedOnShroom = true;
        isCarried = false;
        transform.SetParent(null);

        _bu.simulated = false;
        _col.enabled = false;
        transform.position = runPoint.position;
    }

    void FixedUpdate()
    {
        if (groundCheck == null) return;

        bool isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (isGrounded && !isCarried && !isPlacedOnShroom)
        {
            // Stop moving once on ground
            _bu.bodyType = RigidbodyType2D.Static;
        }
        else if (!isCarried && !isPlacedOnShroom)
        {
            // Let it fall if not grounded
            _bu.bodyType = RigidbodyType2D.Dynamic;
        }
    }


}
