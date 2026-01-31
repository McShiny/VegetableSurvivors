using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] private LayerMask projectileLayerMask;
    [SerializeField] private LayerMask enemyLayerMask;

    protected float moveSpeed = 4f;
    protected float maxHealth = 3f;

    protected virtual void TakeDamage(float amount) {
        
    }

    protected virtual void HitByProjectile() {

    }

    protected virtual Vector2 DontHitOtherEnemyPath() {
        return Vector2.zero;
    }

    protected virtual void Move(float moveSpeed) {
        
    }

    protected virtual void Die() {
        Destroy(gameObject);
    }

    public LayerMask GetProjectileLayerMask() { return projectileLayerMask; }
    public LayerMask GetEnemyLayerMask() {return enemyLayerMask; }

    public void SetSpeed(float speed) {
        moveSpeed = speed;
    }

    public void SetHealth(float health) {
        maxHealth = health;
    }

}
