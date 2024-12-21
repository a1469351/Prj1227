using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ns;
using TMPro;

public class GameLogic : MonoBehaviour
{
    public static GameLogic Instance;
    [SerializeField] private List<Tower> towerList;
    [SerializeField] private List<Enemy> enemyList;
    [SerializeField] private List<EnemyInfo> enemyInfoList;
    [SerializeField] private TextMeshProUGUI ScoreText;
    public float spawnInterval = 2.0f;  // 生成间隔
    public float score;
    void Awake()
    {
        Instance = this;

        SetScore(0);
        StartCoroutine(SpawnMonsters());
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Tower tower in towerList)
        {
            if (tower.GetCurHp() > 0)
            {
                tower.UpdateLogic();
            }
        }
        foreach (Enemy enemy in enemyList)
        {
            if (enemy.GetCurHp() > 0)
            {
                enemy.UpdateLogic();
            }
        }
        ClearDead();
    }

    void ClearDead()
    {
        List<Tower> clearTowerList = new List<Tower>();
        foreach (Tower tower in towerList)
        {
            if (tower.GetCurHp() <= 0)
            {
                clearTowerList.Add(tower);
            }
        }
        foreach (Tower tower in clearTowerList)
        {
            towerList.Remove(tower);
            Destroy(tower.gameObject);
        }

        List<Enemy> clearEnemyList = new List<Enemy>();
        foreach (Enemy enemy in enemyList)
        {
            if (enemy.GetCurHp() <= 0)
            {
                clearEnemyList.Add(enemy);
            }
        }
        foreach (Enemy enemy in clearEnemyList)
        {
            enemyList.Remove(enemy);
            UpdateScore(enemy.ei.BaseScore);
            Destroy(enemy.gameObject);
        }
    }

    void CreateEnemy(EnemyInfo ei, Vector2 pos)
    {
        NormalEnemy enemy = Instantiate(ei.EnemyPrefab, new Vector3(5, 3, 0), Quaternion.identity).GetComponent<NormalEnemy>();
        enemy.SetAttack(ei.BaseAttack);
        enemy.SetAttackRange(ei.AttackRange);
        enemy.SetAttackCooldown(ei.AttackCoolDown);
        enemy.SetHp(ei.BaseHealth);
        enemy.SetPosition(pos);
        enemy.SetSpeed(ei.Speed);
        enemy.SetEnemyInfo(ei);
        enemyList.Add(enemy);
    }

    IEnumerator SpawnMonsters()
    {
        while (true)
        {
            float randomInterval = Random.Range(1f, 3f); // 1 到 3 秒之间的随机间隔
            yield return new WaitForSeconds(spawnInterval);

            // 伪随机生成怪物
            int randomIndex = Random.Range(0, enemyInfoList.Count);
            EnemyInfo ei = enemyInfoList[randomIndex];
            float randomY = Random.Range(-4, 4);

            Vector3 spawnPosition = new Vector3(4, randomY, 0);
            CreateEnemy(ei, spawnPosition);
        }
    }

    public Enemy GetNearestEnemy(Vector2 pos)
    {
        Enemy enemy = null;
        float dis = 0;
        foreach (Enemy e in enemyList)
        {
            if (enemy == null)
            {
                enemy = e;
                dis = Vector2.Distance(pos, enemy.transform.position);
            }
            else
            {
                float eDis = Vector2.Distance(pos, e.transform.position);
                if (eDis < dis)
                {
                    enemy = e;
                    dis = eDis;
                }
            }
        }
        return enemy;
    }

    public Tower GetNearestTower(Vector2 pos)
    {
        Tower tower = null;
        float dis = 0;
        foreach (Tower t in towerList)
        {
            if (tower == null)
            {
                tower = t;
                dis = Vector2.Distance(pos, tower.transform.position);
            }
            else
            {
                float eDis = Vector2.Distance(pos, t.transform.position);
                if (eDis < dis)
                {
                    tower = t;
                    dis = eDis;
                }
            }
        }
        return tower;
    }

    public void SetScore(float val)
    {
        score = val;
        ScoreText.text = score.ToString();
    }

    public void UpdateScore(float val)
    {
        score += val;
        ScoreText.text = score.ToString();
    }
}
