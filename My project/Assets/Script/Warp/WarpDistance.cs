using UnityEngine;

public class WarpDistance : MonoBehaviour
{
    [SerializeField] private float checkDistance = 0.5f; // Trapまでの距離しきい値

    private float timer = 0f;
    private bool isTimerRunning = false;

    void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            if (timer >= 3f)
            {
                Manager.warpDistance = 1.0f;
                Debug.Log("3秒経過したので Manager.warpDistance を 1.0 に戻した");
                isTimerRunning = false;
                timer = 0f;
            }
        }
    }

    public void CheckNearTrap(Vector3 warpPos)
    {
        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
        foreach (GameObject trap in traps)
        {
            float distance = Vector3.Distance(warpPos, trap.transform.position);
            if (distance <= checkDistance)
            {
                Manager.warpDistance = 0.5f;
                Debug.Log("Trapが近いので Manager.warpDistance = 0.5 にした");

                // 3秒後に元に戻すためのタイマー開始
                timer = 0f;
                isTimerRunning = true;
                return;
            }
        }

        // Trapが近くに無い場合、何もしない（3秒後に戻す）
    }
}
