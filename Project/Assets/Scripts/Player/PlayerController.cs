using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController2D controller;
    public Transform floorCheck;
    public LayerMask m_WhatIsGround;
    public ParticleSystem hitEffect;
    public Animator animator;

    public float velocityMultipler;
    public float attackDistanceHorizontal;
    public float attackDistanceVertical;
    public float animationSpeedMultipler;

    Rigidbody2D rb;

    private float horizontalMovement;
    private bool jump;
    private bool mainAttack;

    private bool right;

    bool m_Grounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * velocityMultipler;
        if (horizontalMovement > 0) right = true;
        if (horizontalMovement < 0) right = false;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            mainAttack = true;
        }

        // set animations
        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x) * animationSpeedMultipler);
        animator.SetBool("jump", rb.velocity.y > .3f);
        animator.SetBool("fall", rb.velocity.y < -.3f);
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(floorCheck.transform.position, .2f, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                break;
            }
        }
        
        if (mainAttack)
        {
            MainAttack();
            mainAttack = false;
        }

        controller.Move(horizontalMovement * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    Vector3 GetDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            return new Vector3(0, 1);
        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !m_Grounded)
            return new Vector3(0, -1);
        if (right)
            return new Vector3(1, 0);
        return new Vector3(-1, 0);
    }

    void MainAttack()
    {
        Vector3 direction = GetDirection();
        Vector3 attackPoint = direction;
        if (attackPoint.x != 0) attackPoint *= attackDistanceHorizontal;
        if (attackPoint.y != 0) attackPoint *= attackDistanceVertical;
        attackPoint += transform.position;

        hitEffect.transform.position = attackPoint;
        hitEffect.transform.rotation = Quaternion.Euler(new Vector3(0, 0, direction.x > 0 ? -45 : direction.x < 0 ? 135 : direction.y < 0 ? 225 : 45));
        hitEffect.Play();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint, .2f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if (colliders[i].TryGetComponent(out SimpleObject obj))
                {
                    if (obj.canPogoJump)
                    {
                        controller.ForceJump();
                    }
                }
            }
        }
    }
}
