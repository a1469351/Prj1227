using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    public float speed;
    public Vector2 dir;
    public float damage;
    public Tower tower;
    private bool active = false;

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            transform.Translate(dir * speed * Time.deltaTime);
        }

        if (Mathf.Abs(transform.position.x) > GameLogic.Instance.PositionLimit 
            || Mathf.Abs(transform.position.y) > GameLogic.Instance.PositionLimit)
        {
            Destroy(gameObject);
        }
    }

    public void SetParam(float _speed, Vector2 _dir, float _damage, Tower _tower)
    {
        speed = _speed;
        dir = _dir.normalized;
        damage = _damage;
        tower = _tower;
        active = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            bool kill = enemy.DoDamage(damage);
            if (kill)
            {
                tower.GetExp(enemy.ei.BaseScore);
            }
            Destroy(gameObject);
        }
    }
}
