using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTower : Tower
{
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        Shoot();
    }

    public override void Shoot()
    {
        if (!CanShoot()) return;
        base.Shoot();
        if (bullet == null)
        {
            Debug.Log("Null Bullet Detected.");
            return;
        }
        Enemy target = GameLogic.Instance.GetNearestEnemy(transform.position);
        if (target != null)
        {
            NormalBullet go = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
            go.SetParam(bulletSpeed, target.transform.position - transform.position, bulletDamage, this);
        }
        shootTimer = shootCooldown;
    }
}
