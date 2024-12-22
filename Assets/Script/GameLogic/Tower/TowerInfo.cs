using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Tower")]
public class TowerInfo : ScriptableObject
{
	public int NameID;
	public float BaseHp;
	public float ShootCooldown;
	public float BulletSpeed;
	public float BulletDamage;
	public float Price;
	public int DescriptionID;
	public GameObject TowerPrefab;
	public GameObject TowerPreview;
}
