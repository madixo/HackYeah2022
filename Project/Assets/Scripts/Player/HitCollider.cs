using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitCollider : MonoBehaviour
{
    DateTime start;
    public int durationMiliseconds;

    void Start()
    {
        start = DateTime.Now;
    }

    void Update()
    {
        if (DateTime.Now - start > new TimeSpan(0, 0, 0, 0, durationMiliseconds) && gameObject)
        {
            Destroy(gameObject);
        }
    }
}
