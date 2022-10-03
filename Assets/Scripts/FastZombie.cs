using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastZombie : EnemyController
{
    protected override void Move()
    {
        float speed = 3.0f;

        Vector3 lookdirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookdirection * speed);
    }
}
