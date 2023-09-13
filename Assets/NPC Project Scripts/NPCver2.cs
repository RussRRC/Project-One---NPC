using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCver2 : MonoBehaviour
{
    enum state
    {
        wandering,
        sensing,
        chasing,
        firing,
        returning,
    }
    public Transform selfTransform;
    public Transform targetObject;
    public Vector3 startingPoint;
    public SpriteRenderer alertSprite;

    public float wanderRadius;
    public float detectionRadius;
    public float firingRadius;
    public float moveSpeed;
    public bool debugMode;

    private Rigidbody2D rigid;
    private Vector3 target;
    private Vector2 speed;
    private bool rightAngle;
    private bool chase;
    [SerializeField] private float _fireRate = 1.0f;
    [SerializeField] private float _cycleTime = 0.0f;
    [SerializeField] private Rigidbody2D _bullet;
    private Rigidbody2D Rigid;
    private Rigidbody2D targetRigid;

    private bool reachPoint
    {
        get
        {
            if (Vector3.Distance(selfTransform.position, target) < 0.5f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private bool inRadius
    {
        get
        {
            if (Vector3.Distance(transform.position, targetObject.transform.position) < detectionRadius)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
    }
    private bool inFireRange
    {
        get
        {
            if(Vector3.Distance(transform.position, targetObject.transform.position) < firingRadius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        startingPoint = transform.position;
        target = RandomPositionInWanderRadius();
        selfTransform = transform;
        speed = new Vector2(1, 1);
        //Debug to see what the position chosen is
        Debug.Log(target.ToString("F4"));
        Rigid = GetComponent<Rigidbody2D>();
        targetRigid = targetObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Checks to see if player is in radius of the NPC
        //Needs to be constantly checking

        if(inRadius)//If player is within Radius
        {
            //
            rightAngle = facingDestination(targetObject.position);
            if(chase)
            {
                RotateTowardsTargetDirection(targetObject.position + positionAdjuster());
                //If facing player, move fast towards them
                
                selfTransform.Translate(speed.magnitude * new Vector2(0, moveSpeed) * Time.deltaTime * moveSpeed);
                //Full chase mode, fully alerted state
                alertSprite.color = Color.red;
            }
            else if(rightAngle == false)//if not facing towards the target object, will rotate towards it
            {

                //Rotate toards the player
                RotateTowardsTargetDirection(targetObject.position );

                //Slowly move towards them
                
                selfTransform.Translate(speed.magnitude * new Vector2(0, moveSpeed / 4) * Time.deltaTime * moveSpeed);
                //In light chase mode, semi-alerted state
                alertSprite.color = Color.yellow;
                
            }
            else
            {
                chase = true;
            }
            if (debugMode)
            {
                DrawCircle(targetObject.transform.position + positionAdjuster(), 2, 64, Color.green);
            }
            target = startingPoint;
        }
        else //If player is not in radius, focus on target position
        {
            //Set sprite to unalerted state
            alertSprite.color = Color.white;
            chase = false;

            //set the angle target to the random point/starting point
            rightAngle = facingDestination(target);

            //if not facing towards the target point yet, move forwards slowly while 
            if(rightAngle == false)
            {
                RotateTowardsTargetDirection(target);
                
                selfTransform.Translate(speed.magnitude * new Vector2(0, moveSpeed/4) * Time.deltaTime * moveSpeed);
            }
            else 
            {
                
                selfTransform.Translate(speed.magnitude * new Vector2(0, moveSpeed) * Time.deltaTime * moveSpeed);
            }
            RotateTowardsTargetDirection(target);
            if (debugMode)
            {
                DrawCircle(target, 2, 64, Color.green);
            }
        }
        if (inFireRange && chase)
        {
            fireProjectile();
        }
        
        //If NPC has reached within n units of the destination
        if (reachPoint)
        {
            //Assign new random point to check
            target = RandomPositionInWanderRadius();
            Debug.Log(target.ToString("F4"));
        }


        //Debug circles to view wander area of NPC and the alert area of NPC
        if (debugMode)
        {
            DrawCircle(startingPoint, wanderRadius, 64, Color.yellow);
            DrawCircle(transform.position, detectionRadius, 64, Color.red);
            DrawCircle(startingPoint, 1, 64, Color.yellow);
            DrawCircle(transform.position, firingRadius, 64, Color.magenta);
        }
    }

    private void FixedUpdate()
    {
        
    }
    //Creates a new Vector3 within set Radius limit
    Vector3 RandomPositionInWanderRadius()
    {
        //randomly generate a number within radius limit of NPC, and add it to NPC's current position
        //radius is limited by starting point of NPC

        //Generates random point in a circle, then multiplies it by radius in order to get true length
        //then adds starting point in order to adjust for where the NPC is setup for movement.
        return Random.insideUnitCircle * wanderRadius + new Vector2(startingPoint.x, startingPoint.y);
    }

    void RotateTowardsTargetDirection(Vector3 destination)
    {
        Vector2 up = selfTransform.up;


        //Finds the Vector3 between the NPC position and the target position needed for signedangle.
        Vector3 movementAngle = destination - selfTransform.position;

        //Debug that draws from position to forward direction
        Debug.DrawLine(selfTransform.position, selfTransform.position + (Vector3)up * 5f);

        //Taes angle between forward direction and target position
        float signedAngle = Vector2.SignedAngle(up, movementAngle);

        //instant rotation
        //selfTransform.Rotate(new Vector3(0, 0, signedAngle));

        //Quaternion rotation in order to get a smooth turn towards target point.
        Quaternion startRotation = selfTransform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 0, signedAngle);

        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, Time.deltaTime * moveSpeed);
        

    }

    //Checks to see if player is within Detection Radius of the NPC
    //Returns true if within detection radius
    
    /*
     * Old check for player in Radius, moved to the variable with accessors
    bool playerInRadius()
    {
        if (Vector3.Distance(transform.position, targetObject.transform.position) < detectionRadius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    */

    //Checks to see if the angle between the npc and target destination is less than 10.
    bool facingDestination(Vector3 destination)
    {
        float angle = 5f;
        Debug.Log(Vector3.Angle(destination - selfTransform.position, selfTransform.up).ToString("F4"));
        if (Vector3.Angle(destination - selfTransform.position, selfTransform.up) < angle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void fireProjectile()
    {
        if(Time.time > _cycleTime)
        {
            _cycleTime = Time.time + _fireRate;
            if(_bullet != null)
            {
                Rigidbody2D rb = Instantiate<Rigidbody2D>(_bullet, transform.position + transform.up, transform.rotation);
                rb.AddRelativeForce(Vector2.up * 50f, ForceMode2D.Impulse);
            }
            else
            {
                Debug.LogError("The BULLET is null");
            }
        }
    } 

    Vector3 positionAdjuster()
    {
        float xTravelled = targetRigid.velocity.x * targetObject.GetComponent<Player>().speed / targetRigid.mass * 0.03f;
        float yTravelled = targetRigid.velocity.y * targetObject.GetComponent<Player>().speed / targetRigid.mass * 0.03f;
        return new Vector3(xTravelled, yTravelled);
    }

    //Debug command in order to draw a circle dependant on what the radius of the circle is, and a center position
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
