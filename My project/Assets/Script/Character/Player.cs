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
        // �W�����v����
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rbody.velocity.y) < 0.01f)
        {
            rbody.AddForce(transform.up * jumpP);
        }
    }

    void FixedUpdate()
    {
        float move = 0f;

        // ���E�ړ�����ƌ���
        if (Input.GetKey(KeyCode.D))
        {
            move = 1f;
            transform.rotation = Quaternion.Euler(0, 0, 0); // �E����
        }
        else if (Input.GetKey(KeyCode.A))
        {
            move = -1f;
            transform.rotation = Quaternion.Euler(0, 180, 0); // ������
        }

        // �ړ�����
        rbody.velocity = new Vector2(move * speed, rbody.velocity.y);

        // �A�j���[�V��������
        if (Mathf.Abs(rbody.velocity.y) > 0.01f)
        {
            // �󒆂ɂ���Ƃ��̓W�����v
            animator.SetBool("isJumping", true);
            animator.SetBool("isRunning", false);
        }
        else if (Mathf.Abs(move) > 0.01f)
        {
            // �n�ʂňړ���
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", true);
        }
        else
        {
            // �n�ʂŒ�~��
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", false);
        }
    }
}
