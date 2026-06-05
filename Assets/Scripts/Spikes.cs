using UnityEngine;

public class Spikes : Enemy
{
    [Header("Damage")]
    public int damageAmount = 3;

    [Header("Knockback")]
    [SerializeField] private float knockbackForceX = 5f;
    [SerializeField] private float knockbackForceY = 8f;

    void Start()
    {
        damage = damageAmount;    
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            // Knock the player away from the spike's center
            float direction = Mathf.Sign(collision.transform.position.x - transform.position.x);
            playerRb.linearVelocity = Vector2.zero; // Cancel current momentum first
            playerRb.AddForce(new Vector2(direction * knockbackForceX, knockbackForceY), ForceMode2D.Impulse);
        }


        // Uncomment when PlayerHealth is ready:
        // PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        // if (playerHealth != null) playerHealth.TakeDamage(damage);
    }
}