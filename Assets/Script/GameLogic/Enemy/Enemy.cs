using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] protected float attackTimer = 0f;
	[SerializeField] protected float attackCooldown = 1f;
	protected float maxHp;
	protected float curHp;
	protected float attack;
	protected float attackRange;
	protected float speed;
	public EnemyInfo ei;
	protected Transform hptrans;
	public bool invisible = false;
	virtual public void Awake()
	{
		hptrans = transform.Find("Mask");
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

	virtual public bool DoDamage(float dmg)
    {
		if (curHp <= 0) return false;
		curHp -= dmg;
		UpdateVisual();
		if (curHp <= 0)
		{
			OnDead();
			return true;
		}
		else return false;
    }

	virtual public void UpdateVisual()
	{
		if (hptrans == null) return;
		float ratio = curHp / maxHp;
		hptrans.localScale = new Vector3(ratio, hptrans.localScale.y, hptrans.localScale.z);
	}

	virtual public float GetCurHp()
	{
		return curHp;
	}

	virtual public void SetHp(float hp)
    {
		curHp = hp;
		maxHp = hp;
		UpdateVisual();
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

	virtual public void LookAt(Vector3 pos)
	{
		Vector2 dir = pos - transform.position;
		transform.rotation = Quaternion.Euler(0, 0, -Vector2.SignedAngle(dir, Vector2.right));
	}

	virtual public void OnDead()
    {

    }
}
