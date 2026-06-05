using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public Transform spawnPoint;   // Assign your initial spawn in the Inspector
    private Transform activeCheckpoint;
    private int activeCheckpointIndex = -1;

    void Start()
    {
        activeCheckpoint = spawnPoint;
    }

    public void TryActivate(int index, Transform point)
    {
        if (index <= activeCheckpointIndex) return;

        activeCheckpointIndex = index;
        activeCheckpoint = point;
        Debug.Log("Checkpoint reached: " + point.name);
    }

    public Transform GetActiveCheckpoint() => activeCheckpoint;
}
