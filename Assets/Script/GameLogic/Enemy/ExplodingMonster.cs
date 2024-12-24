using UnityEngine;

public class ExplodingMonster : NormalEnemy
{
    // 自爆相关属性
    public float explosionRadius = 2f;  // 自爆半径
    public int explosionDamage = 50;    // 自爆伤害
    public GameObject explosionEffectPrefab;  // 自爆效果预制体

    // 重写 OnDead 方法，添加自爆逻辑
    override public void OnDead()
    {
        // 调用自爆方法
        Explode();
    }

    // 自爆方法
    private void Explode()
    {
        // 播放自爆效果
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // 检测范围内所有碰撞体
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        // 对每个碰撞体造成伤害
        foreach (Collider2D collider in hitColliders)
        {
            // 确保不对自己造成伤害
            if (collider.gameObject != gameObject)
            {
                // 检查碰撞体是否属于可攻击的目标（例如塔或核心）
                Tower damageable = collider.GetComponent<Tower>();
                if (damageable != null)
                {
                    damageable.DoDamage(explosionDamage);
                }
            }
        }
    }

    //绘制自爆半径（仅用于编辑器调试）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}