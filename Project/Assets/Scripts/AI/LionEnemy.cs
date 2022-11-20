using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionEnemy : MonoBehaviour
{
    [SerializeField]
    private Transform probe;
    [SerializeField]
    private float radius;
    [SerializeField]
    private LayerMask whatIsGround;
    private bool lastState;
    [SerializeField]
    private bool movingRight = false;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private float speed;
    // private bool canFlip = false;

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
        Vector3 scale = rigidBody.transform.localScale;
        scale.x *= -1;
        rigidBody.transform.localScale = scale;

    }

    void FixedUpdate() {

        Collider2D leftCollider = Physics2D.OverlapCircle(probe.position, radius, whatIsGround);

        if(leftCollider == null) //{
            // if(canFlip) {
                Flip();
                // canFlip = false;
            // }
        // }else {
            // if(!canFlip)
                // canFlip = true;
        // }

        rigidBody.velocity = new Vector2(movingRight ? speed : -speed, rigidBody.velocity.y);

    }
}
