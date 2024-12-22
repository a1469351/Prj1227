using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class Tower : MonoBehaviour
{
	[SerializeField] protected float shootTimer = 0f;
	[SerializeField] protected float shootCooldown = 0.5f;
	[SerializeField] protected float bulletSpeed = 5;
	[SerializeField] protected float bulletDamage = 10;
	[SerializeField] protected GameObject bullet;
	public TowerInfo ti;
	public int level;
	public float curHp;
	private float maxHp;
	private float curExp;
	private Transform hptrans;
	virtual public void Awake()
	{
		hptrans = transform.Find("Mask");
	}

	virtual public void UpdateLogic()
    {
		if (shootTimer > 0) shootTimer -= Time.deltaTime;
    }

	virtual public bool CanShoot()
	{
		if (shootTimer > 0) return false;
		return true;
	}

	virtual public void Shoot()
    {
		
    }

	virtual public float GetCurHp()
    {
		return curHp;
    }

	virtual public void GetExp(float exp)
    {
		curExp += exp;
		level = Mathf.FloorToInt(curExp / 100);
    }

	virtual public void SetParam(TowerInfo _ti, int lvl)
	{
		shootCooldown = _ti.ShootCooldown;
		bulletSpeed = _ti.BulletSpeed;
		bulletDamage = _ti.BulletDamage;
		curHp = _ti.BaseHp;
		maxHp = _ti.BaseHp;
		level = lvl;
		ti = _ti;
		UpdateVisual();
	}

	virtual public void DoDamage(float val)
    {
		if (curHp <= 0) return;
		curHp -= val;
		UpdateVisual();
	}

	virtual public void UpdateVisual()
    {
		if (hptrans == null) return;
		float ratio = curHp / maxHp;
		hptrans.localScale = new Vector3(hptrans.localScale.x, ratio, hptrans.localScale.z);
		hptrans.localPosition = new Vector3(hptrans.localPosition.x, -(1 - ratio) / 2, hptrans.localPosition.z);
		GameLogic.Instance.UpdateTowerInGame(this);

	}
}