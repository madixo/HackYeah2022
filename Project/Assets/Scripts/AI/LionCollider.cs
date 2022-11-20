using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionCollider : MonoBehaviour
{
    public LionEnemy enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemy.TriggerEnter2D(collision);
    }
}
