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
            if (Vector2.Distance(target.transform.position, transform.position) > attackRange)
            {
                Vector2 dir = target.transform.position - transform.position;
                transform.Translate(dir.normalized * Time.deltaTime * speed);
            }
            else
            {
                if (CanAttack())
                {
                    Debug.Log("Attack");
                    attackTimer = attackCooldown;
                }
            }
        }
        else
        {
            transform.Translate(-Vector2.one * Time.deltaTime);
        }
    }
}
