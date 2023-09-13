using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisable : MonoBehaviour
{
    public Player player;
    public bool isTimerRunning = false;
    public float currentTimer = 0;
    public float maxTimer = 1f;

    public SpriteRenderer spriteRender;
    public Sprite startingSprite;
    public Sprite collision;
    // Start is called before the first frame update

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("I Collided with " + collision.gameObject.name);

        //player = collision.gameObject.GetComponent<PhysicsPlayer>();
        if (player != null)
        {
            player.enabled = false;
            isTimerRunning = true;
            currentTimer = 0;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
