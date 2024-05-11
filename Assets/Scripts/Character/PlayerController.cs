using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 10f;
    [SerializeField] float verticalSpeed = 2.5f;
    [SerializeField] float jumpForce = 24f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundRadious = 2f;
    public Animator animator;
    public bool CanWeGoVertical { get; set; }
    public bool CanWeTakeAnObject { get; set; }
    public bool IsWeHoldAnObject { get; set; }
    public Rigidbody2D rb { get; private set; }
    public UnityEvent onTakeKeyPressed;
    public UnityEvent onThrowKeyPressed;
    public UnityEvent onPushKeyPressed;
    public bool facingRight = true;
    public bool isPushing { get; private set; }
    public bool isGrounded;
    private float startSpeed;
    private float startJumpForce;

    private bool isRestarting = false;
    private float heldTime = 0f;
    private const float restartTimeThreshold = 1f;

    private float lastJumpTime;

    private float jumpTime = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    { 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadious, groundLayer);

        if (!isGrounded)
        {
            jumpTime += Time.deltaTime;

            if (jumpTime >= 1f)
            {
                rb.velocity = new Vector2(rb.velocity.x, -9);
            }
        }

        if (isGrounded && jumpTime != 0)
        {
            jumpTime = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (Time.time - lastJumpTime < 0.5f)
            {
                animator.SetTrigger("LedgeClimbing");
            }
            else
            {
                animator.SetTrigger("Jump");
            }
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            lastJumpTime = Time.time;
        }

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
    }

    private void FixedUpdate()
    {
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
            rb.gravityScale = 5;
        }

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
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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
}

