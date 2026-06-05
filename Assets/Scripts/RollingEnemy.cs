using UnityEngine;

public class RollingEnemy : Enemy
{
    [Header("Patrol Limits")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float slowDownDistance = 1.5f; // Start slowing at this distance
    [SerializeField] private float minSpeed = 0.3f;         // Never fully stop

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.1f;

    [Header("Health Settings")]
    public int damageAmount = 20;
    private Rigidbody2D rb;
    private bool movingToB = true;
    private bool isGrounded;

    void Start()
    {
        damage = damageAmount;    
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError($"{name}: No Rigidbody2D found!", this);
            enabled = false;
            return;
        }

        if (pointA == null || pointB == null)
        {
            Debug.LogError($"{name}: Patrol points not assigned!", this);
            enabled = false;
            return;
        }

        // Allow rotation so the enemy can roll visually
        rb.freezeRotation = false;
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(
            transform.position + Vector3.down * 0.5f,
            groundCheckRadius,
            groundLayer
        );

        if (!isGrounded) return;

        Transform target = movingToB ? pointB : pointA;
        float distanceToTarget = Mathf.Abs(transform.position.x - target.position.x);

        // Check if we've reached the target point
        if (movingToB && transform.position.x >= pointB.position.x)
        {
            movingToB = false;
            Flip();
            return;
        }
        else if (!movingToB && transform.position.x <= pointA.position.x)
        {
            movingToB = true;
            Flip();
            return;
        }

        // Lerp speed: full speed far away, slows as it approaches the target
        float t = Mathf.Clamp01(distanceToTarget / slowDownDistance);
        float currentSpeed = Mathf.Lerp(minSpeed, moveSpeed, t);

        float direction = movingToB ? 1f : -1f;

        // Move horizontally
        rb.linearVelocity = new Vector2(direction * currentSpeed, rb.linearVelocity.y);

        // Roll: spin the body based on movement direction and speed
        // Negative angular velocity = clockwise = rolls forward when moving right
        rb.angularVelocity = -direction * currentSpeed * 150f;
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * 0.5f, groundCheckRadius);

        // Visualize slow down zones
        if (pointA != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(pointA.position, slowDownDistance);
        }
        if (pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(pointB.position, slowDownDistance);
        }
    }
}