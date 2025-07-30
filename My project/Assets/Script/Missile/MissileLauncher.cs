using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField] Transform player;      // プレイヤー
    [SerializeField] Transform firePoint;    // ミサイルの発射位置
    [SerializeField] GameObject missilePrefab; // ミサイルプレハブ
    [SerializeField] float attackRange = 10f; // 発射する最短距離

    private bool isMissileActive = false; // ミサイルがアクティブかどうか

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);

        if (distance <= attackRange && !isMissileActive)
        {
            FireMissile();
        }
    }

    void FireMissile()
    {
        var missile = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
        isMissileActive = true;

        // HomingMissile の Target を指定も可能
        var homing = missile.GetComponent<HomingMissile>();

        if (homing)
        {
            homing.transform.rotation = firePoint.rotation;
        }

        // ミサイルが破壊された時に通知
        missile.AddComponent<MissileTracker>().Init(this);
    }

    // ミサイルが消滅した時に再発射可能にする
    public void OnMissileDestroyed()
    {
        isMissileActive = false;
    }
}

public class MissileTracker : MonoBehaviour
{
    private MissileLauncher launcher;

    public void Init(MissileLauncher launcher)
    {
        this.launcher = launcher;
    }

    private void OnDestroy()
    {
        if (launcher)
        {
            launcher.OnMissileDestroyed();
        }
    }
}
