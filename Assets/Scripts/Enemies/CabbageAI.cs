using System;
using Unity.VisualScripting;
using UnityEngine;

public class CabbageAI : EnemyAI
{

    public static event EventHandler OnCabbageEnemyKilled;

    public static void ResetStaticData() {
        OnCabbageEnemyKilled = null;
    }

    public event EventHandler OnCabbageEnemyHit;

    private float currentHealth;

    private void Awake() {
        CabbageSetHealth();
        CabbageSetSpeed();
    }

    private void Start() {
        currentHealth = maxHealth;
    }

    private void Update() {
        Move(moveSpeed);

        HitByProjectile();
    }

    protected override void TakeDamage(float amount) {
        currentHealth -= amount;
        if (currentHealth <= 0) {
            OnCabbageEnemyKilled?.Invoke(this, EventArgs.Empty);
            Die();
        }
    }

    protected override void HitByProjectile() {
        float enemyRadius = 2f;
        Collider2D collider = Physics2D.OverlapCircle((Vector2)transform.position, enemyRadius, GetProjectileLayerMask());

        if (collider != null) {
            Destroy(collider.gameObject);
            TakeDamage(collider.GetComponent<Projectile>().GetProjectileSO().damage);
            OnCabbageEnemyHit?.Invoke(this, EventArgs.Empty);
        }
    }

    protected override Vector2 DontHitOtherEnemyPath() {
        float enemyRadius = 2f;
        Collider2D collider = Physics2D.OverlapCircle((Vector2)transform.position, enemyRadius, GetEnemyLayerMask());

        if (collider == null) {
            return Vector2.zero;
        }
        else {
            return (Vector2)(transform.position - collider.transform.position);
        }
    }


    protected override void Move(float moveSpeed) {
        Vector3 moveDir = ((Player.Instance.transform.position - transform.position) + new Vector3(DontHitOtherEnemyPath().x, DontHitOtherEnemyPath().y, 0f) * 1.85f).normalized;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void CabbageSetHealth() {
        SetHealth(3f);
    }

    private void CabbageSetSpeed() {
        SetSpeed(4f);
    }

}
