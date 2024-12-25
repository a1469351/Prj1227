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
    [SerializeField] private List<PhaseInfo> phaseInfoList;
    [SerializeField] private Transform SelectionRoot;
    [SerializeField] private TowerInfo FirstTower;
    [SerializeField] private Transform CursorFollower;
    public GameObject DestroyEffect;
    public float PositionLimit;
    private List<Enemy> newEnemyList = new List<Enemy>();
    private GameObject tempTower;

    [Header("GameValue")]
    [SerializeField] private float InitialGold;
    [SerializeField] private float GoldPerPhase;
    [SerializeField] private float AdditiveGoldPerPhase;
    public float EnemyStrengthPerPhase;
    public float TowerStrengthPerLevel;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI GoldText;
    [SerializeField] private TextMeshProUGUI phaseText;
    [SerializeField] private DescriptionUI desc;
    [SerializeField] private GameObject gameOverPopup;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject GuidePopup;

    [Header("Info")]
    public float spawnInterval = 2.0f;  // 生成间隔
    public float score;
    public float gold;
    public int phase;
    public bool phaseSpawning = false;
    public bool gamePausing = false;
    public TowerInfo currentSelection;
    public Tower currentSelectionTower;
    void Awake()
    {
        Instance = this;

        gameObject.AddComponent<ResourceManager>();
        gameObject.AddComponent<AudioManager>();
        Init();
    }

    void Init()
    {
        SpwanSelection();

        //StartCoroutine(SpawnMonsters());
        //StartPhase();
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
        if (gamePausing) return;
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

        CheckGameOver();
        CheckInput();
        CheckPhase();
        CheckCursorPos();
    }

    void CheckCursorPos()
    {
        Vector2 pos = GetMousePosition();
        CursorFollower.position = pos;
        bool canBuild = CanBuildTower();
        Color maskColor = canBuild ? Color.yellow : Color.red;
        maskColor.a = 0.8f;
        CursorFollower.Find("Mask").GetComponent<SpriteRenderer>().color = maskColor;
    }

    void CheckGameOver()
    {
        if (phase <= 0) return;
        if (towerList.Count <= 0)
        {
            gameOverPopup.SetActive(true);
            gamePausing = true;
            gameOverScoreText.text = score.ToString();
        }
    }

    public void OnContinueClick()
    {
        float randomX = Random.Range(-5, 0.0f);
        float randomY = Random.Range(-4, 4f);
        BuildTower(towerInfoList[0], new Vector2(randomX, randomY));
        gameOverPopup.SetActive(false);
        gamePausing = false;
    }

    public void OnStartClick()
    {
        startMenu.SetActive(false);
        InitParam();
        if (!GuidePopup.activeInHierarchy)
        {
            StartPhase();
        }
        else
        {
            gamePausing = true;
            SetSelection(towerInfoList[0]);
            CursorFollower.gameObject.SetActive(false);
        }
    }

    public void OnGuideClick()
    {
        GuidePopup.SetActive(false);
        StartPhase();
    }

    void InitParam()
    {
        SetScoreAndGold(0, InitialGold);
        BuildTower(FirstTower, new Vector2(-5.5f, 0));
        phase = 1;
        phaseText.text = phase.ToString();
    }

    public void StartPhase()
    {
        CancelSelection();
        phaseSpawning = true;
        gamePausing = false;
        StartCoroutine("SpawnPhase");
    }

    public void BackToStartMenu()
    {
        startMenu.SetActive(true);
        gameOverPopup.SetActive(false);
        phase = 0;
        ClearAll();
    }

    void CheckPhase()
    {
        if (phase <= 0) return;
        if (phaseInfoList.Count <= 0) return;
        if (enemyList.Count != 0 || newEnemyList.Count != 0) return;
        if (phaseSpawning) return;

        phaseSpawning = true;
        StartCoroutine("SpawnPhase");
        phase++;
        phaseText.text = phase.ToString();
        UpdateGold(GoldPerPhase + phase * AdditiveGoldPerPhase);
    }

    IEnumerator SpawnPhase()
    {
        int randomPhase = Random.Range(0, phaseInfoList.Count);
        PhaseInfo pi = phaseInfoList[randomPhase];
        yield return new WaitForSeconds(1);
        for (int i = 0; i < pi.enemyList.Count; i++)
        {
            for (int j = 0; j < pi.enemyNum[i] + phase - 1; j++)
            {
                EnemyInfo ei = pi.enemyList[i];
                float randomX = Random.Range(4, 7.0f);
                float randomY = Random.Range(-4, 4.0f);

                Vector3 spawnPosition = new Vector3(randomX, randomY, 0);
                CreateNewEnemy(ei, spawnPosition);
                yield return new WaitForSeconds(pi.spawnInterval);
            }
        }
        phaseSpawning = false;
    }

    void AddNewEnemy()
    {
        foreach (Enemy enemy in newEnemyList)
        {
            enemyList.Add(enemy);
        }
        newEnemyList.Clear();
    }

    void ClearAll()
    {
        foreach (Tower tower in towerList)
        {
            Destroy(tower.gameObject);
        }
        foreach (Enemy enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }
        towerList.Clear();
        enemyList.Clear();
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
            UpdateScoreAndGold(enemy.ei.BaseScore, 0);
            Destroy(enemy.gameObject);
        }
    }

    public void CreateNewEnemy(EnemyInfo ei, Vector2 pos)
    {
        float phaseModifier = 1 + phase * EnemyStrengthPerPhase;
        Enemy enemy = Instantiate(ei.EnemyPrefab, new Vector3(5, 3, 0), Quaternion.identity).GetComponent<Enemy>();
        enemy.SetAttack(ei.BaseAttack * phaseModifier);
        enemy.SetAttackRange(ei.AttackRange);
        enemy.SetAttackCooldown(ei.AttackCoolDown);
        enemy.SetHp(ei.BaseHealth * phaseModifier);
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
            if (e.invisible) continue;
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
        GoldText.text = gold.ToString();
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
            if (CanBuildTower())
            {
                BuildTower();
                UpdateGold(-currentSelection.Price);
                CancelSelection();
            }
        }
        if (Input.GetMouseButton(1))
        {
            CancelSelection();
        }
    }

    bool CanBuildTower()
    {
        if (currentSelection == null || IsPointerOnUI()) return false;
        return !IsCursorArroundTower() && gold >= currentSelection.Price;
    }

    bool IsCursorArroundTower()
    {
        Vector2 cursorPos = GetMousePosition();
        Collider2D[] cds = Physics2D.OverlapBoxAll(cursorPos, Vector2.one, 0);
        foreach (Collider2D cd in cds)
        {
            if (cd.gameObject.GetComponent<Tower>() != null) return true;
        }
        return false;
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

    private void BuildTower(TowerInfo ti, Vector2 pos)
    {
        Tower tower = Instantiate(ti.TowerPrefab, pos, Quaternion.identity).GetComponent<Tower>();
        tower.ti = ti;
        tower.SetParam(ti, 1);
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
        if (tempTower != null) Destroy(tempTower);
        CursorFollower.gameObject.SetActive(false);
    }

    public void UpdateSelection(TowerInfo ti)
    {
        desc.UpdateInfoInSelection(ti.NameID, ti.ShootCooldown, ti.BulletDamage, ti.Price, ti.BaseHp, ti.DescriptionID);
        desc.gameObject.SetActive(true);
        if (tempTower != null) Destroy(tempTower);
        tempTower = Instantiate(currentSelection.TowerPrefab, CursorFollower);
        tempTower.GetComponent<BoxCollider2D>().enabled = false;
        CursorFollower.gameObject.SetActive(true);
    }

    public void UpdateSelectionInGame(Tower t)
    {
        if (t.GetCurHp() <= 0)
        {
            CancelSelection();
        }
        TowerInfo ti = t.ti;
        desc.UpdateInfoInGame(ti.NameID, ti.ShootCooldown, t.bulletDamage, t.level, t.GetCurHp(), ti.DescriptionID);
        desc.gameObject.SetActive(true);
        currentSelection = null;
        currentSelectionTower = t;
        if (tempTower != null) Destroy(tempTower);
        CursorFollower.gameObject.SetActive(false);
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
