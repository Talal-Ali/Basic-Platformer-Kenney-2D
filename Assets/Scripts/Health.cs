using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public Checkpoints checkpoints;
    public PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;   
    }

    // Update is called once per frame
    void Update()
    {
        Death();   
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            int enemyDamage = collision.gameObject.GetComponent<Enemy>().damage;
            health -= enemyDamage;
            Debug.Log("Damage Taken: " + enemyDamage);
        }   
    }
    void Death()
    {
        if (health <= 0)
        {
            Debug.Log("Player has died.");
            this.transform.position = checkpoints.GetActiveCheckpoint().position;
            health = maxHealth;
            playerMovement.rb.linearVelocity = Vector2.zero;
        }
    }
    public float healthpercentage()
    {
        float healthPercent = (float)health / maxHealth * 100f;
        return healthPercent;
    }
}
