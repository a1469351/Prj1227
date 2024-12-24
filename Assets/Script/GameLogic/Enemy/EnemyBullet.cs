using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : NormalEnemy
{

    override public void UpdateLogic()
    {
        base.UpdateLogic();
        bool attack = BulletMoveAndAttack();
        if (attack)
        {
            DoDamage(maxHp);
        }
    }
    public bool BulletMoveAndAttack()
    {
        Tower target = GameLogic.Instance.GetNearestTower(transform.position);
        if (target != null)
        {
            LookAt(target.transform.position);
            Vector3 dir = target.transform.position - transform.position;
            Vector3 pos = transform.position + (dir.normalized * Time.deltaTime * speed);
            transform.position = pos;
            if (Vector2.Distance(target.transform.position, transform.position) > attackRange)
            {
                
            }
            else
            {
                if (CanAttack())
                {
                    target.DoDamage(attack);
                    attackTimer = attackCooldown;
                    return true;
                }
            }
        }
        else
        {
            Vector3 pos = transform.position + (-Vector3.right * Time.deltaTime);
            //transform.position = pos;
        }
        return false;
    }
}
