using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{

    //1. [] Get my projectile reference
    public Rigidbody2D projectilePrefab;

    //2. [] Input - 
    public bool input;

    public float shootingForce = 10f;
    //3. [] posiion to spawn at
    //4. [] rotation to spawn at

    //5. [] get a reference to the projectiles rigidbody
    //6. [] add force


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        input = Input.GetButtonDown("Jump");

        if (input)
        {
            Rigidbody2D rb = Instantiate<Rigidbody2D>(projectilePrefab, transform.position, transform.rotation);

            rb.AddRelativeForce(Vector3.forward * shootingForce);  
        }
    }
}
