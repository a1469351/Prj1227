using UnityEngine;

public class StealthMonster : NormalEnemy
{
    public float stealthDuration = 5f;  // 隐身持续时间
    private float timeSinceStealthStarted = 0f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.2f);  // 初始隐身
            invisible = true;
        }
    }

    override public void UpdateLogic()
    {
        base.UpdateLogic();  // 调用父类的 Update 方法

        // 处理隐身逻辑
        timeSinceStealthStarted += Time.deltaTime;
        if (timeSinceStealthStarted >= stealthDuration)
        {
            Reveal();
        }
    }

    // 显形
    private void Reveal()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);  // 显形
            invisible = false;
        }
    }
}