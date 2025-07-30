using UnityEngine;
using UnityEngine.UI;

public class GageGager : MonoBehaviour
{
    private float myHp = 500.0f;
    private Image image;

    private void Start()
    {
        image = this.GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            myHp -= 0.2f;
        }
        else if (!Input.GetMouseButton(1) && myHp < 500f)
        {
            myHp += 0.1f;
        }

        myHp = Mathf.Clamp(myHp, 0, 500);
        image.fillAmount = myHp / 500.0f;
    }

    public float GetHpRatio()
    {
        return myHp / 500.0f;
    }
}
