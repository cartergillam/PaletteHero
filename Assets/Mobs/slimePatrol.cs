using UnityEngine;

public class SlimePatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public float speed; // Declare speed variable
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Validate the Rigidbody2D and Animator components
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on " + gameObject.name);
        }
        if (anim == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }

        // Validate pointA and pointB, then initialize currentPoint
        if (pointA == null || pointB == null)
        {
            Debug.LogError("PointA and/or PointB not set on " + gameObject.name);
        }
        else
        {
            currentPoint = pointB.transform;
            anim.SetBool("isRunning", true);
        }
    }

    void Update()
    {
        if (currentPoint == null || rb == null) return; // Ensure we have a target and a Rigidbody2D

        // Move towards the current point
        MoveTowardsCurrentPoint();

        // Check proximity to the current point and switch if close
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            if (currentPoint == pointA.transform)
            {
                currentPoint = pointB.transform;
            }
            else
            {
                currentPoint = pointA.transform;
            }
            Flip();
        }
    }

    void MoveTowardsCurrentPoint()
    {
        Vector2 direction = ((Vector2)currentPoint.position - rb.position).normalized;
        rb.velocity = direction * speed;

        // Check direction to flip the sprite if necessary
        if ((currentPoint == pointA.transform && direction.x > 0) || (currentPoint == pointB.transform && direction.x < 0))
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
