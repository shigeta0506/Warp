using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;   // 追従対象（プレイヤー）
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // カメラの位置補正
    [SerializeField] private float followSpeed = 10f; // 追従速度

    void LateUpdate()
    {
        if (target == null) return;

        // スムーズに追従（直接追従したいならLerpを省略）
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}
