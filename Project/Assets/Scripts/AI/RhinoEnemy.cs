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
    private Rigidbody rigidBody;
    [SerializeField]
    private float speed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate() {

        if(target.transform.position.x - transform.position.x < 0) {

            if(movingRight)
                Flip();


        }else {

            if(!movingRight)
                Flip();

        }

        rigidBody.velocity = new Vector2(movingRight ? speed : -speed, rigidBody.velocity.y);

    }

    void Flip() {

        movingRight = !movingRight;
        Vector3 scale = rigidBody.transform.localScale;
        scale.x *= -1;
        rigidBody.transform.localScale = scale;

    }
}
