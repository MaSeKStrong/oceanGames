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
        [SerializeField] float jumpForce;
    [SerializeField] int jumpIteration;
    [SerializeField] int jumpValueIteration;
    
        [SerializeField] float groundCheckRadius;
    [SerializeField] float newWallCheckRadius;
    private float movementInputDirection;
    public float ledgeRayCorrectY = 0.5f;
    public float offsetY;
    public float minCorrectDistance = 0.01f;
        [SerializeField] private float wallChekDistance;        
        [SerializeField] private float ledgeClimbXoffset1;
        [SerializeField] private float ledgeClimbYoffset1;
        [SerializeField] private float ledgeClimbXoffset2;
        [SerializeField] private float ledgeClimbYoffset2;
        private float startSpeed; // система от первого программиста, работало/работает с текущей системой старта/рестарта, при переделке можно будет удалить
        private float startJumpForce; //эта переменная аналогична, я не трогал потому что пока что она необходима в текущих условиях
       
    public float jumpWallTime;
    private float timerJumpWall;
        private int facingDirection = 1;// нужно для скольжения по стенам, удалить при удалении скольжения

        [SerializeField] Transform groundChek;
    [SerializeField] Transform newWallChek;
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
        public bool canPushing;
        private bool isRestarting = false;
        private bool isWalking;
        public bool canJump;
        private bool isTouchingLedge;
        private bool isTouchingWall;
    private bool isTouchingWallDown;
    public bool isWall;
    private bool canClimbLedge = false;
        private bool ledgeDetected;      
        private bool isObjPull;
    private bool blockMoveX;
        [HideInInspector] public bool isPulling;
    
        private float heldTime = 0f; // нужно для рестарта, если сделать нормальную функцию рестарта можно будет удалить
        private const float restartTimeThreshold = 1f;
        private float lastJumpTime;
        private float jumpTime = 0f;

        GameObject pullingObj;
    
        private Vector2 ledgePosBot;
        private Vector2 ledgePos1;
        private Vector2 Ledgepos2;
            
    public Vector2 jumpAngle = new Vector2(3.5f, 10);

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
            CheckLedgeClimb();
             Jump();
        JumpTree();

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

            if (canPushing /*Input.GetKey(KeyCode.LeftShift)*/) 
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
        ChekingLedge();
        }
    
        public void FinishLedgeClimb()
        {
            canClimbLedge = false;
            transform.position = Ledgepos2;
            ledgeDetected = false;
        blockMoveX = false;
            animator.SetBool("canClimbe", canClimbLedge);


        }
        private void CheckSurroundings()
        {
            isGrounded = Physics2D.OverlapCircle(groundChek.position, groundCheckRadius, whatIsGround);
        isTouchingWallDown = Physics2D.OverlapCircle(newWallChek.position, newWallCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallChekDistance, whatIsGround);
        isWall = (isTouchingWall && isTouchingWallDown);
                   
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, transform.right, wallChekDistance, whatIsObj);
        /*
            if (isTouchingWall && !isTouchingLedge && !ledgeDetected)
            {
                ledgeDetected = true;
                ledgePosBot = wallCheck.position;
                animator.SetTrigger("preClimb");

            }             
        */
            if (isGrounded)
            {
                ledgeDetected = false;
            animator.SetBool("proClimb", false);
        }
        
            if (hit.collider != null)
            {
                pullingObj = hit.collider.gameObject;
                isObjPull = true;
            }
        }
    void ChekingLedge()
    {
        if (isTouchingWall)
        {
            isTouchingLedge = !Physics2D.Raycast(
                new Vector2(wallCheck.position.x, wallCheck.position.y + ledgeRayCorrectY),
                new Vector2(transform.localScale.x, 0),
                wallChekDistance,
                whatIsGround);
        }
        else
        {
            isTouchingLedge = false;
        }       

        if (isTouchingLedge && !isGrounded)
        {
            //animator.SetTrigger("preClimb");
            animator.SetBool("proClimb", true);
            ledgeDetected = true;
            canClimbLedge = false;
            blockMoveX = true;
            ledgePosBot = wallCheck.position;
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0, 0);
            offsetCalculateandCorrect();
        }
    }

    void offsetCalculateandCorrect()
    {
        offsetY = Physics2D.Raycast(
                new Vector2(wallCheck.position.x + wallChekDistance * transform.localScale.x, wallCheck.position.y + ledgeRayCorrectY),
                Vector2.down,
                ledgeRayCorrectY,
                whatIsGround).distance;
        if (offsetY > minCorrectDistance * 1.5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - offsetY + minCorrectDistance, transform.position.z);
        }
    }

        void CheckLedgeClimb()
        {
           
        if (Input.GetButton("Jump") && ledgeDetected && !canClimbLedge)
            {
            animator.SetBool("proClimb", false);
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
            
            }
            
            animator.SetBool("canClimbe", canClimbLedge);
         
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
        if (!blockMoveX)
        {
            movementInputDirection = Input.GetAxisRaw("Horizontal");
        }
            if (Input.GetKey(KeyCode.LeftShift) && isObjPull)
            {
                pullingObj.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
                pullingObj.GetComponent<FixedJoint2D>().enabled = true;
                isPulling = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
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
            animator.SetBool("isPulling", isPulling);
            animator.SetBool("IsPushing", isPushing);

        }
       
        void Jump()
        {
       
        if (Input.GetButton("Jump"))
        {
            if (isGrounded)
            {
                canJump = true;
            }            
        }else { canJump = false; }
        
        if (canJump)
        {
            if(jumpIteration++ < jumpValueIteration)
            {
                rb.AddForce(Vector2.up * jumpForce / jumpIteration);
                
            }
        }
        else
        {
            jumpIteration = 0;   
        }      
    }
        void JumpTree()
        {
        if(CanWeGoVertical && !isGrounded && Input.GetButtonDown("Jump"))
        {
            blockMoveX = true;

            animator.SetTrigger("Jump");

            transform.localScale *= new Vector2(-1, 1);
            facingRight = !facingRight;

            rb.gravityScale = 10;
            rb.velocity = new Vector2(0, 0);
            rb.velocity = new Vector2(transform.localScale.x * jumpAngle.x, jumpAngle.y);
        }
        if(blockMoveX && (timerJumpWall += Time.deltaTime) >= jumpWallTime)
        {
            if (isGrounded || CanWeGoVertical || Input.GetAxisRaw("Horizontal") != 0) 
            { blockMoveX = false;
                CanWeGoVertical = false;
                timerJumpWall = 0; 
            }
        }        
        }

        void ApplyMovement()
        {
        if (!blockMoveX)
        {
            rb.velocity = new Vector2(MoveSpeed * movementInputDirection, rb.velocity.y);
        }
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

        }

        private void Flip()
        {
                facingDirection *= -1;
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
        Gizmos.DrawWireSphere(newWallChek.position, newWallCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallChekDistance, wallCheck.position.y, wallCheck.position.z));
            Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + wallChekDistance, ledgeCheck.position.y, ledgeCheck.position.z));
        }
   }

