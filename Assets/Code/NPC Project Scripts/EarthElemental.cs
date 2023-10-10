using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EarthElemental : MonoBehaviour
{

    [SerializeField] private Transform targetObject;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float _cycleTime = 0.0f;
    [SerializeField] private float _fireRate = 2.0f;
    public float detectionRadius;
    public float shootForce;
    public bool debugMode;
    public Rigidbody2D _bullet;
    private Sense currentState = Sense.noDetection;

    enum Sense
    {
        noDetection,
        detection,
        attacking,
    }
    // Start is called before the first frame update
    void Start()
    {        
        targetObject = GameObject.Find("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
        if (currentState == Sense.detection)
        {
            RotateTowardsTargetDirection(targetObject.position);
        }
        else if (currentState == Sense.attacking)
        {
            if(Time.time > _cycleTime)
            {
                _cycleTime = Time.time + _fireRate;
                fireProjectile();
                Destroy(gameObject);
            }
        }
        else if (currentState == Sense.noDetection)
        {
            RotateTowardsTargetDirection(new Vector3(0, 0));
        }
        if (debugMode)
        {
            DrawCircle(transform.position, detectionRadius, 64, Color.yellow);
        }
    }

    void CheckDistance()
    {
        if (Vector3.Distance(transform.position, targetObject.position) < detectionRadius)
        {
            float angle = 5f;
            if (Vector3.Angle(targetObject.position - transform.position, transform.up) < angle)
            {
                currentState = Sense.attacking;
            }
            else
            {
                currentState = Sense.detection;
            }
        }
        else
        {
            currentState = Sense.noDetection;
        }
    }
    void RotateTowardsTargetDirection(Vector3 destination)
    {
        Vector2 up = transform.up;


        //Finds the Vector3 between the NPC position and the target position needed for signedangle.
        Vector3 movementAngle = destination - transform.position;

        //Debug that draws from position to forward direction
        Debug.DrawLine(transform.position, transform.position + (Vector3)up * 5f);

        //Taes angle between forward direction and target position
        float signedAngle = Vector2.SignedAngle(up, movementAngle);

        //instant rotation
        //selfTransform.Rotate(new Vector3(0, 0, signedAngle));

        //Quaternion rotation in order to get a smooth turn towards target point.
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 0, signedAngle);

        transform.rotation = Quaternion.RotateTowards(startRotation, targetRotation, moveSpeed * Time.deltaTime);

    }

    void fireProjectile()
    {
        if(_bullet != null)
        {

            Rigidbody2D rb = Instantiate<Rigidbody2D>(_bullet, transform.position, transform.rotation);
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
