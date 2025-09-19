using UnityEngine;

public class MomCarry : MonoBehaviour
{
    [Header("Setup")]
    public Transform carryPoint;          // assign Mom/CarryPoint
    public LayerMask fellaLayer;          // set to Fella's layer
    public float pickupRadius = 0.7f;     // tweak to taste

    [Header("Input")]
    public bool useDedicatedCarryAction = false;

    Rigidbody2D _momRb;
    MomInput _input;
    Vector2 _move;
    bool _carrying;
    FellaCarryable _carried;

    // debounce for using Down-as-carry
    bool _downWasHeld;

    void Awake()
    {
        _momRb = GetComponent<Rigidbody2D>();
        _input  = new MomInput();
    }

    void OnEnable()
    {
        _input.Mom.Enable();

        // You already have these in your move script:
        _input.Mom.Move.performed += ctx => _move = ctx.ReadValue<Vector2>();
        _input.Mom.Move.canceled  += ctx => _move = Vector2.zero;

        if (useDedicatedCarryAction)
        {
            // Create an action "Carry" in your Mom action map (Keyboard S or E, Gamepad South, etc.)
            _input.Mom.PickNDrop.performed += _ => ToggleCarry();
        }
    }

    void OnDisable()
    {
        if (_input != null) _input.Mom.Disable();
    }

    void Update()
    {
        if (!useDedicatedCarryAction)
        {
            // Use Down on the Move stick as a "press"
            bool downNow = _move.y < -0.5f;
            if (downNow && !_downWasHeld)
            {
                ToggleCarry();
            }
            _downWasHeld = downNow;
        }
    }

    void ToggleCarry()
    {
        if (_carrying) { Drop(); }
        else           { TryPickup(); }
    }

    void TryPickup()
    {
        if (_carrying) return;

        // Look for Fella in a small radius around Mom
        var hit = Physics2D.OverlapCircle((Vector2)transform.position, pickupRadius, fellaLayer);
        if (!hit) return;

        var fella = hit.GetComponent<FellaCarryable>();
        if (!fella || fella.IsCarried) return;

        _carried = fella;
        _carried.BeginCarry(carryPoint);
        _carrying = true;
    }

    void Drop()
    {
        if (!_carrying || !_carried) return;

        // pass Mom's current velocity so Fella pops off naturally
        Vector2 inherit = _momRb ? _momRb.linearVelocity : Vector2.zero;

        _carried.EndCarry(inherit);
        _carried  = null;
        _carrying = false;
    }

    // Optional: visualize pickup radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}

