using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{   
    [Header("Input")]
    public Vector2 input;
    public bool jumpButtonIsDown = false;

    // Components
    private Rigidbody2D rigid;
   //private float cachedDrag = 


    [Header("Movement")]
    public float xMoveSpeed = 1f;

    [Header("Jumping")]
    public float jumpForce = 1f;
    private bool isJumping = false;

    public float groundDetectHeight = 1f;
    private bool isGrounded = false;

    public LayerMask groundLayerMask;

    public bool DEBUG_MODE = true;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public bool GetIsGrounded() {
        return isGrounded;
    }

    public bool GetIsJumping() {
        return isJumping;
    }

    // Update is called once per frame
    void Update()
    {
        // Get input. 
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        jumpButtonIsDown = Input.GetButton("Jump");


        // input but with no down direction
        Vector2 inputNoDown = new Vector2(input.x, Mathf.Abs(input.y));

        // Movement - move side to side or up, no down
        rigid.AddForce(inputNoDown);

        // Jump button, jump from 45* left to 45* right up. 
        if(jumpButtonIsDown && isGrounded) {
            isJumping = true;
            rigid.AddForce((Vector2.up + inputNoDown).normalized, ForceMode2D.Impulse);
        }

        // Raycast to see if we are near the ground. 
        RaycastHit2D hitResults = Physics2D.Raycast(transform.position, Vector2.down, groundDetectHeight, groundLayerMask.value);
        
        if(hitResults.collider != null) {
            Debug.DrawRay(transform.position, Vector2.down * groundDetectHeight, Color.green);
            isGrounded = true;


        } else {
            Debug.DrawRay(transform.position, Vector2.down * groundDetectHeight, Color.red);
            isGrounded = false;
        }

        if(rigid.velocity.y < -.1f && isGrounded) {
            isJumping = false;
        }



    }
}
