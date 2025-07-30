using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindWarp : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリックで実行
        {
            Vector2 origin = transform.position;

            // マウス位置をワールド座標に変換
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - origin).normalized;
            float distance = Vector2.Distance(origin, mouseWorldPos);

            // レイキャストを全部取得（すべてのヒットを調べる）
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance);

            Transform farthestEnemy = null;
            float maxDist = -1f;

            foreach (var hit in hits)
            {
                if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                {
                    float d = Vector2.Distance(origin, hit.point);
                    if (d > maxDist)
                    {
                        maxDist = d;
                        farthestEnemy = hit.collider.transform;
                    }
                }
            }

            if (farthestEnemy != null)
            {
                if (farthestEnemy.childCount > 0)
                {
                    Transform child = farthestEnemy.GetChild(0);
                    transform.position = child.position;
                    Debug.Log("一番遠い Enemy の子にワープ: " + child.name);
                }
                else
                {
                    Debug.Log("Enemy に子オブジェクトがありません");
                }
            }
            else
            {
                Debug.Log("Enemy タグのついたオブジェクトが見つかりませんでした");
            }
        }
    }
}
