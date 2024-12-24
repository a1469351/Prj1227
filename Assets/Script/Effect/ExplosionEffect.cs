using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;

    // Update is called once per frame
    void Update()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - 0.01f);
        if (sr.color.a <= 0) Destroy(gameObject);
    }
}
