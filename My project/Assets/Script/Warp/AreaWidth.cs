using UnityEngine;

public class AreaWidth : MonoBehaviour
{
    private float baseScale = 2.0f;         // 通常サイズ
    private float expandedScale = 20.0f;    // 最大拡大サイズ
    private float expandSpeed = 2.0f;       // 拡大スピード

    private float currentScale;

    public float warp = 0f;                // ワープ距離（直径）
    public bool justReleased = false;      // ワープ距離を更新済みかフラグ

    private void Start()
    {
        currentScale = baseScale;
        transform.localScale = new Vector3(currentScale, currentScale, 1f);
    }

    public void Update()
    {
        if (!Input.GetMouseButton(1))
        {
            if (!justReleased)
            {
                warp = currentScale;
                justReleased = true;
            }
            currentScale = baseScale;
        }
        else
        {
            justReleased = false;
            currentScale = Mathf.MoveTowards(currentScale, expandedScale, expandSpeed * Time.deltaTime);
        }

        transform.localScale = new Vector3(currentScale, currentScale, 1f);
    }

    public float CurrentScale()
    {
        return warp * 0.5f * 0.5f;
    }

    public bool JustReleased
    {
        get => justReleased;
        set => justReleased = value;
    }
}
