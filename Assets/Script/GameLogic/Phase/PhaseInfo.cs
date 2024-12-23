using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Phase")]
public class PhaseInfo : ScriptableObject
{
    public float spawnInterval;
    public List<EnemyInfo> enemyList;
    public List<int> enemyNum;
}
