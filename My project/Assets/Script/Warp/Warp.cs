using UnityEngine;

public class Warp : MonoBehaviour
{
    [SerializeField] public AreaWidth areaWidth;
    public Transform playerTransform;
    [SerializeField] private WarpDistance warpDistanceChecker;

    private bool wasRightClickHeld = false;

    void Update()
    {
        if (areaWidth == null || playerTransform == null) return;

        bool isRightClickHeld = Input.GetMouseButton(1);

        // 右クリック離した瞬間にワープ＆距離チェック
        if (wasRightClickHeld && !isRightClickHeld)
        {
            WarpToMousePosition();
        }

        wasRightClickHeld = isRightClickHeld;
    }

    void WarpToMousePosition()
    {
        Vector3 playerPos = playerTransform.position;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = playerPos.z;

        Vector3 direction = mouseWorldPos - playerPos;
        float radius = areaWidth != null ? areaWidth.CurrentScale() : direction.magnitude;

        Vector3 warpPos;
        if (radius <= 0.01f)
        {
            warpPos = mouseWorldPos;
        }
        else
        {
            Vector3 normalizedDir = direction.normalized;
            warpPos = playerPos + normalizedDir * radius;
        }

        float distanceWarped = Vector3.Distance(playerPos, warpPos);
        Debug.Log($"Warped distance: {distanceWarped} (半径: {radius})");

        playerTransform.position = warpPos;

        // ここで近くに Trap があるか判定
        if (warpDistanceChecker != null)
        {
            warpDistanceChecker.CheckNearTrap(warpPos);
        }

        // リセット
        areaWidth.warp = 0f;
        areaWidth.JustReleased = false;
    }
}
