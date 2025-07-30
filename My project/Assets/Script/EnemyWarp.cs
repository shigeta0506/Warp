using UnityEngine;

public class EnemyWarp : MonoBehaviour
{
    [SerializeField] private float predictionTime = 0.5f;     // 何秒先を予測するか
    [SerializeField] private float warpCooldown = 3.0f;       // ワープのクールタイム（秒）
    [SerializeField] private float offsetBack = 0.5f;         // プレイヤー予測位置の少し後ろにワープする

    [Header("ワープ後のエフェクト")]
    [SerializeField] private GameObject warpEffectAfter;      // ワープ後エフェクト

    private float lastWarpTime = -Mathf.Infinity;
    private Transform player;
    private Rigidbody2D playerRb;

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

        if (Time.time - lastWarpTime >= warpCooldown)
        {
            WarpToPredictedPosition();
            lastWarpTime = Time.time;
        }
    }

    void WarpToPredictedPosition()
    {
        // 予測位置を計算（位置 + 速度 * 時間）
        Vector2 predictedPosition = (Vector2)player.position + playerRb.velocity * predictionTime;

        // プレイヤーの進行方向に少し手前にワープ
        Vector2 backOffset = playerRb.velocity.normalized * offsetBack;
        Vector2 warpTarget = predictedPosition - backOffset;

        // ワープ実行
        transform.position = warpTarget;

        // --- ワープ後のエフェクトのみ ---
        if (warpEffectAfter != null)
        {
            GameObject effect = Instantiate(warpEffectAfter, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
        }

        Debug.Log("敵がプレイヤーの予測位置にワープしました: " + warpTarget);
    }
}
