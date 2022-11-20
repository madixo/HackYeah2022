using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giraffe : MonoBehaviour
{
    public float speed;
    Animator animator;

    int state;
    int counter = 0;

    bool mUp = false;
    bool mDown = false;

    Vector3 posTo;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        state = 0;
        posTo = transform.position;
        animator.SetBool("attack", true);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, posTo, Time.deltaTime * speed);
        if (mDown && transform.position.y - posTo.y < 0.2f)
        {
            mDown = false;
            mUp = true;
            state++;
            state %= 2;
            switch (state)
            {
                case 1:
                    transform.position = new Vector3(-13f, transform.position.y);
                    posTo = transform.position + new Vector3(0, 9, 0);
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
                    break;
                case 0:
                    transform.position = new Vector3(6.7f, transform.position.y);
                    posTo = transform.position + new Vector3(0, 9, 0);
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
                    break;
            }
        }
        if (mUp && posTo.y - transform.position.y < 0.2f)
        {
            mUp = false;
            animator.SetBool("attack", true);
            animator.SetBool("idle", false);
        }
    }

    public void EndIdle()
    {
        if (!mDown && !mUp)
        {
            counter++;
            if (counter == 2)
            {
                posTo = new Vector3(transform.position.x, -9);
                mDown = true;
                counter = 0;
            }
        }
    }

    public void EndAttack()
    {
        animator.SetBool("attack", false);
        animator.SetBool("idle", true);
        counter = 0;
    }
}
