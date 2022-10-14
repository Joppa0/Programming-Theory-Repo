using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Brute : EnemyController
{
    protected override void Start()
    {
        //Overrides the start function of EnemyController to change the life and point values
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        nav.avoidancePriority = Random.Range(0, 100);

        pointValue *= 2;
        life = 2;
        damage = 2;
    }
}
