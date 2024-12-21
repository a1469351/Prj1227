using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] protected float attackTimer = 0f;
	[SerializeField] protected float attackCooldown = 1f;
	protected float curHp;
	protected float attack;
	protected float attackRange;
	protected float speed;
	public EnemyInfo ei;
	virtual public void Awake()
	{
		
	}

	virtual public void UpdateLogic()
	{
		attackTimer -= Time.deltaTime;
	}

	virtual public bool CanAttack()
	{
		if (attackTimer > 0) return false;
		return true;
	}

	virtual public void DoDamage(float dmg)
    {
		curHp -= dmg;
		Debug.Log("CurHp : " + curHp);
    }

	virtual public float GetCurHp()
	{
		return curHp;
	}

	virtual public void SetHp(float hp)
    {
		curHp = hp;
    }

	virtual public void SetAttack(float att)
	{
		attack = att;
	}

	virtual public void SetAttackRange(float attRange)
	{
		attackRange = attRange;
	}

	virtual public void SetAttackCooldown(float cd)
	{
		attackCooldown = cd;
	}

	virtual public void SetPosition(Vector2 pos)
	{
		transform.position = pos;
	}

	virtual public void SetSpeed(float _speed)
	{
		speed = _speed;
	}

	virtual public void SetEnemyInfo(EnemyInfo _ei)
	{
		ei = _ei;
	}
}
