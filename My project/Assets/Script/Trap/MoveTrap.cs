using UnityEngine;

public class MoveTrap : MonoBehaviour
{
    [SerializeField] private string targetName = "TargetPoint";
    [SerializeField] private float baseMoveSpeed = 5f; // 基本スピード

    private Transform targetPoint;

    void OnEnable()
    {
        var target = GameObject.Find(targetName);
        if (target)
        {
            targetPoint = target.transform;
        }
        else
        {
            targetPoint = null;
            Debug.LogWarning("指定されたオブジェクトが見つかりません。: " + targetName);
        }
    }

    void Update()
    {
        if (targetPoint == null) return;

        // warpDistanceが0.5ならスピード半分にする（1.0なら元のまま）
        float speedModifier = Manager.warpDistance;
        float currentSpeed = baseMoveSpeed * speedModifier;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint.position,
            currentSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.05f)
        {
            Destroy(gameObject);
        }
    }
}
