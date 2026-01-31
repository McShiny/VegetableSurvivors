using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyAI : MonoBehaviour
{

    public static event EventHandler OnEnemyKilled;

    public event EventHandler OnEnemyHit;

    [SerializeField] private LayerMask projectileLayerMask;
    [SerializeField] private LayerMask enemyLayerMask;

    private float moveSpeed = 4f;
    private float health = 3f;

    private void Start() {
    }

    private void Update() {
        
        Vector3 moveDir = ((Player.Instance.transform.position - transform.position) + new Vector3(DontHitOtherEnemyPath().x, DontHitOtherEnemyPath().y, 0f) * 1.75f).normalized;

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        HitByProjectile();

    }

    private void HitByProjectile() {
        float enemyRadius = 1f;
        Collider2D collider = Physics2D.OverlapCircle((Vector2)transform.position, enemyRadius, projectileLayerMask);

        if (collider != null) {
            Destroy(collider.gameObject);
            TakeDamage();
            OnEnemyHit?.Invoke(this, EventArgs.Empty);
        }
    }

    private Vector2 DontHitOtherEnemyPath() {
        float enemyRadius = 1f;
        Collider2D collider = Physics2D.OverlapCircle((Vector2)transform.position, enemyRadius, enemyLayerMask);

        if (collider == null) {
            return Vector2.zero;
        } else {
            return (Vector2)(transform.position - collider.transform.position);
        }
    }

    private void TakeDamage() {
        health -= 1;
        
        if (health <= 0) {
            Destroy(gameObject);

            OnEnemyKilled?.Invoke(this, EventArgs.Empty);
        }

    }

}
