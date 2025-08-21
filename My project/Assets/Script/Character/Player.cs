using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 5f;
    private float jumpP = 300f;

    Rigidbody2D rbody;
    Animator animator;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ジャンプ入力
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rbody.velocity.y) < 0.01f)
        {
            rbody.AddForce(transform.up * jumpP);
        }
    }

    void FixedUpdate()
    {
        float move = 0f;

        // 左右移動判定と向き
        if (Input.GetKey(KeyCode.D))
        {
            move = 1f;
            transform.rotation = Quaternion.Euler(0, 0, 0); // 右向き
        }
        else if (Input.GetKey(KeyCode.A))
        {
            move = -1f;
            transform.rotation = Quaternion.Euler(0, 180, 0); // 左向き
        }

        // 移動処理
        rbody.velocity = new Vector2(move * speed, rbody.velocity.y);

        // アニメーション判定
        if (Mathf.Abs(rbody.velocity.y) > 0.01f)
        {
            // 空中にいるときはジャンプ
            animator.SetBool("isJumping", true);
            animator.SetBool("isRunning", false);
        }
        else if (Mathf.Abs(move) > 0.01f)
        {
            // 地面で移動中
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", true);
        }
        else
        {
            // 地面で停止中
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", false);
        }
    }
}
