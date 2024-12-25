using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : NormalBullet
{
    private void Awake()
    {
        Destroy(gameObject, 0.5f);
    }
}
