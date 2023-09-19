using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcVerTwo : MonoBehaviour
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
    private bool rightAngle;
    private bool chase;
    [SerializeField] private float _fireRate = 1.0f;
    [SerializeField] private float _cycleTime = 0.0f;
    [SerializeField] private float shootForce = 50f;
    [SerializeField] private Rigidbody2D _bullet;
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

        state moveState;
        moveState = state.wandering;
        //Set Rigidbody2D targets
        rigid = GetComponent<Rigidbody2D>();
        targetRigid = targetObject.GetComponent<Rigidbody2D>();

        //Set origin point for NPC
        startingPoint = transform.position;

        //Set selfTransform transform variable
        selfTransform = transform;

        //Set first random point to wander to
        target = RandomPositionInWanderRadius();

        //Debug to see what the position chosen is
        if (debugMode)
        {
            Debug.Log(target.ToString("F4"));
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        //Checks to see if player is in radius of the NPC
        //Needs to be constantly checking
        if(inRadius)//If player is within Radius
        {
            //set rightAngle to detect target object instead of random wander point
            rightAngle = FacingDestination(targetObject.position);

            //If target Object is within FOV detection of the NPC
            if(chase)
            {

                //NPC now rotates towards the target object's percieved destination
                RotateTowardsTargetDirection(targetObject.position + positionAdjuster());
               
                //Since the NPC has entered chase mode, NPC moves 2x as fast
                selfTransform.Translate(new Vector2(0, moveSpeed) * Time.deltaTime * moveSpeed);

                //Full chase mode, fully alerted state
                alertSprite.color = Color.red;
            }
            else if(rightAngle == false)//if not facing towards the target object, will rotate towards it
            {

                //NPC rotates towards the target object's position
                RotateTowardsTargetDirection(targetObject.position );

                //NPC moves slower than chase state as target Object has not entered FOV
                selfTransform.Translate(new Vector2(0, moveSpeed / 4) * Time.deltaTime * moveSpeed);

                //In light chase mode, semi-alerted state
                alertSprite.color = Color.yellow;
                
            }
            else //Since the target object entered FOV, set chase to true to enter chase mode
            {
                chase = true;
            }

            //Debug Mode to view the target position the NPC is aiming for, target object position if non-chase mode, percieved destination if chase mode
            if (debugMode)
            {
                if (!chase)
                {
                    DrawCircle(targetObject.transform.position, 2, 64, Color.green);
                }
                else
                {
                    DrawCircle(targetObject.transform.position + positionAdjuster(), 2, 64, Color.green);
                }
                
            }
            target = startingPoint;
        }
        else //If player is not in radius, focus on target position
        {
            //Set sprite to unalerted state
            alertSprite.color = Color.white;

            //Set chase mode to false in case the NPC was in chase mode before Target Object left detection radius
            chase = false;

            //set the angle target to the random point/starting point
            rightAngle = FacingDestination(target);

            //if not facing towards the target point yet, move forwards slowly 
            if(rightAngle == false)
            {    
                //NPC moves forwards slowly as it turns towards target
                selfTransform.Translate(new Vector2(0, moveSpeed/4) * Time.deltaTime * moveSpeed);
            }
            else //If facing towards target point
            {
                //NPC moves faster when facing towards target point
                selfTransform.Translate(new Vector2(0, moveSpeed) * Time.deltaTime * moveSpeed);
            }

            //NPC rotates towards the target wander position
            RotateTowardsTargetDirection(target);

            //Debug Mode to view the target wander point
            if (debugMode)
            {
                DrawCircle(target, 2, 64, Color.green);
            }
        }

        //If target object is within the firing Radius of the NPC and the NPC is in chase mode, will fire a projectile
        if (inFireRange && chase)
        {
            //Sets sprite's color to black
            alertSprite.color = Color.black;

            //Initiates fireProjectile
            if (Time.time > _cycleTime)
            {
                //Update cycleTime to process next bullet firing time
                _cycleTime = Time.time + _fireRate;

                //fire the projectile
                if (Vector3.Angle(targetObject.position - selfTransform.position, selfTransform.up) < 20f)
                {
                    FireProjectile();
                }
            }
        }
        
        //If NPC has reached within n units of the destination
        if (reachPoint)
        {
            //Assign new random point to check
            target = RandomPositionInWanderRadius();

            //Debug print target position in console
            if (debugMode)
            {
                Debug.Log(target.ToString("F4"));
            }
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

    //Checks to see if the angle between the npc and target destination is less than 10.
    bool FacingDestination(Vector3 destination)
    {
        float angle = 5f;
        if (debugMode)
        {
            Debug.Log(Vector3.Angle(destination - selfTransform.position, selfTransform.up).ToString("F4"));
        }
        if (Vector3.Angle(destination - selfTransform.position, selfTransform.up) < angle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*Method: fireProjectile()
     *Description: Spawns in projectile (_bullet)
     *every n seconds with x amount of force
     */
    void FireProjectile()
    {
        if(_bullet != null)
        {
           
            Rigidbody2D rb = Instantiate<Rigidbody2D>(_bullet, transform.position + transform.up, transform.rotation);    
            rb.AddRelativeForce(Vector2.up * shootForce, ForceMode2D.Impulse);
        }
        else
        {
            if (debugMode)
            {
                Debug.LogError("The BULLET is null");
            }
        }
    }
 

    //Returns the expected displacement of the targetObject in terms of movement.
    Vector3 positionAdjuster()
    {
        float travelTime = 1f;
        //Displacement = Initial Velocity * time + 1/2 * accelleration (force/mass) * time^2
        float xTravelled = targetRigid.velocity.x * travelTime + 1/2 * targetObject.GetComponent<Player>().force / targetRigid.mass * Mathf.Pow(travelTime, 2);
        float yTravelled = targetRigid.velocity.y * travelTime + 1/2 * targetObject.GetComponent<Player>().force / targetRigid.mass * Mathf.Pow(travelTime, 2);
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
