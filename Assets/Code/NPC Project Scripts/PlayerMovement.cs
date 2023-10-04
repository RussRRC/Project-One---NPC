using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 14f;
    private bool isFacingRight = true;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.05f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.9f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    private Animator _anim;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is NULL!!");
        }
    }
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }

        if (!IsGrounded() && rb.velocity.y > 0)
        {//Jumping
            _anim.SetBool("WallSliding", false);
            _anim.SetBool("Falling", false);
            _anim.SetBool("Jumping", true);
            _anim.SetBool("Idling", false);
            _anim.SetBool("Running", false);
        }
        else if (rb.velocity.y == 0 && Input.GetAxisRaw("Horizontal") == 0)
        {//Idling
            _anim.SetBool("WallSliding", false);
            _anim.SetBool("Falling", false);
            _anim.SetBool("Jumping", false);
            _anim.SetBool("Idling", true);
            _anim.SetBool("Running", false);
        }
        else if (rb.velocity.y == 0 && Input.GetAxisRaw("Horizontal") != 0)
        {//Running
            _anim.SetBool("WallSliding", false);
            _anim.SetBool("Falling", false);
            _anim.SetBool("Jumping", false);
            _anim.SetBool("Idling", false);
            _anim.SetBool("Running", true);

        }
        else if (IsWalled())
        {//WallSliding
            _anim.SetBool("WallSliding", true);
            _anim.SetBool("Falling", false);
            _anim.SetBool("Jumping", false);
            _anim.SetBool("Idling", false);
            _anim.SetBool("Running", false);

        }
        else if (rb.velocity.y < 0)
        {//Falling
            _anim.SetBool("WallSliding", false);
            _anim.SetBool("Falling", true);
            _anim.SetBool("Jumping", false);
            _anim.SetBool("Idling", false);
            _anim.SetBool("Running", false);
        }

    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }else if (IsGrounded())
        {
            isWallJumping = false;
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    public float GetSpeed()
    {
        return speed;
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}