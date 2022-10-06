using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : PowerUp
{
    protected override IEnumerator PowerupEffect()
    {
        yield return new WaitForSeconds(0);
        player.life++;
        player.hasPowerup = false;
    }
}
