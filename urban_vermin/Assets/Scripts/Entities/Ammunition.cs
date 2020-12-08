using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : Collectible
{
    [SerializeField]
    private int bulletsToPickup;

    public override void OnCollect(Player player)
    {
        player.Ammo += bulletsToPickup;
        Destroy(gameObject);
    }
}
