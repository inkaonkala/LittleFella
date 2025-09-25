using UnityEngine;
using UnityEngine.InputSystem;

public class MomMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float jumpForce = 20f;
    public int jumpMax = 2;

    public Transform groundCheck;
    public LayerMask groundLayer; private Rigidbody2D body;

    private Vector2 movement;
    private MomInput input;

    private bool isGrounded;
    private int jumpsLeft;

    private Animator animatoor;
    private SpriteRenderer sr;


    void Start()
    {
        jumpsLeft = jumpMax;
        body = GetComponent<Rigidbody2D>();
        animatoor = GetComponent<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Awake()
    {
        input = new MomInput();
    }

    private void OnEnable()
    {
        input.Mom.Enable();
        input.Mom.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        input.Mom.Move.canceled += ctx => movement = Vector2.zero;
        input.Mom.Jump.performed += ctx => TryJump();
    }

    private void OnDisable()
    {
        input.Mom.Disable();
    }

    void Update()
   {
        animatoor.SetBool("isWalking", Mathf.Abs(movement.x) > 0.01f);

        if (movement.x != 0f)
            sr.flipX = movement.x > 0f;   // if this is reversed, change to `> 0f`
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);

        if (isGrounded && body.linearVelocity.y <= 0f)
            jumpsLeft = jumpMax;

        body.linearVelocity = new Vector2(movement.x * moveSpeed, body.linearVelocity.y);

    //    Debug.Log("Is Grounded: " + isGrounded);
        //body.MovePosition(body.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    
    private void TryJump()
    {
        if (isGrounded || jumpsLeft > 0)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);

            if (!isGrounded)
                jumpsLeft--;

            if (isGrounded)
                jumpsLeft = jumpMax - 1;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }
}
