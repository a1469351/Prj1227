using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTower : Tower
{
    [SerializeField] private float bulletNum;
    [SerializeField] private float rotateAngle;
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
            Vector3 dir = target.transform.position - transform.position;
            NormalBullet go = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
            go.SetParam(bulletSpeed, dir, bulletDamage, this);
            for (int i = 0; i < bulletNum-1; i++)
            {
                int row = Mathf.CeilToInt((i + 1.0f) / 2);
                int adddir = i % 2 == 0 ? 1 : -1;
                Quaternion addrotate = Quaternion.AngleAxis(adddir * row * rotateAngle, Vector3.forward);
                NormalBullet addgo = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                addgo.SetParam(bulletSpeed, addrotate * dir, bulletDamage, this);
            }
        }
        shootTimer = shootCooldown;
    }
}
