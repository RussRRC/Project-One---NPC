using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    private Vector3 target;// The target position NPC wanders to randomly.

    public Transform targetObject; //select player that it will detect and follow
    public float moveSpeed = 2f; //Move speed for the NPC

    public Vector3 startingPoint; //Origin point for NPC

    public SpriteRenderer alertSprite;

    public float wanderRadius;
    public float detectionRadius;

    private bool reachPoint;

    // Start is called before the first frame update
    void Start()
    {
        //Set spawn point for NPC;
        startingPoint = transform.position;
        target = startingPoint;


    }


    // Update is called once per frame
    void Update()
    {

        // Move our position a step closer to the target.
        var step = moveSpeed * Time.deltaTime; // calculate distance to move

        var inRadius = playerInRadius();
        if (inRadius == true)
        {//If player is within NPC radius, will trigger follow response 
            //Move the NPC towards the player
            transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position, step / 2);
            //Change color of NPC to display they are alerted
            alertSprite.color = Color.red;
            //Draw circle around target player
            DrawCircle(targetObject.transform.position, 2, 64, Color.green);

        }
        else
        {//move towards the random position that was generated
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            alertSprite.color = Color.white;
            DrawCircle(target, 2, 64, Color.green);
            RotateTowardsTargetDestination(target);
        }
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {//When target destination is reached, pick new target destinationk
            // Choose a new position for the NPC to wander to
            target = RandomPositionInWanderRadius();
            Debug.Log(target.ToString("F4"));
        }

        //Debug circles to view wander area of NPC and the alert area of NPC
        DrawCircle(startingPoint, wanderRadius, 64, Color.yellow);
        DrawCircle(transform.position, detectionRadius, 64, Color.red);
        DrawCircle(new Vector3(0,0,0), 1, 64, Color.yellow);
    }


    // Method used to create a new random point
    Vector3 RandomPositionInWanderRadius()
    {
        //randomly generate a number within radius limit of NPC, and add it to NPC's current position
        //radius is limited by starting point of NPC

        //Generates random point in a circle, then multiplies it by radius in order to get true length
        //then adds starting point in order to adjust for where the NPC is setup for movement.
        return Random.insideUnitCircle * wanderRadius + new Vector2(startingPoint.x, startingPoint.y);
    }

    void RotateTowardsTargetDestination(Vector3 destination)
    {
        Vector3 movement = destination - transform.position;

        Vector2 up = transform.up;
        Debug.DrawLine(transform.position, transform.position + (Vector3)up * 5f);
        Debug.DrawRay(transform.position, (Vector3)up * 5f);
        float signedAngle = Vector2.SignedAngle(up, movement);

        transform.Rotate(new Vector3(0, 0, signedAngle));
    }

    bool playerInRadius()
    {
        if(Vector3.Distance(transform.position, targetObject.transform.position) < detectionRadius)
        {
            return true;
        }
        else
        {
            return false;
        }
;
    }

    bool facingDestination(Vector3 origin, Vector3 destination)
    {
        //origin = origin.normalized;
        //destination = destination.normalized;
        if (Vector3.Dot(origin, destination) == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void DrawCircle(Vector3 position, float radius, int segments, Color color)
    {

        // If either radius or number of segments are less or equal to 0, skip drawing
        if (radius <= 0.0f || segments <= 0)
        {
            return;
        }

        // Single segment of the circle covers (360 / number of segments) degrees
        float angleStep = (360.0f / segments);

        // Result is multiplied by Mathf.Deg2Rad constant which transforms degrees to radians
        // which are required by Unity's Mathf class trigonometry methods

        angleStep *= Mathf.Deg2Rad;

        // lineStart and lineEnd variables are declared outside of the following for loop
        Vector3 lineStart = Vector3.zero;
        Vector3 lineEnd = Vector3.zero;

        for (int i = 0; i < segments; i++)
        {
            // Line start is defined as starting angle of the current segment (i)
            lineStart.x = Mathf.Cos(angleStep * i);
            lineStart.y = Mathf.Sin(angleStep * i);

            // Line end is defined by the angle of the next segment (i+1)
            lineEnd.x = Mathf.Cos(angleStep * (i + 1));
            lineEnd.y = Mathf.Sin(angleStep * (i + 1));

            // Results are multiplied so they match the desired radius
            lineStart *= radius;
            lineEnd *= radius;

            // Results are offset by the desired position/origin 
            lineStart += position;
            lineEnd += position;

            // Points are connected using DrawLine method and using the passed color
            Debug.DrawLine(lineStart, lineEnd, color);
        }
    }
}
