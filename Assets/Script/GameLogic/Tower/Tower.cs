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
	private float curHp;
	virtual public void Awake()
	{
		curHp = 100;
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
}