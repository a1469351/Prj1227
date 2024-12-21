using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Enemy")]
public class EnemyInfo : ScriptableObject
{
    public string EnemyName;
    public float BaseScore;
    public float BaseHealth;
    public float BaseAttack;
    public float AttackRange;
    public float AttackCoolDown;
    public float Speed;
    public GameObject EnemyPrefab;
}
