using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D playerRb;
    public float predictionTime = 0.5f; // 0.5•bŒã‚ğ—\‘ª
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    private Rigidbody2D rbody;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 predictedPos = PredictPlayerPosition();
        float distance = Vector2.Distance(transform.position, predictedPos);

        if (distance > attackRange)
        {
            // —\‘ªˆÊ’u‚ÉŒü‚©‚Á‚ÄˆÚ“®
            Vector2 direction = (predictedPos - (Vector2)transform.position).normalized;
            rbody.velocity = new Vector2(direction.x * moveSpeed, rbody.velocity.y);
        }
        else
        {
            // ‹ß‚Ã‚¢‚½‚çUŒ‚i‚±‚±‚ÉUŒ‚ˆ—‚ğ“ü‚ê‚éj
            Debug.Log("—\‘ª’n“_‚ÉUŒ‚I");
        }
    }

    Vector2 PredictPlayerPosition()
    {
        return (Vector2)player.position + playerRb.velocity * predictionTime;
    }
}
