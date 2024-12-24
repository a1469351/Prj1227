using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower
{
    
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        Shoot();
        //LevelUp();
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
            Vector3 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0 , angle);
            LaserBullet go = Instantiate(bullet, transform.position, rotation).GetComponent<LaserBullet>();
            go.SetParam(bulletSpeed, dir, bulletDamage, this);
        }
        shootTimer = shootCooldown;
    }
}