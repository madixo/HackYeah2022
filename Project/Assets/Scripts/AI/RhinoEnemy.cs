using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoEnemy : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private bool movingRight;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform probe;
    [SerializeField]
    private float radius;
    [SerializeField]
    private LayerMask whatIsGround;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate() {

        if(target.position.x - rigidBody.transform.position.x < 0) {

            if(movingRight)
                Flip();


        }else {

            if(!movingRight)
                Flip();

        }

        Collider2D collider = Physics2D.OverlapCircle(probe.position, radius, whatIsGround);

        if(collider != null)
            rigidBody.velocity = new Vector2(movingRight ? speed : -speed, rigidBody.velocity.y);
        else
            rigidBody.velocity = Vector2.zero;


    }

    void Flip() {

        movingRight = !movingRight;
        Vector3 scale = rigidBody.transform.localScale;
        scale.x *= -1;
        rigidBody.transform.localScale = scale;

    }
}
