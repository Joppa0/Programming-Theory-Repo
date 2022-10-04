using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INHERITANCE
public class FastZombie : EnemyController
{
    //POLYMORPHISM
    protected override void Move()
    {
        if (!mainManager.m_GameOver)
        {
            //Overrides the Move method from it's parent to change the speed
            float speed = 3.0f;

            Vector3 lookdirection = (player.transform.position - transform.position).normalized;

            float rotation = Mathf.Atan2(lookdirection.x, lookdirection.z) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, rotation, 0);

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
