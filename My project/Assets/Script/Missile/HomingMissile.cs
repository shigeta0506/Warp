using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed = 5f;
    private float rotationSpeed = 300f;

    void OnEnable()
    {
        // 有効化時にプレイヤータグから再取得して設定
        var player = GameObject.FindGameObjectWithTag("Player");

        if (player)
        {
            target = player.transform;
        }
        else
        {
            target = null;
            Debug.LogWarning("Player タグのオブジェクトが見つかりません。");

        }
    }

    void Update()
    {
        if (target == null) return;

        // ターゲットとの向きを算出
        Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
        direction.Normalize();

        // 現在の向きも取得
        float currentRotation = transform.rotation.eulerAngles.z;

        // 目的の向きを算出
        float targetRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // 補間して回転
        float newRotation = Mathf.MoveTowardsAngle(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, newRotation);

        // 前に移動
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // obstacleレイヤーに当たったら消滅
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Debug.Log("当たり");
            Destroy(gameObject);
        }
    }
}
