using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public event EventHandler OnEnemyHit;


    [SerializeField] private LayerMask projectileLayerMask;

    private float moveSpeed = 3f;
    private float health = 3f;

    private void Start() {
    }

    private void Update() {
        
        Vector3 moveDir = (Player.Instance.transform.position - transform.position).normalized;

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

    private void TakeDamage() {
        health -= 1;
        
        if (health <= 0) {
            Destroy(gameObject);
        }

    }

}
