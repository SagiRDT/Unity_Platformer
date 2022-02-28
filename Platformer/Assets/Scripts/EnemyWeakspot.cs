using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeakspot : MonoBehaviour
{
    // Config
    [SerializeField] AudioClip bounceSFX = null;

    // Cached component references
    private PolygonCollider2D myBodyCollider2D;
    private Enemy enemy;

    private void Start()
    {
        myBodyCollider2D = GetComponent<PolygonCollider2D>();
        enemy = GetComponentInParent<Enemy>();
    }

    // Handle the enemy weak spot collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isTouchingAPlayer = myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Player"));

        if (isTouchingAPlayer)
        {
            enemy.TakeDamage();
            AudioSource.PlayClipAtPoint(bounceSFX, Camera.main.transform.position);
        }
    }

}
