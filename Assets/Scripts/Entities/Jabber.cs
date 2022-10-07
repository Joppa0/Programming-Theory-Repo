using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INHERITANCE
public class Jabber : EnemyController
{
    //POLYMORPHISM
    protected override void Move()
    {
        if (!mainManager.m_GameOver)
        {
            //Overrides the Move method from it's parent to change the speed

            nav.speed = 4.0f;

            nav.SetDestination(player.transform.position);
        }
    }
}
