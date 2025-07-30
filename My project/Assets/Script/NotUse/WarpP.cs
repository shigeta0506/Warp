using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpP : MonoBehaviour
{
    public float maxDistance = 3f;
    public Transform WarpPoint;  // ワープ先の Transform

    void Update()
    {
        if (Input.GetMouseButtonDown(1))  // 右クリック
        {
            Vector2 origin = transform.position;
            Vector2 direction = transform.up;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance);

            if (hit.collider != null)
            {
                Transform hitTransform = hit.collider.transform;

                if (hitTransform.childCount > 0)
                {
                    WarpPoint = hitTransform.GetChild(0);
                    Debug.Log("ヒットしたオブジェクトの子をWarpPointに設定: " + WarpPoint.name);
                }
                else
                {
                    Debug.Log("ヒットしたオブジェクトに子がありません");
                    WarpPoint = null;
                }
            }
            else
            {
                if (transform.childCount > 0)
                {
                    WarpPoint = transform.GetChild(0);
                    Debug.Log("ヒットしなかったので、自分の子をWarpPointに設定: " + WarpPoint.name);
                }
                else
                {
                    Debug.Log("自分自身にも子がありません");
                    WarpPoint = null;
                }
            }
        }
    }
}
