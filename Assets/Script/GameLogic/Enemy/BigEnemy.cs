using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemy : Enemy
{
    [SerializeField] private int InstantiateNum;
    [SerializeField] EnemyInfo InstantiateInfo;
    [SerializeField] float distance;
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

    override public void OnDead()
    {
        for (int i = 0; i < InstantiateNum; i++)
        {
            int row = Mathf.CeilToInt((i+1.0f)/2);
            int dir = i % 2 == 0 ? 1 : -1;
            Vector3 pos = transform.position + dir * row * distance * Vector3.up;
            GameLogic.Instance.CreateNewEnemy(InstantiateInfo, pos);
        }
    }
}
