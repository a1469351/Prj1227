using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Tower")]
public class TowerInfo : ScriptableObject
{
	public float ShootCooldown;
	public float BulletSpeed;
	public float BulletDamage;
	public GameObject TowerPrefab;
	public GameObject TowerPreview;
}
