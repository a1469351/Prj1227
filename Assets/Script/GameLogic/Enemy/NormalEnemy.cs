using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        MoveAndAttack();
    }

    public void MoveAndAttack()
    {
        Tower target = GameLogic.Instance.GetNearestTower(transform.position);
        if (target != null)
        {
            LookAt(target.transform.position);
            if (Vector2.Distance(target.transform.position, transform.position) > attackRange)
            {
                Vector3 dir = target.transform.position - transform.position;
                Vector3 pos = transform.position + (dir.normalized * Time.deltaTime * speed);
                transform.position = pos;
            }
            else
            {
                if (CanAttack())
                {
                    target.DoDamage(attack);
                    attackTimer = attackCooldown;
                }
            }
        }
        else
        {
            Vector3 pos = transform.position + (-Vector3.right * Time.deltaTime);
            //transform.position = pos;
        }
    }
}
