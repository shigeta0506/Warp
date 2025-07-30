using UnityEngine;

public class Launcherdelete : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 衝突したオブジェクトのレイヤーチェック
        if (collision.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            // Missile を消去
            Destroy(gameObject);
        }
    }
}
