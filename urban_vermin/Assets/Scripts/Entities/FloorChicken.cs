using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorChicken : Collectible
{
    [SerializeField]
    private int healthIncrease;

    public override void OnCollect(Player player)
    {
        player.health += healthIncrease;
        Destroy(gameObject);
    }
}
