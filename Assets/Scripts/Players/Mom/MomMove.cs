using UnityEngine;
using UnityEngine.InputSystem;

public class MomMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 20f;
    public int jumpMax = 2;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D body;
    private Vector2 movement;
    private MomInput input;

    private bool isGrounded;
    private int jumpsLeft;

    private Animator animatoor;
    private SpriteRenderer sr;

    //Climb
    public float climbSpeed = 3.5f;
    private bool inClimbZone = false;
    private float ogGravity = 1f; // check what it actually is!!

    private void Awake()
    {
        input = new MomInput();
    }

    void Start()
    {
        jumpsLeft = jumpMax;
        body = GetComponent<Rigidbody2D>();
        animatoor = GetComponent<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        ogGravity = body.gravityScale;
    }

    private void OnEnable()
    {
        input.Mom.Enable();

        /*
                input.Mom.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
                input.Mom.Move.canceled += ctx => movement = Vector2.zero;
                input.Mom.Jump.performed += ctx => TryJump();
                input.Mom.Smack.performed += OnSmack;
                */
        input.Mom.Move.performed += OnMove;
        input.Mom.Move.canceled  += OnMoveCanceled;
        input.Mom.Jump.performed += OnJump;
        input.Mom.Smack.performed += OnSmack;
        input.Mom.Move.canceled  += OnMoveCanceled;
        input.Mom.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        input.Mom.Move.performed -= OnMove;
        input.Mom.Move.canceled  -= OnMoveCanceled;
        input.Mom.Jump.performed -= OnJump;
        input.Mom.Smack.performed -= OnSmack;

        input.Mom.Disable();
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
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);

            if (isGrounded && body.linearVelocity.y <= 0f)
                jumpsLeft = jumpMax;

            body.linearVelocity = new Vector2(movement.x * moveSpeed, body.linearVelocity.y);
        }
        else
        {
            body.gravityScale = 0f;
            float vy = movement.y * climbSpeed;
            body.linearVelocity = new Vector2(0f, vy);
        }

    }


    // ===== Handlers =====

    private void OnMove(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        movement = Vector2.zero;
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        TryJump();
    }

    private void OnSmack(InputAction.CallbackContext ctx)
    {
        animatoor.SetTrigger("Hit");
    }



    private void TryJump()
    {
        //exit climbZone if needed!!!!
        if (inClimbZone)
        {
            eixtClimb();
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            return;
        }

        if (isGrounded || jumpsLeft > 0)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);

            if (!isGrounded)
                jumpsLeft--;

            if (isGrounded)
                jumpsLeft = jumpMax - 1;
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
            eixtClimb();
        }
    }

    private void eixtClimb()
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
