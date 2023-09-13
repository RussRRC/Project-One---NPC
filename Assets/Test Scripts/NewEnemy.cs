using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : MonoBehaviour
{

    public Vector2 moveVector;

    public float timer;
    public float maxTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Non-physics way to move
        //transform.Translate(moveVector * Time.deltaTime);
    }
}
