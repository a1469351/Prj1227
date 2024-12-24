using UnityEngine;

public class RangedMonster : Enemy
{
    public EnemyInfo projectileInfo;  // 投射物预制体
    public Transform firePoint;  // 投射点

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        
        MoveAndAttack();
    }

    // 发射投射物
    private void Attack()
    {
        // 定期发射投射物
        GameLogic.Instance.CreateNewEnemy(projectileInfo, firePoint.position);
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
            if (CanAttack())
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
        else
        {
            Vector3 pos = transform.position + (-Vector3.right * Time.deltaTime);
            //transform.position = pos;
        }
    }
}