using UnityEngine;

public class Deadzone : MonoBehaviour
{
    private Transform respawnPoint;
    public Checkpoints checkpoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        respawnPoint = checkpoints.GetActiveCheckpoint();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Health health = collision.GetComponent<Health>();
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

        collision.transform.position = respawnPoint.position;
        rb.linearVelocity = Vector2.zero;

        // Restore full health only if dead, otherwise leave it alone
        if (health.health <= 0)
            health.health = health.maxHealth;
    }
}
