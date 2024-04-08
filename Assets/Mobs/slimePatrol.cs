using UnityEngine;

public class SlimePatrol : MonoBehaviour
{
    public GameObject floatingPoints;
    public GameObject pointA;
    public GameObject pointB;
    public int maxHealth = 3;
    private int currentHealth;
    public float speed; // Declare speed variable
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    private const float directionThreshold = 0.1f; // Tolerance threshold for direction check
    private bool facingRight = true; // Keeps track of the sprite's facing direction
    private Vector3 initialPosition;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Validate components and points
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on " + gameObject.name);
        }
        if (anim == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
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

    private void Update()
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

    private void MoveTowardsCurrentPoint()
    {
        Vector2 direction = ((Vector2)currentPoint.position - rb.position).normalized;
        rb.velocity = direction * speed;

        // Check direction to flip the sprite if necessary
        if (Mathf.Abs(direction.x) > directionThreshold)
        {
            if (direction.x > 0 && !facingRight || direction.x < 0 && facingRight)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // Reduce current health by the damage amount

        if (currentHealth <= 0)
        {
            Die(); // Call Die function if health is zero or below
        }
    }


    public void Die(){
        Destroy(gameObject);
    }

}
