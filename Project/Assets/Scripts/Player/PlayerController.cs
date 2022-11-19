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
    public HitCollider hitPrefab;

    public float velocityMultipler;
    public float attackDistanceHorizontal;
    public float attackDistanceVertical;
    public float animationSpeedMultipler;

    Rigidbody2D rb;

    private float horizontalMovement;
    private bool jump;
    private bool mainAttack;
    bool mainAttackAnim;

    [HideInInspector]
    public bool animAttacktToRecord;
    [HideInInspector]
    public Vector3 animVectorToRecord;
    [HideInInspector]
    public Quaternion animRotToRecord;

    private bool right;

    bool m_Grounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainAttackAnim = false;
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
        if (Input.GetMouseButtonDown(0) && !mainAttackAnim)
        {
            mainAttack = true;
            mainAttackAnim = true;
        }

        // set animations
        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x) * animationSpeedMultipler);
        animator.SetBool("jump", rb.velocity.y > .3f);
        animator.SetBool("fall", rb.velocity.y < -.3f);
        animator.SetBool("run", Mathf.Abs(rb.velocity.x) > 2f);
        animator.SetBool("attack", mainAttackAnim);
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

    public void AttackLaunched()
    {
        mainAttackAnim = false;
    }

    void MainAttack()
    {
        Vector3 direction = GetDirection();
        Vector3 attackPoint = direction;
        if (attackPoint.x != 0) attackPoint *= attackDistanceHorizontal;
        if (attackPoint.y != 0) attackPoint *= attackDistanceVertical;
        attackPoint += transform.position;

        animAttacktToRecord = true;
        animVectorToRecord = attackPoint;
        animRotToRecord = Quaternion.Euler(new Vector3(0, 0, direction.x > 0 ? -45 : direction.x < 0 ? 135 : direction.y < 0 ? 225 : 45));

        hitEffect.transform.position = animVectorToRecord;
        hitEffect.transform.rotation = animRotToRecord;
        hitEffect.Play();

        Instantiate(hitPrefab, animVectorToRecord, Quaternion.Euler(0, 0, 0));

        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint, .5f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if (colliders[i].TryGetComponent(out SimpleObject obj))
                {
                    // check pogo jump
                    if (obj.canPogoJump && direction.y < 0)
                    {
                        controller.ForceJump();
                    }
                }
            }
        }
    }
}
