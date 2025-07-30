using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpSender : MonoBehaviour
{
    [SerializeField] private Transform warpTarget; //ワープ先
    [SerializeField] private float launchForce = 5f; //飛び出す力

    private void Update()
    {
        //warpTargetが設定されていない場合、タグに応じたワープターゲットを再取得
        if (warpTarget == null)
        {
            if (CompareTag("Warp1"))
            {
                GameObject warpTargetObject = GameObject.FindGameObjectWithTag("WarpP2");
                if (warpTargetObject != null)
                {
                    warpTarget = warpTargetObject.transform;
                    Debug.Log("Warp1タグなのでWarp2ターゲットを再設定");
                }
                else
                {
                    Debug.LogWarning("Warp2タグのオブジェクトが見つかりませんでした");
                }
            }
            else if (CompareTag("Warp2"))
            {
                GameObject warpTargetObject = GameObject.FindGameObjectWithTag("WarpP1");
                if (warpTargetObject != null)
                {
                    warpTarget = warpTargetObject.transform;
                    Debug.Log("Warp2タグなのでWarp1ターゲットを再設定");
                }
                else
                {
                    Debug.LogWarning("Warp1タグのオブジェクトが見つかりませんでした");
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (warpTarget != null)
        {
            Rigidbody2D targetRb = collision.collider.GetComponent<Rigidbody2D>();

            if (targetRb != null)
            {
                // ワープさせる
                collision.transform.position = warpTarget.position;

                // 速度リセット
                targetRb.velocity = Vector2.zero;

                // warpTargetの回転を使って、飛び出す方向を正しく計算
                Vector2 launchDirection = warpTarget.rotation * Vector3.up;

                // 飛び出し
                targetRb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
            }
            else
            {
                Debug.LogWarning("ワープ対象にRigidbody2Dがついていません！");
            }
        }
        else
        {
            Debug.LogWarning("WarpSenderにワープ先が設定されていません！");
        }
    }
}
