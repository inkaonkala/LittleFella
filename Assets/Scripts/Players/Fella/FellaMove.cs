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

    private Animator animatoor;
    private SpriteRenderer sr;

    public float climbSpeed = 3.5f;
    private bool inClimbZone = false;
    private float ogGravity = 1f;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animatoor = GetComponent<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        ogGravity = body.gravityScale;
    }

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
        input.Fella.Move.performed -= ctx => movement = ctx.ReadValue<Vector2>(); // clean up to be safe
        input.Fella.Move.canceled  -= ctx => movement = Vector2.zero;
        input.Fella.Jump.performed -= ctx => TryJump();
        input.Fella.Burb.performed -= ctx => TryBurb();
        input.Fella.Disable();
    }

    void Update()
   {
        animatoor.SetBool("isWalking", Mathf.Abs(movement.x) > 0.01f);
        animatoor.SetBool("isClimbing", inClimbZone && Mathf.Abs(movement.y) > 0.01f);


        if (movement.x != 0f)
            sr.flipX = movement.x > 0f;
    }

    void FixedUpdate()
    {
        if (!inClimbZone)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

            body.linearVelocity = new Vector2(movement.x * moveSpeed, body.linearVelocity.y);
        }
        else
        {
            body.gravityScale = 0f;
            float vy = movement.y * climbSpeed;
            body.linearVelocity = new Vector2(0f, vy);
        }
    }
    

    private void TryJump()
    {
        if (inClimbZone)
        {
            ExitClimb();
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForceFella);
            return;
        }
        
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

    // FIND Climb zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Klimbable"))
        {
            inClimbZone = true;
            body.gravityScale = 0f;
            //prevent sudden fall
            body.linearVelocity = new Vector2(0f, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Klimbable"))
        {
            ExitClimb();
        }
    }

    private void ExitClimb()
    {
        inClimbZone = false;
        body.gravityScale = ogGravity;
        animatoor.SetBool("isClimbing", false);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }
}
