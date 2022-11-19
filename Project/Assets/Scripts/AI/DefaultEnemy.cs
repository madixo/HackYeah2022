using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemy : MonoBehaviour
{
    [SerializeField]
    private Transform leftFoot;
    [SerializeField]
    private Transform rightFoot;
    [SerializeField]
    private float radius;
    [SerializeField]
    private LayerMask whatIsGround;
    private bool onEdge = false;
    private bool lastState;
    private bool movingRight = true;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private float speed;
    private bool canFlip = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


    }

    void Flip() {

        movingRight = !movingRight;
        // Vector3 scale = transform.localScale;
        // scale.x *= -1;
        // transform.localScale = scale;

    }

    void FixedUpdate() {

        onEdge = false;

        Collider2D leftCollider = Physics2D.OverlapCircle(leftFoot.position, radius, whatIsGround);
        Collider2D rightCollider = Physics2D.OverlapCircle(rightFoot.position, radius, whatIsGround);

        if(leftCollider != rightCollider) {
            onEdge = true;
            if(canFlip) {
                Flip();
                canFlip = false;
            }
        }else {
            if(!canFlip)
                canFlip = true;
        }

        rigidBody.velocity = new Vector2(movingRight ? speed : -speed, rigidBody.velocity.y);

    }
}
