using System.Collections;
using UnityEngine;

public class Springs : MonoBehaviour
{
    [SerializeField] private float springForce = 20f;
    [SerializeField] private Animator animator;
    [SerializeField] private string activeAnim = "Spring_Active";
    [SerializeField] private string idleAnim = "Idle";
    [SerializeField] private float animResetDelay = 0.5f;

    private bool isSpringing = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isSpringing)
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 launchDirection = transform.up;

                float projected = Vector2.Dot(rb.linearVelocity, launchDirection);
                rb.linearVelocity -= projected * launchDirection;

                rb.AddForce(launchDirection * springForce, ForceMode2D.Impulse);

                if (animator != null)
                {
                    StartCoroutine(PlaySpringAnimation());
                }
            }
        }
    }

    private IEnumerator PlaySpringAnimation()
    {
        isSpringing = true;
        animator.Play(activeAnim);

        yield return new WaitForSeconds(animResetDelay);

        animator.Play(idleAnim);
        isSpringing = false;
    }
}