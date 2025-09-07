using UnityEngine;
using UnityEngine.InputSystem;

public class FellaMove : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpForceFella = 4f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    //burbs
    public int burbs = 3;
    public float burbRange = 3f;
    public HealthBar health; // health only on momma, this is a ref

    private Rigidbody2D body;
    private Vector2 movement;
    private FellaInput input;
    private bool isGrounded;

    private void Awake()
    {
        input = new FellaInput();
        
    }

    private void OnEnable()
    {
        input.Fella.Enable();
        input.Fella.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        input.Fella.Move.canceled += ctx => movement = Vector2.zero;
        input.Fella.Jump.performed += ctx => TryJump();
        input.Fella.Burb.performed += ctx => TryBurb();
    }

    private void OnDisable()
    {
        input.Fella.Disable();
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        body.linearVelocity = new Vector2(movement.x * moveSpeed, body.linearVelocity.y);

    //    Debug.Log("Is Grounded: " + isGrounded);

    }

    private void TryJump()
    {
        if (isGrounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForceFella);
        }
    }

    private void TryBurb()
    {
        Debug.Log("BURB!");
        if (burbs <= 0)
        {
            Debug.Log("No Burbs left!");
            return;
        }
        if (health == null)
        {
            Debug.Log("No health refrence :(");
            return;
        }

        float distanceToMom = Vector2.Distance(transform.position, health.transform.position);
        if (distanceToMom <= burbRange)
        {
            health.Heal(30f);
            burbs--;
            Debug.Log("Mommy healed. Burbs: " + burbs);
        }
        else
        {
            burbs--;
            Debug.Log("Mommy too far! Burbs: " + burbs);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }
}
