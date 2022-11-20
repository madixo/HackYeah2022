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
    public HealthBar uiBar;
    public Image damageEffect;

    public float velocityMultipler;
    public float attackDistanceHorizontal;
    public float attackDistanceVertical;
    public float animationSpeedMultipler;

    Rigidbody2D rb;
    private float hp;

    private float horizontalMovement;
    private bool jump;
    private bool mainAttack;
    bool mainAttackAnim;
    private bool secondAttack;

    [HideInInspector]
    public bool animAttacktToRecord;
    [HideInInspector]
    public Vector3 animVectorToRecord;
    [HideInInspector]
    public Quaternion animRotToRecord;
    [HideInInspector]
    public bool animAttackIsWhite;

    private bool right;

    bool m_Grounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainAttackAnim = false;
        hp = 1;
    }

    void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * velocityMultipler;
        if (horizontalMovement > 0) right = true;
        if (horizontalMovement < 0) right = false;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump = true;
        }
        if (Input.GetMouseButtonDown(0) && !mainAttackAnim)
        {
            mainAttack = true;
            mainAttackAnim = true;
        }
        if (Input.GetMouseButtonDown(1) && !mainAttackAnim)
        {
            secondAttack = true;
            mainAttackAnim = true;
        }

        // set animations
        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x) * animationSpeedMultipler);
        animator.SetBool("jump", rb.velocity.y > .3f);
        animator.SetBool("fall", rb.velocity.y < -.3f);
        animator.SetBool("run", Mathf.Abs(rb.velocity.x) > 2f);
        animator.SetBool("attack", mainAttackAnim);

        uiBar.SetHealth(hp);

        CanvasGroup cg = damageEffect.GetComponent<CanvasGroup>();
        if (cg.alpha > 0)
        {
            cg.alpha = Mathf.MoveTowards(cg.alpha, 0, Time.deltaTime * 1);
        }
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
        if (secondAttack)
        {
            WhiteAttack();
            secondAttack = false;
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

    void WhiteAttack()
    {
        Vector3 direction = GetDirection();
        Vector3 attackPoint = direction;
        if (attackPoint.x != 0) attackPoint *= attackDistanceHorizontal;
        if (attackPoint.y != 0) attackPoint *= attackDistanceVertical;
        attackPoint += transform.position;

        animAttacktToRecord = true;
        animVectorToRecord = attackPoint;
        animRotToRecord = Quaternion.Euler(new Vector3(0, 0, direction.x > 0 ? 0 : direction.x < 0 ? 180 : direction.y < 0 ? 270 : 90));
        animAttackIsWhite = true;

        HitCollider hit = Instantiate(hitPrefab, animVectorToRecord, animRotToRecord);
        hit.GetComponent<SpriteRenderer>().color = Color.white;
        SimpleObject obj = hit.GetComponent<SimpleObject>();
        obj.canHeal = true;
        obj.canHurt = false;

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

    void MainAttack()
    {
        Vector3 direction = GetDirection();
        Vector3 attackPoint = direction;
        if (attackPoint.x != 0) attackPoint *= attackDistanceHorizontal;
        if (attackPoint.y != 0) attackPoint *= attackDistanceVertical;
        attackPoint += transform.position;

        animAttacktToRecord = true;
        animVectorToRecord = attackPoint;
        animRotToRecord = Quaternion.Euler(new Vector3(0, 0, direction.x > 0 ? 0 : direction.x < 0 ? 180 : direction.y < 0 ? 270 : 90));
        animAttackIsWhite = false;

        //hitEffect.transform.position = animVectorToRecord;
        //hitEffect.transform.rotation = animRotToRecord;
        //hitEffect.Play();

        HitCollider hit = Instantiate(hitPrefab, animVectorToRecord, animRotToRecord);
        hit.GetComponent<SpriteRenderer>().color = Color.black;
        SimpleObject obj = hit.GetComponent<SimpleObject>();
        obj.canHeal = false;
        obj.canHurt = true;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out SimpleObject obj))
        {
            if (obj.canHurt) Hurt();
            if (obj.canHeal) Heal();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out SimpleObject obj))
        {
            if (obj.canHurt) Hurt();
            if (obj.canHeal) Heal();
        }
    }

    void Hurt()
    {
        hp -= 0.2f;
        damageEffect.color = Color.red;
        damageEffect.GetComponent<CanvasGroup>().alpha = 1;
    }

    void Heal()
    {
        hp += 0.2f;
        damageEffect.color = Color.green;
        damageEffect.GetComponent<CanvasGroup>().alpha = 1;
    }
}
