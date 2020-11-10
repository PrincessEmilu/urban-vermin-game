using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingEntity : MonoBehaviour
{
    public float damage;
    public float knockBack;

    void Start()
    {
        damage = 10.0f;
        knockBack = 5.0f;
    }
}
