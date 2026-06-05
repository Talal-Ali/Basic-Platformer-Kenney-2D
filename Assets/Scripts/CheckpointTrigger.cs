using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int index;
    public Checkpoints manager;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            manager.TryActivate(index, transform);
    }
}