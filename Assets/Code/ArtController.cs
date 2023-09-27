using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class ArtController : MonoBehaviour
{
    [Header("Input")]
    public float xInput;
    public float yInput;
    public Vector2 input;

    [Header("Art")]
    private SpriteRenderer spriteRenderer;
    public Sprite walkSprite;
    public Sprite idleSprite;
    public Sprite jumpSprite;

    [Header("Player Controller")]
    public PlayerController playerController;

    // Update is called once per frame
    void Update(){
        GetInput();

        // Jump
        if (playerController.GetIsJumping()) {
            // [x] first i need the jump sprite
            // [ ] update the sprite renderer to the jump sprite
            spriteRenderer.sprite = jumpSprite;

        } else {
            UpdateWalk();
        }
    }

    private void GetInput() {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        input = new Vector2(xInput, yInput);
    }

    private void UpdateWalk() {
        // Left Walk 
        if ( input.x < -0.1f) {
            spriteRenderer.sprite = walkSprite;
            spriteRenderer.flipX = true;
            // Walk Right 
        } else if(input.x > 0.1f) {
            spriteRenderer.sprite = walkSprite;
            spriteRenderer.flipX = false;
            // IDle
        } else {
            spriteRenderer.sprite = idleSprite;

        }

    }


    // Start is called before the first frame update
    void Start() {
        // Art
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null) {
            Debug.LogWarning("No SpriteREnderer Found");
        }

        // get a reference to the player controller
        playerController = GetComponentInParent<PlayerController>();
        
        if (playerController == null) {
            Debug.LogWarning("No playerController Found");
        }
    }
}
