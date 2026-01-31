using System;
using UnityEngine;

public class CarrotAI : EnemyAI
{
    public static event EventHandler OnCarrotEnemyKilled;

    public static void ResetStaticData() {
        OnCarrotEnemyKilled = null;
    }

    public event EventHandler OnCarrotEnemyHit;

    [SerializeField] private Transform carrotBullet;

    private float currentHealth;

    private bool hasFired = false;
    private bool firing = false;
    private float fireCooldown = 3f;
    private float fireTime = 0.25f;
    private int maxTimesFired = 6;
    private int timesFired = 0;

    private void Awake() {
        CarrotSetHealth();
        CarrotSetSpeed();
    }

    private void Start() {
        currentHealth = maxHealth;
    }

    private void Update() {
        
        Move(moveSpeed);

        HitByProjectile();

        if (Mathf.Sqrt(Vector3.Dot((Player.Instance.transform.position - transform.position), (Player.Instance.transform.position - transform.position))) <= 10f) {
            if(!hasFired) {
                hasFired = true;
                firing = true;
            }
        }

        if (firing) {
            if(fireTime <= 0) {
                if(timesFired <= maxTimesFired) {
                    FireBullet();
                    timesFired++;
                    fireTime = 0.2f;
                } else {
                    firing = false;
                    fireTime = 0.25f;
                    timesFired = 0;
                } 
            }

            fireTime -= Time.deltaTime;
        }

        if (hasFired && !firing) {
            if (fireCooldown <= 0) {
                hasFired = false;
                fireCooldown = 3f;
            }
            fireCooldown -= Time.deltaTime;
        }
    }

    protected override void TakeDamage(float amount) {
        currentHealth -= amount;
        if (currentHealth <= 0) {
            OnCarrotEnemyKilled?.Invoke(this, EventArgs.Empty);
            Die();
        }
    }

    protected override void HitByProjectile() {
        float capsuleWidth = 0.8f; ;
        float capsuleHeight = 1.8f;
        Vector2 capsuleSize = new Vector2(capsuleWidth, capsuleHeight);

        Collider2D collider = Physics2D.OverlapCapsule((Vector2)transform.position, capsuleSize, CapsuleDirection2D.Vertical, 0f, GetProjectileLayerMask());

        if (collider != null) {
            Destroy(collider.gameObject);
            TakeDamage(collider.GetComponent<Projectile>().GetProjectileSO().damage);
            OnCarrotEnemyHit?.Invoke(this, EventArgs.Empty);
        }
    }

    protected override Vector2 DontHitOtherEnemyPath() {
        float capsuleWidth = 0.8f;
        float capsuleHeight = 1.8f;
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
        Vector3 moveDir = ((Player.Instance.transform.position - transform.position) + new Vector3(DontHitOtherEnemyPath().x, DontHitOtherEnemyPath().y, 0f) * 1.85f).normalized;
        if (!firing) {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        else {
            transform.LookAt(Player.Instance.transform.position);
        }
    }

    private void FireBullet() {
        Instantiate(carrotBullet, transform.position, Quaternion.identity);
    }

    private void CarrotSetHealth() {
        SetHealth(3f);
    }

    private void CarrotSetSpeed() {
        SetSpeed(4f);
    }
}
