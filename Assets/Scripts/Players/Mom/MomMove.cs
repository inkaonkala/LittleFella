using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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

    public Animator animatoor;
    private SpriteRenderer sr;

    //Climb
    public float climbSpeed = 3.5f;
    private bool inClimbZone = false;
    private float ogGravity = 1f; // check what it actually is!!

    //HEAL
    public HealthBar scarabHealth;
    private bool canControl = true;

    //TheStraw
    public bool hasWeapon = false;
    [Header("HitBoxes")]
    public Collider2D hitboxLeft;
    public Collider2D hitboxRight;



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
        DisaableHitboxes();
    }

    private void OnEnable()
    {
        input.Mom.Enable();

        input.Mom.Move.performed += OnMove;
        input.Mom.Move.canceled  += OnMoveCanceled;
        input.Mom.Jump.performed += OnJump;      
        input.Mom.Smack.performed += OnSmack;

        if (scarabHealth != null)
        {
            scarabHealth.OnZeroHealth += HandleDown;
            scarabHealth.OnHealthUp += HandlegetUp;
        }
    }

    private void OnDisable()
    {
        input.Mom.Move.performed -= OnMove;
        input.Mom.Move.canceled  -= OnMoveCanceled;
        input.Mom.Jump.performed -= OnJump;
        input.Mom.Smack.performed -= OnSmack;

        input.Mom.Disable();

        if (scarabHealth != null)
        {
            scarabHealth.OnZeroHealth -= HandleDown;
            scarabHealth.OnHealthUp -= HandlegetUp;
        }
    }

    void Update()
    {
        if (!canControl)
        {
            animatoor.SetBool("isWalking", false);
            return;
        }
        
        animatoor.SetBool("isWalking", Mathf.Abs(movement.x) > 0.01f);
        animatoor.SetBool("isClimbing", inClimbZone && Mathf.Abs(movement.y) > 0.01f);

        if (movement.x != 0f)
            sr.flipX = movement.x > 0f;
            
    }

    void FixedUpdate()
    {
        if (!canControl)
        {
            body.linearVelocity = new Vector2(0f, body.linearVelocity.y);
            return;
        }

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


    private void HandleDown()
    {
        canControl = false;
        animatoor.SetTrigger("isDown");
        if (inClimbZone)
            eixtClimb();
        body.linearVelocity = new Vector2(0f,  body.linearVelocity.y);
    }

    private void HandlegetUp()
    {
        if (canControl)
            return;
        canControl = true;
        animatoor.SetTrigger("getUp");
    }

    //Smack

    private bool IsFacingRight()
    {
        return sr.flipX;
    }

    private void EnaableHitbox()
    {
        hitboxLeft.enabled = false;
        hitboxRight.enabled = false;

        Debug.Log("Box on");

        if (IsFacingRight())
            hitboxRight.enabled = true;
        else
            hitboxLeft.enabled = true;
    }

    private void DisaableHitboxes()
    {
        Debug.Log("BOX off");
        hitboxLeft.enabled = false;
        hitboxRight.enabled = false;
    }

    IEnumerator HitboxTimer(float delay, float hitTime)
    {
        yield return new WaitForSeconds(delay);
        EnaableHitbox();
        yield return new WaitForSeconds(hitTime);
        DisaableHitboxes();
    }

    private void OnSmack(InputAction.CallbackContext ctx)
    {
        if (!hasWeapon)
            return;
        
        animatoor.SetTrigger("Hit");

        StartCoroutine(HitboxTimer(0.2f, 0.4f));
    }

    public void RefreshSmackBind()
    {
        hasWeapon = true;
    }

    private void TryJump()
    {
        //exit climbZone if needed!!!!
        if (!canControl)
            return;
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
