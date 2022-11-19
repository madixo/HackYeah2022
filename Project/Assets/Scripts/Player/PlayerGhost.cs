using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerGhost : MonoBehaviour
{
    public PlayerController recordPlayer;
    public int timeSpanMilliseconds;
    TimeSpan timeDifference;

    List<AnimState> state;

    private void Awake()
    {
        state = new List<AnimState>();
        timeDifference = new TimeSpan(0, 0, 0, 0, timeSpanMilliseconds);
    }

    private void Update()
    {
        Record();
        Reply();
    }

    void Record()
    {
        state.Add(new AnimState()
        {
            time = DateTime.Now,
            pos = recordPlayer.transform.position
        });
    }

    void Reply()
    {
        if (state.Count == 0)
        {
            Reply0();
            return;
        }
        while (state.Count > 0 && state[0].time < DateTime.Now - timeDifference)
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

    void Reply0()
    {
        transform.position = recordPlayer.transform.position;
    }

    void ReplyState(AnimState state)
    {
        transform.position = state.pos;
    }

    struct AnimState
    {
        public DateTime time;
        public Vector3 pos;
    }
}
