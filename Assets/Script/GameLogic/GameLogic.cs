using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ns;
using TMPro;
using UnityEngine.EventSystems;

public class GameLogic : MonoBehaviour
{
    public static GameLogic Instance;
    [Header("GamePlay")]
    [SerializeField] private List<Tower> towerList;
    [SerializeField] private List<TowerInfo> towerInfoList;
    [SerializeField] private List<Enemy> enemyList;
    [SerializeField] private List<EnemyInfo> enemyInfoList;
    [SerializeField] private Transform SelectionRoot;
    public float PositionLimit;
    private List<Enemy> newEnemyList = new List<Enemy>();

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI GoldText;
    [SerializeField] private DescriptionUI desc;

    [Header("Info")]
    public float spawnInterval = 2.0f;  // 生成间隔
    public float score;
    public float gold;
    public TowerInfo currentSelection;
    public Tower currentSelectionTower;
    void Awake()
    {
        Instance = this;

        Init();
    }

    void Init()
    {
        CancelSelection();
        SpwanSelection();
        SetScoreAndGold(0, 0);
        StartCoroutine(SpawnMonsters());

        if (towerList.Count > 0)
        {
            towerList[0].SetParam(towerInfoList[0], 1);
            EventTriggerListener.Get(towerList[0].gameObject).onClick = (go) =>
            {
                UpdateSelectionInGame(towerList[0]);
            };
        }
    }

    void SpwanSelection()
    {
        RectTransform rootRect = SelectionRoot.GetComponent<RectTransform>();
        rootRect.sizeDelta = new Vector3(towerInfoList.Count * 100 + 50, rootRect.sizeDelta.y);
        foreach (TowerInfo ti in towerInfoList)
        {
            GameObject go = Instantiate(ti.TowerPreview, SelectionRoot);
            EventTriggerListener.Get(go).onClick = (go) =>
            {
                SetSelection(ti);
            };
        }
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
        AddNewEnemy();

        CheckInput();
    }

    void AddNewEnemy()
    {
        foreach (Enemy enemy in newEnemyList)
        {
            enemyList.Add(enemy);
        }
        newEnemyList.Clear();
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
            UpdateScoreAndGold(enemy.ei.BaseScore, enemy.ei.BaseScore);
            Destroy(enemy.gameObject);
        }
    }

    public void CreateNewEnemy(EnemyInfo ei, Vector2 pos)
    {
        Enemy enemy = Instantiate(ei.EnemyPrefab, new Vector3(5, 3, 0), Quaternion.identity).GetComponent<Enemy>();
        enemy.SetAttack(ei.BaseAttack);
        enemy.SetAttackRange(ei.AttackRange);
        enemy.SetAttackCooldown(ei.AttackCoolDown);
        enemy.SetHp(ei.BaseHealth);
        enemy.SetPosition(pos);
        enemy.SetSpeed(ei.Speed);
        enemy.SetEnemyInfo(ei);
        newEnemyList.Add(enemy);
    }

    public void CreateEnemy(EnemyInfo ei, Vector2 pos)
    {
        Enemy enemy = Instantiate(ei.EnemyPrefab, new Vector3(5, 3, 0), Quaternion.identity).GetComponent<Enemy>();
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
            float randomX = Random.Range(4, 7.0f);
            float randomY = Random.Range(-4, 4.0f);

            Vector3 spawnPosition = new Vector3(randomX, randomY, 0);
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

    public void SetScoreAndGold(float sval, float gval)
    {
        score = sval;
        ScoreText.text = score.ToString();
        gold = gval;
        GoldText.text = score.ToString();
    }

    public void UpdateScoreAndGold(float sval, float gval)
    {
        score += sval;
        ScoreText.text = score.ToString();
        gold += gval;
        GoldText.text = gold.ToString();
    }

    public void UpdateGold(float val)
    {
        gold += val;
        GoldText.text = gold.ToString();
    }

    void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOnUI() && currentSelection != null)
            {
                if (gold >= currentSelection.Price)
                {
                    BuildTower();
                    UpdateGold(-currentSelection.Price);
                    CancelSelection();
                }
            }
        }
        if (Input.GetMouseButton(1))
        {
            CancelSelection();
        }
    }

    private void BuildTower()
    {
        Vector2 pos = GetMousePosition();
        Tower tower = Instantiate(currentSelection.TowerPrefab, pos, Quaternion.identity).GetComponent<Tower>();
        tower.ti = currentSelection;
        tower.SetParam(currentSelection, 1);
        towerList.Add(tower);
        EventTriggerListener.Get(tower.gameObject).onClick = (go) =>
        {
            UpdateSelectionInGame(tower);
        };
    }

    public Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void SetSelection(TowerInfo ti)
    {
        currentSelection = ti;
        currentSelectionTower = null;
        UpdateSelection(ti);
    }

    public void CancelSelection()
    {
        currentSelection = null;
        currentSelectionTower = null;
        desc.gameObject.SetActive(false);
    }

    public void UpdateSelection(TowerInfo ti)
    {
        desc.UpdateInfoInSelection(ti.NameID, ti.ShootCooldown, ti.BulletDamage, ti.Price, ti.BaseHp, ti.DescriptionID);
        desc.gameObject.SetActive(true);
    }

    public void UpdateSelectionInGame(Tower t)
    {
        TowerInfo ti = t.ti;
        desc.UpdateInfoInGame(ti.NameID, ti.ShootCooldown, ti.BulletDamage, t.level, t.GetCurHp(), ti.DescriptionID);
        desc.gameObject.SetActive(true);
        currentSelection = null;
        currentSelectionTower = t;
    }

    public void UpdateTowerInGame(Tower t)
    {
        if (currentSelectionTower == t)
        {
            UpdateSelectionInGame(t);
        }
    }

    public bool IsPointerOnUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else return false;
    }
}
