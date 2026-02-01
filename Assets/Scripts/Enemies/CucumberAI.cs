using System;
using UnityEngine;

public class CucumberAI : EnemyAI
{

    public static event EventHandler OnCucumberEnemyKilled;

    public static void ResetStaticData() {
        OnCucumberEnemyKilled = null;
    }

    public event EventHandler OnCucumberEnemyHit;

    private float currentHealth;

    private Vector3 dashDirection;
    private bool isDash = false;
    private float dashTime = 1.5f;

    private bool doingDash = false;
    private float doingDashTime = 1.2f;

    private bool canDash = true;
    private bool dashOnCooldown = false;
    private float dashCooldown = 5f;

    private void Awake() {
        CucumberSetHealth();
        CucumberSetSpeed();
    }

    private void Start() {
        currentHealth = maxHealth;
    }

    private void Update() {
        Move(moveSpeed);

        HitByProjectile();

        if(isDash) {
            if(dashTime <= 0) {
                CucumberDash();
                dashTime = 1.5f;
                isDash = false;
                doingDash = true;
            }
            transform.Rotate(0f, 0f, 800f / 1.5f * Time.deltaTime);
            dashTime -= Time.deltaTime;
        }

        if(doingDash) {
            if (doingDashTime <= 0) {
                doingDash = false;
                doingDashTime = 0.7f;
                dashOnCooldown = true;
            } else {
                CucumberDash();
                doingDashTime -= Time.deltaTime;
            }
        }

        if (dashOnCooldown) {
            if(dashCooldown <= 0) {
                dashOnCooldown = false;
                canDash = true;
                dashCooldown = 5f;
            }
            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, (transform.forward + new Vector3(0f, 0f, 100f)).normalized, rotateSpeed * Time.deltaTime);
            dashCooldown -= Time.deltaTime;
        }
    }

    protected override void TakeDamage(float amount) {
        currentHealth -= amount;
        if (currentHealth <= 0) {
            OnCucumberEnemyKilled?.Invoke(this, EventArgs.Empty);
            Die();
        }
    }

    protected override void HitByProjectile() {
        float capsuleWidth = 1.2f;
        float capsuleHeight = 2.4f;
        Vector2 capsuleSize = new Vector2(capsuleWidth, capsuleHeight);

        Collider2D collider = Physics2D.OverlapCapsule((Vector2)transform.position, capsuleSize, CapsuleDirection2D.Vertical, 0f, GetProjectileLayerMask());

        if (collider != null) {
            Destroy(collider.gameObject);
            TakeDamage(collider.GetComponent<Projectile>().GetProjectileSO().damage);
            OnCucumberEnemyHit?.Invoke(this, EventArgs.Empty);
        }
    }

    protected override Vector2 DontHitOtherEnemyPath() {
        float capsuleWidth = 1.2f; 
        float capsuleHeight = 2.4f;
        Vector2 capsuleSize = new Vector2(capsuleWidth, capsuleHeight);

        Collider2D collider = Physics2D.OverlapCapsule((Vector2)transform.position, capsuleSize, CapsuleDirection2D.Vertical, 0f, GetEnemyLayerMask());

        if (collider == null) {
            return Vector2.zero;
        }
        else {
            return (Vector2)(transform.position - collider.transform.position);
        }
    }

    protected override void Move(float moveSpeed) {
        if (((Math.Sqrt(Vector3.Dot((Player.Instance.transform.position - transform.position), (Player.Instance.transform.position - transform.position))) <= 10f)) && canDash) {
            isDash = true;
            canDash = false;
            dashDirection = (Player.Instance.transform.position - transform.position).normalized;
        } else if (!isDash && !doingDash) {
            Vector3 moveDir = ((Player.Instance.transform.position - transform.position) + new Vector3(DontHitOtherEnemyPath().x, DontHitOtherEnemyPath().y, 0f) * 2f).normalized;

            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
            
    }
    private void CucumberDash() {
        float dashSpeed = 20f;
        transform.position += dashDirection * dashSpeed * Time.deltaTime;
    }

    private void CucumberSetHealth() {
        SetHealth(5f);
    }

    private void CucumberSetSpeed() {
        SetSpeed(5f);
    }

}
