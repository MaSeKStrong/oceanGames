using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb { get; private set; }

    public float MoveSpeed = 10f;
    public float baseSpeed;
    public float pusingspeed;
    [SerializeField] float verticalSpeed = 2.5f;
    [SerializeField] float jumpForce = 24f;    
    [SerializeField] float groundCheckRadius;
    private float movementInputDirection;
    [SerializeField] private float wallChekDistance;
    [SerializeField] private float wallslidingspeed;
    [SerializeField] private float ledgeClimbXoffset1;
    [SerializeField] private float ledgeClimbYoffset1;
    [SerializeField] private float ledgeClimbXoffset2;
    [SerializeField] private float ledgeClimbYoffset2;
    private float startSpeed;
    private float startJumpForce;

    [SerializeField] Transform groundChek;
    public Transform wallCheck;
    public Transform ledgeCheck;

    public LayerMask whatIsGround;
    public LayerMask whatIsObj;
    public bool CanWeGoVertical { get; set; }
    public bool CanWeTakeAnObject { get; set; }
    public bool IsWeHoldAnObject { get; set; }
    
    public bool facingRight = true;
    public bool isPushing { get; private set; }
    public bool isGrounded;
    private bool isRestarting = false;
    private bool isWalking;
    private bool canJump;
    private bool isTouchingLedge;
    private bool isTouchingWall;
    private bool canClimbLedge = false;
    private bool ledgeDetected;
    private bool isWallSliding;
    private bool isObjPull;
    [HideInInspector] public bool isPulling;
    private float heldTime = 0f;
    private const float restartTimeThreshold = 1f;
    private float lastJumpTime;
    private float jumpTime = 0f;

    GameObject pullingObj;
    
    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 Ledgepos2;

    public UnityEvent onTakeKeyPressed;
    public UnityEvent onThrowKeyPressed;
    public UnityEvent onPushKeyPressed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckifCanJump();
        CheckLedgeClimb();
        CheckIfWallSliding();
                
        if (Input.GetKeyDown(KeyCode.Q) && CanWeTakeAnObject && !IsWeHoldAnObject) 
        {
            Debug.Log("onTakeKeyPressed");
            onTakeKeyPressed.Invoke();
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        if (Input.GetKeyDown(KeyCode.Q) && !CanWeTakeAnObject && IsWeHoldAnObject)
        {
            Debug.Log("onThrowKeyPressed");
            onThrowKeyPressed.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log($"CanWeTakeAnObject + {CanWeTakeAnObject}");
            Debug.Log($"IsWeHoldAnObject + {IsWeHoldAnObject}");
        }

        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            isPushing = true;
        }
        else 
        {
            isPushing = false;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            isRestarting = true;
        }
        else if (Input.GetKeyUp(KeyCode.Return))
        {
            isRestarting = false;
            heldTime = 0f;
        }

        if (isRestarting)
        {
            heldTime += Time.deltaTime;

            if (heldTime >= restartTimeThreshold)
            {
                Restart.LoadThisScene();
            }
        }

        if (isGrounded && rb.velocity.y != 0)
        {
            Debug.Log("deadth");
        }
        
    }
   
    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
         if (CanWeGoVertical)
        {
            rb.gravityScale = 0;           
            float verticalInput = Input.GetAxis("Vertical");
            Vector2 moveDirectionVertical = new Vector2(0, verticalInput);
            float moveVertical = moveDirectionVertical.y * verticalSpeed;
            rb.velocity = new Vector2(rb.velocity.x, moveDirectionVertical.y * verticalSpeed);
        }
        else
        {
            rb.gravityScale = 10;
        }         
         /*
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(horizontalInput, 0);
        float move = moveDirection.x * MoveSpeed;
        rb.velocity = new Vector2(moveDirection.x * MoveSpeed, rb.velocity.y);
        if (rb.velocity.x > 0.1f || rb.velocity.x < -0.1f)
        {
            animator.SetBool("IsWalking", true);
        }
        else 
        {
            animator.SetBool("IsWalking", false);
        }
        if (rb.velocity.x > 5f || rb.velocity.x < -5f)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }


        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }*/
    }
    void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = Ledgepos2;
        ledgeDetected = false;
        animator.SetBool("canClimbe", canClimbLedge);

    }
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundChek.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallChekDistance, whatIsGround);
        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallChekDistance, whatIsGround);
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, transform.right, wallChekDistance, whatIsObj);

        if (isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }

        if (hit.collider != null)
        {
            pullingObj = hit.collider.gameObject;
            isObjPull = true;
        }
    }

    void CheckLedgeClimb()
    {
        if (ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            if (facingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallChekDistance) - ledgeClimbXoffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYoffset1);
                Ledgepos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallChekDistance) + ledgeClimbXoffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYoffset2);
            }
            else
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallChekDistance) + ledgeClimbXoffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYoffset1);
                Ledgepos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallChekDistance) - ledgeClimbXoffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYoffset1);
            }


            //anim.SetBool("canClimbe", canClimbLedge);
        }

        animator.SetBool("canClimbe", canClimbLedge);
        /*
        if (canClimbLedge)
        {
            transform.position = ledgePos1;
        }*/
    }


    private void CheckMovementDirection()
    {
        if (facingRight && movementInputDirection < 0 && !isPulling)
        {
            Flip();
        }
        else if (!facingRight && movementInputDirection > 0 && !isPulling)
        {
            Flip();
        }

        if (rb.velocity.x != 0 && isGrounded)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.LeftControl) && isObjPull)
        {
            pullingObj.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
            pullingObj.GetComponent<FixedJoint2D>().enabled = true;
            isPulling = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            pullingObj.GetComponent<FixedJoint2D>().enabled = false;
            isObjPull = false;
            isPulling = false;
        }
    }

    void UpdateAnimations()
    {
        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("IsPushing", isPulling);
        animator.SetBool("IsPushing", isPushing);

    }

    void CheckifCanJump()
    {

        if (isGrounded && rb.velocity.y <= 0)
        {
            canJump = true;            
        }
        else
        {
            canJump = false;            
        }
    }
    void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
           
        }
    }

    void ApplyMovement()
    {
        rb.velocity = new Vector2(MoveSpeed * movementInputDirection, rb.velocity.y);

        if (isWallSliding)
        {
            if (rb.velocity.y < -wallslidingspeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallslidingspeed);
            }
        }
    }

    private void Flip()
    {/*
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;*/
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void StopCharacter()
    {
        startSpeed = MoveSpeed;
        startJumpForce = jumpForce;
        MoveSpeed = 0;
        jumpForce = 0;
    }
    
    public void CharacterCanGo()
    {
       MoveSpeed = startSpeed;
      jumpForce = startJumpForce;
    } 

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundChek.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallChekDistance, wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + wallChekDistance, ledgeCheck.position.y, ledgeCheck.position.z));
    }
}

