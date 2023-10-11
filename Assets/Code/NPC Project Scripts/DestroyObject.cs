using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Will destroy itself after 8 seconds
        Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            //Destroy if hits player
            Destroy(gameObject);
        }
        else
        {
            //Destroy after 2 seconds if it collides with the stage
            Destroy(gameObject, 2f);
        }
    }
}
