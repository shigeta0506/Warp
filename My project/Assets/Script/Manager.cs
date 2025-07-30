using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 2f;
    private static Manager instance;

    public static float warpDistance = 1.0f;

    private void Awake()
    {
        instance = this;
    }

    public static float GetSpawnInterval()
    {
        return instance.spawnInterval;
    }
}
