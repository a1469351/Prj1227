using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    public float speed;
    public Vector2 dir;
    public float damage;
    private bool active = false;

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            transform.Translate(dir * speed * Time.deltaTime);
        }

        if (transform.position.x > 100 || transform.position.y > 100)
        {
            Destroy(gameObject);
        }
    }

    public void SetParam(float _speed, Vector2 _dir, float _damage)
    {
        speed = _speed;
        dir = _dir.normalized;
        damage = _damage;
        active = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.DoDamage(damage);
            Destroy(gameObject);
        }
    }
}
