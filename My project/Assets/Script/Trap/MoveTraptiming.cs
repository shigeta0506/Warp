using UnityEngine;

public class MoveTraptiming : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private int Num = 0;
    private int count = 0;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= Manager.GetSpawnInterval()) // Manager‚©‚çŽæ“¾
        {
            SpawnTrap();
            timer = 0f;
        }
    }

    void SpawnTrap()
    {
        count++;
        if (count == Num)
        {

            count = 0;

            return;
        }
        else
        {
            Instantiate(trapPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
