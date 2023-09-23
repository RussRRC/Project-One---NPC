using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 input;
    public float force = 25f;
    public Vector2 moveDirectionWithSpeed;

    private Rigidbody2D rigid;


    //public Transform selfTransform;

    //public float moveSpeed = 10f;
    //public float rotationSpeed = 165f;

    //public Vector2 initialPosition = (-1.5, 3);


    //public SpriteRenderer villain;

    //public float wanderRadius;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //cache input
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //Old movement, constant speed and full body movement

        /*
        //normalizing input
        float mag = Mathf.Min(1f, input.magnitude);
        Vector2 normInput = input.normalized; //direction

        Debug.Log("Input: " + input + " mag: " + mag + " norm Input: " + normInput);

        Vector2 cleanedInput = normInput * mag;

        moveDirectionWithSpeed = cleanedInput * speed;

        //

        //transform.position = transform.position * (Vector3)moveDirectionWithSpeed
        transform.Translate(moveDirectionWithSpeed * new Vector2(0,1) * Time.deltaTime);

        Debug.DrawRay(transform.position, moveDirectionWithSpeed * Time.deltaTime *25f);
        */
    }

    private void FixedUpdate()
    {
        rigid.AddRelativeForce(new Vector2(0, input.y) * force);
        rigid.AddTorque(-input.x * 6);
    }
}
