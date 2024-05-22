using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyPlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    GameObject pullingObj;
    
    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool canJump;
    private bool isTouchingLedge;
    private bool isTouchingWall;
    private bool canClimbLedge = false;
    private bool ledgeDetected;    
    private bool isWallSliding;
    private bool isObjPull;
   [HideInInspector] public bool isPulling;
    

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 Ledgepos2;
    
    private float movementInputDirection;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallChekDistance;
    [SerializeField] private float wallslidingspeed;
    [SerializeField] private float ledgeClimbXoffset1;
    [SerializeField] private float ledgeClimbYoffset1;
    [SerializeField] private float ledgeClimbXoffset2;
    [SerializeField] private float ledgeClimbYoffset2;

    public Transform groundChek;
    public Transform wallCheck;
    public Transform ledgeCheck;

    public LayerMask whatIsGround;
    public LayerMask whatIsObj;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckifCanJump();
        CheckLedgeClimb();
        CheckIfWallSliding();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();        
    }

    void CheckIfWallSliding()
    {
        if(isTouchingWall && !isGrounded && rb.velocity.y < 0)
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
        anim.SetBool("canClimbe", canClimbLedge);
       
    }
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundChek.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallChekDistance, whatIsGround);
        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallChekDistance, whatIsGround);
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, transform.right,wallChekDistance, whatIsObj);

        if(isTouchingWall && !isTouchingLedge && !ledgeDetected)
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
        if(ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            if (isFacingRight)
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

        anim.SetBool("canClimbe", canClimbLedge);
        /*
        if (canClimbLedge)
        {
            transform.position = ledgePos1;
        }*/
    }
        

    private void CheckMovementDirection()
    {
        if(isFacingRight && movementInputDirection < 0 && !isPulling)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0 && !isPulling)
        {
            Flip();
        }

        if(rb.velocity.x != 0 && isGrounded)
        {
            isWalking = true;
        }
        else {
            isWalking = false; 
        }
    }

    void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        
        if (Input.GetKey(KeyCode.LeftControl) && isObjPull){
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
        anim.SetBool("IsWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("IsPushing", isPulling);       
        
    }
    
    void CheckifCanJump() { 
    
        if(isGrounded && rb.velocity.y <= 0)
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
        rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);

        if (isWallSliding)
        {
            if(rb.velocity.y < -wallslidingspeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallslidingspeed);
            }
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundChek.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallChekDistance, wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + wallChekDistance, ledgeCheck.position.y, ledgeCheck.position.z));
    }
}
