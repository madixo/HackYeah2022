using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerGhost : MonoBehaviour
{
    public PlayerController recordPlayer;
    public int timeSpanMilliseconds;
    TimeSpan timeDifference;
    Animator playerAnimator;
    Animator animator;

    List<AnimState> state;

    private void Awake()
    {
        state = new List<AnimState>();
        timeDifference = new TimeSpan(0, 0, 0, 0, timeSpanMilliseconds);
        animator = GetComponent<Animator>();
        playerAnimator = recordPlayer.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Record();
        Reply();
    }

    void Record()
    {
        state.Add(new AnimState()
        {
            time = DateTime.Now,
            pos = recordPlayer.transform.position,
            jump = playerAnimator.GetBool("jump"),
            fall = playerAnimator.GetBool("fall"),
            run = playerAnimator.GetBool("run"),
            attack = playerAnimator.GetBool("attack"),
            animSpeed = playerAnimator.GetFloat("speed"),
            localScale = recordPlayer.transform.localScale,
            spawnAnim = recordPlayer.animAttacktToRecord,
            spawnAnimPosition = recordPlayer.animVectorToRecord,
            spawnAnimRotation = recordPlayer.animRotToRecord,
            action = recordPlayer.animAttacktToRecord,
        });
        recordPlayer.animAttacktToRecord = false;
    }

    void Reply0()
    {
        transform.position = recordPlayer.transform.position;
        animator.SetBool("jump", false);
        animator.SetBool("fall", false);
        animator.SetBool("run", false);
        animator.SetBool("attack", false);
        animator.SetFloat("speed", 1);
        // transform.localScale = state.localScale;
    }

    void ReplyState(AnimState state)
    {
        transform.position = state.pos;
        animator.SetBool("jump", state.jump);
        animator.SetBool("fall", state.fall);
        animator.SetBool("run", state.run);
        animator.SetBool("attack", state.attack);
        animator.SetFloat("speed", state.animSpeed);
        transform.localScale = state.localScale;

        if (state.action)
        {
            if (state.spawnAnim)
            {
                recordPlayer.hitEffect.transform.position = state.spawnAnimPosition;
                recordPlayer.hitEffect.transform.rotation = state.spawnAnimRotation;
                recordPlayer.hitEffect.Play();
                Instantiate(recordPlayer.hitPrefab, state.spawnAnimPosition, Quaternion.Euler(0, 0, 0));
            }
            state.action = false;
            this.state[0] = state;
        }
    }

    void Reply()
    {
        if (state.Count == 0)
        {
            Reply0();
            return;
        }
        while (state.Count > 0 && state[0].time < DateTime.Now - timeDifference && !state[0].action)
        {
            state.RemoveAt(0);
        }
        if (state.Count == 0)
        {
            Reply0();
            return;
        }
        ReplyState(state[0]);
    }

    struct AnimState
    {
        public DateTime time;
        public Vector3 pos;
        public Vector3 localScale;
        public bool jump;
        public bool fall;
        public bool run;
        public bool attack;
        public bool spawnAnim;
        public Vector3 spawnAnimPosition;
        public Quaternion spawnAnimRotation;
        public float animSpeed;
        public bool action;
    }

    public void AttackLaunched() { }
}
