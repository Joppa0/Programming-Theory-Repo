using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PowerUp : MonoBehaviour
{
    private PlayerController player;

    [SerializeField] int powerupID;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Checks which powerup the player has collided with, then calls the appropriate function
            switch (powerupID)
            {
                case 0:
                    player.OnHealthPickupEnter();
                    break;

                case 1:
                    player.OnSpeedBoostEnter();
                    break;

                case 2:
                    player.OnTripleShotEnter();
                    break;

                case 3:
                    player.OnSlowMotionEnter();
                    break;

                case 4:
                    player.OnHeatSeekingEnter();
                    break;
                case 5:
                    player.numOfHearts++;
                    break;
            }
            Destroy(gameObject);
        }
    }
}