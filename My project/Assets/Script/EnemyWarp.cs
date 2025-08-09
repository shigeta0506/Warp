using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class EnemyWarp : MonoBehaviour
{
    [Header("予測とワープ設定")]
    [SerializeField] private float predictionTime = 0.5f;     // 予測時間
    [SerializeField] private float warpCooldown = 3.0f;       // クールタイム（ワープ間隔）
    [SerializeField] private float offsetBack = 0.5f;         // 予測位置の手前にずらす距離
    [SerializeField] private float warpDelay = 0.5f;          // 予告からワープまでの遅延

    [Header("エフェクト")]
    [SerializeField] private GameObject warpEffectAfter;
    [SerializeField] private GameObject warpWarningEffect;

    [Header("地形判定（Inspectorで Ground レイヤーをセットしてください）")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCastStartHeight = 10f;   // レイの開始高さ（上方）
    [SerializeField] private float groundCastDistance = 20f;     // レイの長さ（下方向）
    [SerializeField] private float groundSnapOffset = 0.05f;     // 地面にめり込まないための微小オフセット
    [SerializeField] private float overlapCheckRadius = 0.12f;   // ワープ先が衝突してないかのチェック半径
    [SerializeField] private int overlapFixMaxAttempts = 8;      // 衝突してたら上方向に移動してリトライする回数

    [Header("フェード")]
    [SerializeField] private float fadeDuration = 0.25f;

    private Transform player;
    private Rigidbody2D playerRb;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // 加速度サンプリング用（FixedUpdateで更新）
    private Vector2 prevSampledVelocity;
    private Vector2 lastSampledVelocity;
    private float prevSampleTime;
    private float lastSampleTime;

    private Vector2 nextWarpTarget;
    private GameObject warningInstance;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("[EnemyWarp] Player が見つかりません (Tag=Player を確認)。");
            enabled = false;
            return;
        }

        player = playerObj.transform;
        playerRb = playerObj.GetComponent<Rigidbody2D>();
        if (playerRb == null)
        {
            Debug.LogError("[EnemyWarp] Player に Rigidbody2D がありません。");
            enabled = false;
            return;
        }

        // 初期サンプル時間設定
        lastSampledVelocity = playerRb.velocity;
        prevSampledVelocity = playerRb.velocity;
        lastSampleTime = Time.time;
        prevSampleTime = Time.time - Time.fixedDeltaTime;

        StartCoroutine(WarpRoutine());
    }

    void FixedUpdate()
    {
        // FixedUpdate で速度をサンプリング（物理ステップと同期）
        prevSampledVelocity = lastSampledVelocity;
        prevSampleTime = lastSampleTime;

        lastSampledVelocity = playerRb.velocity;
        lastSampleTime = Time.time;
    }

    IEnumerator WarpRoutine()
    {
        while (true)
        {
            // 予測位置計算 & 予告出現
            PredictWarpPosition();

            // フェードアウト（視覚演出）
            yield return StartCoroutine(FadeSprite(1f, 0f, fadeDuration));

            // 予告表示待ち
            yield return new WaitForSeconds(warpDelay);

            // ワープ実行
            ExecuteWarp();

            // フェードイン
            yield return StartCoroutine(FadeSprite(0f, 1f, fadeDuration));

            // クールタイム
            yield return new WaitForSeconds(warpCooldown);
        }
    }

    void PredictWarpPosition()
    {
        // 安全に加速度を求める（FixedUpdateでサンプリングした値を使用）
        float dt = lastSampleTime - prevSampleTime;
        if (dt <= Mathf.Epsilon) dt = Time.fixedDeltaTime; // 安全措置

        Vector2 acceleration = (lastSampledVelocity - prevSampledVelocity) / dt;

        Vector2 predicted = (Vector2)player.position
                            + playerRb.velocity * predictionTime
                            + 0.5f * acceleration * predictionTime * predictionTime;

        // 手前にオフセット（速度が小さい場合はオフセット無し）
        Vector2 backOffset = playerRb.velocity.sqrMagnitude > 0.0001f ? playerRb.velocity.normalized * offsetBack : Vector2.zero;
        Vector2 candidate = predicted - backOffset;

        // 地形にスナップするために上方から下へ Raycast
        Vector2 rayStart = candidate + Vector2.up * groundCastStartHeight;
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, groundCastDistance, groundLayer);
        if (hit.collider != null)
        {
            candidate.y = hit.point.y + groundSnapOffset; // 少し上にオフセットして埋まりを防ぐ
        }
        else
        {
            // Raycast が当たらない場合はログ（デバッグ）を残す（Inspectorのパラメータを要確認）
            Debug.LogWarning("[EnemyWarp] Ground に Raycast が当たりませんでした。groundLayer / cast 高さ を確認してください。");
        }

        // ワープ先が他のコライダと重なっていたら上へ少しずつ移動して逃がす（最大 attempt 回）
        int attempts = 0;
        while (attempts < overlapFixMaxAttempts)
        {
            Collider2D overl = Physics2D.OverlapCircle(candidate, overlapCheckRadius, groundLayer);
            if (overl == null) break;
            candidate.y += overlapCheckRadius * 1.5f; // 少しずつ上げる
            attempts++;
        }
        if (attempts >= overlapFixMaxAttempts)
        {
            Debug.LogWarning("[EnemyWarp] ワープ先が衝突している可能性があります。修正が必要です。");
        }

        nextWarpTarget = candidate;

        // 予告エフェクトを配置（表示位置は少し上に）
        if (warpWarningEffect != null)
        {
            if (warningInstance != null) Destroy(warningInstance);
            Vector3 warnPos = (Vector3)nextWarpTarget + Vector3.up * 0.05f;
            warningInstance = Instantiate(warpWarningEffect, warnPos, Quaternion.identity);
        }

        // デバッグログ
        // Debug.Log("[EnemyWarp] Next target: " + nextWarpTarget);
    }

    void ExecuteWarp()
    {
        // Rigidbody2D を使って位置を移動（transform ではなく rb.position を使う）
        rb.position = nextWarpTarget;

        // 重要：ワープ後の残存速度をきれいに消す（特に y 成分）
        rb.velocity = Vector2.zero;
        rb.Sleep(); // 物理挙動をいったん休止させる（必要なら WakeUp で復帰）
        rb.WakeUp();

        // ワープ後エフェクト
        if (warpEffectAfter != null)
        {
            GameObject ef = Instantiate(warpEffectAfter, (Vector3)nextWarpTarget, Quaternion.identity);
            Destroy(ef, 1.0f);
        }

        // 予告エフェクトを消す
        if (warningInstance != null)
        {
            Destroy(warningInstance);
            warningInstance = null;
        }
    }

    IEnumerator FadeSprite(float from, float to, float duration)
    {
        if (spriteRenderer == null) yield break;
        float elapsed = 0f;
        Color c = spriteRenderer.color;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(from, to, elapsed / duration);
            c.a = a;
            spriteRenderer.color = c;
            yield return null;
        }
        c.a = to;
        spriteRenderer.color = c;
    }

    // デバッグ用：Gizmosで予測位置とRayを可視化
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(nextWarpTarget, 0.12f);

        Vector2 rayStart = nextWarpTarget + Vector2.up * groundCastStartHeight;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(rayStart, rayStart + Vector2.down * groundCastDistance);
    }
}
