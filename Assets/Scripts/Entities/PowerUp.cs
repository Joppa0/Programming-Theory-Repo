using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    protected PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.hasPowerup = true;
            Destroy(gameObject);
            StartCoroutine(PowerupEffect());
        }
    }

    protected virtual IEnumerator PowerupEffect()
    {
        yield return new WaitForSeconds(7);
        player.hasPowerup = false;
    }
}
