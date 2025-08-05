using UnityEngine;

public class EnemyWarp : MonoBehaviour
{
    [SerializeField] private float predictionTime = 0.5f;     // 予測時間
    [SerializeField] private float warpCooldown = 3.0f;       // クールタイム
    [SerializeField] private float offsetBack = 0.5f;         // 少し手前にワープ

    [Header("ワープ用エフェクト")]
    [SerializeField] private GameObject warpEffectAfter;      // ワープ後の爆発エフェクト
    [SerializeField] private GameObject warpWarningEffect;    // ワープ前の予告エフェクト

    private float lastWarpTime = -Mathf.Infinity;
    private Transform player;
    private Rigidbody2D playerRb;

    private GameObject warningInstance;   // 現在表示中の予告エフェクト
    private Vector2 nextWarpTarget;       // 次のワープ先を記録

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerRb = playerObj.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogError("Playerが見つかりません。Tagが正しいか確認してください。");
        }
    }

    void Update()
    {
        if (player == null || playerRb == null) return;

        // クールタイムが過ぎていれば予測して目印を出す
        if (Time.time - lastWarpTime >= warpCooldown)
        {
            PredictWarpPosition();  // 予告を出す
            Invoke(nameof(ExecuteWarp), 0.5f);  // 少し遅れてワープ（0.5秒後）
            lastWarpTime = Time.time;
        }
    }

    // ワープ位置を予測し、そこに目印を出す
    void PredictWarpPosition()
    {
        Vector2 predictedPosition = (Vector2)player.position + playerRb.velocity * predictionTime;
        Vector2 backOffset = playerRb.velocity.normalized * offsetBack;
        nextWarpTarget = predictedPosition - backOffset;

        // 予告エフェクトを配置
        if (warpWarningEffect != null)
        {
            if (warningInstance != null) Destroy(warningInstance); // 前回のを消す
            warningInstance = Instantiate(warpWarningEffect, nextWarpTarget, Quaternion.identity);
        }
    }

    // 実際にワープする
    void ExecuteWarp()
    {
        transform.position = nextWarpTarget;

        // ワープ後エフェクト
        if (warpEffectAfter != null)
        {
            GameObject effect = Instantiate(warpEffectAfter, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
        }

        // 予告エフェクトを消す
        if (warningInstance != null)
        {
            Destroy(warningInstance);
            warningInstance = null;
        }

        Debug.Log("敵が予告地点にワープしました: " + nextWarpTarget);
    }
}
