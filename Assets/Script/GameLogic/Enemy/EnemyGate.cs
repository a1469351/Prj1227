using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGate : Enemy
{
    [SerializeField] private int InstantiateNum;
    [SerializeField] EnemyInfo InstantiateInfo;
    [SerializeField] private float spawnInterval;
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (CanAttack())
        {
            Spwan();
            attackTimer = attackCooldown;
        }
    }

    public void Spwan()
    {
        StartCoroutine("CreateSomeEnemy");
    }

    IEnumerator CreateSomeEnemy()
    {
        for (int i = 0; i < InstantiateNum; i++)
        {
            GameLogic.Instance.CreateNewEnemy(InstantiateInfo, transform.position);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public override void OnDead()
    {
        base.OnDead();
        StopCoroutine("CreateSomeEnemy");
    }

    public override void UpdateVisual()
    {
        if (hptrans == null) return;
        float ratio = curHp / maxHp;
        hptrans.localScale = new Vector3(ratio, ratio, hptrans.localScale.z);
    }
}
