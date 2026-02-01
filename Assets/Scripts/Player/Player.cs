using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance {  get; private set; }

    public event EventHandler OnPlayerDamaged;
    public event EventHandler OnStateChanged;
    public event EventHandler OnGameOver;
    public event EventHandler OnGotUpgrade;

    public event EventHandler<OnPlayerAgedEventArgs> OnPlayerAged;
    public class OnPlayerAgedEventArgs : EventArgs {
        public float progressNormalized;
    }

    public enum State {
        Baby,
        Teen,
        Adult,
        Elder,
    }

    [SerializeField] private Transform player;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask enemiesLayerMask;
    [SerializeField] private LayerMask enemyProjectileLayerMask;
    [SerializeField] private LayerMask shrineLayerMask;
    [SerializeField] private LayerMask cantMoveLayerMask;
    [SerializeField] private PlayerAimWeapon playerAimWeapon;

    [SerializeField] private List<PlayerAgesSO> playerAgesSOList;

    private State state;

    private float moveSpeed;

    private float immunityTime = 0.5f;
    private bool isImmune = false;
    private float maxAge = 100f;

    private Vector3 lastDirection;
    private bool isWalking = false;

    private Vector3 dirRecoil;
    private Vector3 distanceRecoiled;
    private bool isRecoil;
    private float recoilSpeed = 10f;

    private Vector3 enemyPosition;
    private Vector3 dirPush;
    private Vector3 distancePushed;
    private bool isPushed = false;
    private float pushSpeed = 10f;

    private float age = 0f;
    private float ageIncreaseTime = 10f;

    private float damageMultiplier = 1f;

    private bool isUpgraded = false;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        state = State.Baby;
        UpdatePlayerStats(state);

        playerAimWeapon.OnPlayerFire += PlayerAimWeapon_OnPlayerFire;
        playerAimWeapon.OnPlayerAlternateFire += PlayerAimWeapon_OnPlayerAlternateFire;
        Upgrade.Instance.OnDamageUpgrade += Upgrade_OnDamageUpgrade;
        Upgrade.Instance.OnHealthUpgrade += Upgrade_OnHealthUpgrade;
        EnemySpawner.Instance.OnWaveChanged += EnemySpawner_OnWaveChanged;
    }

    private void EnemySpawner_OnWaveChanged(object sender, EnemySpawner.OnWaveChangedEventArgs e) {
        isUpgraded = false;
        if (age >= 10f) {
            age -= 10;
        }
        else {
            age = 0f;
        }
    }

    private void Upgrade_OnHealthUpgrade(object sender, EventArgs e) {
        if (age >= 15f) {
            age -= 15;
        }
        else {
            age = 0f;
        }
        OnPlayerAged?.Invoke(this, new OnPlayerAgedEventArgs {
            progressNormalized = age / maxAge
        });
    }

    private void Upgrade_OnDamageUpgrade(object sender, EventArgs e) {
        damageMultiplier += .5f;
    }

    private void PlayerAimWeapon_OnPlayerAlternateFire(object sender, EventArgs e) {
        AlternateRecoil();
    }

    private void PlayerAimWeapon_OnPlayerFire(object sender, EventArgs e) {
        Recoil();
    }

    void Update()
    {

        switch (state) {
            case State.Baby:
                if (age > 12) {
                    state = State.Teen;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                    UpdatePlayerStats(state);
                }
                break;
            case State.Teen:
                if (age > 24) {
                    state = State.Adult;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                    UpdatePlayerStats(state);
                }
                break;
            case State.Adult:
                if (age > 60) {
                    state = State.Elder;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                    UpdatePlayerStats(state);
                }
                break;
            case State.Elder:
                break;
        }

        if (ageIncreaseTime <= 0) {
            age++;

            OnPlayerAged?.Invoke(this, new OnPlayerAgedEventArgs {
                progressNormalized = age / maxAge
            });

            ageIncreaseTime = 10f;
        }

        ageIncreaseTime -= Time.deltaTime;

        HandleMovement();
        HandleTakeDamage();
        HitByProjectile();
        HandleUpgradeShrine();

        if (isImmune) {
            if (immunityTime <= 0) {
                isImmune = false;
                immunityTime = 0.25f;
            }
            immunityTime -= Time.deltaTime;
        }

        if(isRecoil) {
            if (Mathf.Sqrt(Vector3.Dot(distanceRecoiled, distanceRecoiled)) <= Mathf.Sqrt(Vector3.Dot(dirRecoil, dirRecoil))) {
                float capsuleWidth = 0f;
                float capsuleHeight = 0f;

                foreach (var status in playerAgesSOList) {
                    if (status == null) continue;

                    if (status.ageName == state.ToString()) {
                        capsuleWidth = status.width;
                        capsuleHeight = status.height;
                    }
                }

                Vector2 capsuleSize = new Vector2(capsuleWidth, capsuleHeight);

                bool canMove = !Physics2D.OverlapCapsule((Vector2)transform.position, capsuleSize, CapsuleDirection2D.Vertical, 0f, cantMoveLayerMask);

                if (canMove) {
                    transform.position += dirRecoil * recoilSpeed * Time.deltaTime;
                    distanceRecoiled += dirRecoil * recoilSpeed * Time.deltaTime;
                }
                
            }
            else {
                distanceRecoiled = Vector3.zero;
                dirRecoil = Vector3.zero;
                isRecoil = false;
            }
        }

        if (isPushed) {
            if (Mathf.Sqrt(Vector3.Dot(distancePushed, distancePushed)) <= Mathf.Sqrt(Vector3.Dot(dirPush, dirPush))) {
                float capsuleWidth = 0f;
                float capsuleHeight = 0f;

                foreach (var status in playerAgesSOList) {
                    if (status == null) continue;

                    if (status.ageName == state.ToString()) {
                        capsuleWidth = status.width;
                        capsuleHeight = status.height;
                    }
                }

                Vector2 capsuleSize = new Vector2(capsuleWidth, capsuleHeight);

                bool canMove = !Physics2D.OverlapCapsule((Vector2)transform.position, capsuleSize, CapsuleDirection2D.Vertical, 0f, cantMoveLayerMask);

                if (canMove) {
                    transform.position += dirPush * pushSpeed * Time.deltaTime;
                    distancePushed += dirPush * pushSpeed * Time.deltaTime;
                }
                
            }
            else {
                distancePushed = Vector3.zero;
                dirPush = Vector3.zero;
                isPushed = false;
            }
        }

    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        if ((inputVector.x != 0)) {
            lastDirection = inputVector;
        }
        if(inputVector.magnitude > 0) {
            isWalking = true;
        }
        else {
            isWalking = false;
        }
            Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0);

        float capsuleWidth = 0f;
        float capsuleHeight = 0f;

        foreach (var status in playerAgesSOList) {
            if (status == null) continue;

            if (status.ageName == state.ToString()) {
                capsuleWidth = status.width;
                capsuleHeight = status.height;
            }
        }

        Vector2 capsuleSize = new Vector2(capsuleWidth, capsuleHeight);

        bool canMove = !Physics2D.OverlapCapsule((Vector2)transform.position, capsuleSize, CapsuleDirection2D.Vertical, 0f, cantMoveLayerMask);

        if (canMove) {
            player.transform.position += moveDir * moveSpeed * Time.deltaTime;
        } else {
            player.transform.position += -player.transform.position.normalized * moveSpeed * Time.deltaTime;
        }
        
    }

    private void HandleTakeDamage() {
        float capsuleWidth = 1f;
        float cpsuleHeight = 2f;

        Collider2D collider = Physics2D.OverlapCapsule((Vector2)transform.position, new Vector2(capsuleWidth, cpsuleHeight), CapsuleDirection2D.Vertical, 0f, enemiesLayerMask);

        if (collider != null) {
            enemyPosition = collider.transform.position;
            TakeDamage();
        }
    }

    private void TakeDamage() {
        if (!isImmune) {
            age += 6;

            OnPlayerAged?.Invoke(this, new OnPlayerAgedEventArgs {
                progressNormalized = age / maxAge
            });

            pushBack();
            OnPlayerDamaged?.Invoke(this, EventArgs.Empty);
            isImmune = true;
        }

        if (age >= maxAge) {
            OnGameOver?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }

    private void TakeDamage(float damage, float pushStrength) {
        if (!isImmune) {
            age += damage;

            OnPlayerAged?.Invoke(this, new OnPlayerAgedEventArgs {
                progressNormalized = age / maxAge
            });

            if (pushStrength > 0) {
                pushBack(pushStrength);
            }
            OnPlayerDamaged?.Invoke(this, EventArgs.Empty);
            isImmune = true;
        }

        if (age >= maxAge) {
            OnGameOver?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }

    private void HitByProjectile() {
        float capsuleWidth = 0f;
        float capsuleHeight = 0f;

        foreach (var status in playerAgesSOList) {
            if (status == null) continue;

            if (status.ageName == state.ToString()) {
                capsuleWidth = status.width;
                capsuleHeight = status.height;
            }
        }
        
        Vector2 capsuleSize = new Vector2(capsuleWidth, capsuleHeight);

        Collider2D collider = Physics2D.OverlapCapsule((Vector2)transform.position, capsuleSize, CapsuleDirection2D.Vertical, 0f, enemyProjectileLayerMask);

        if (collider != null) {
            Destroy(collider.gameObject);
            TakeDamage(collider.GetComponent<Projectile>().GetProjectileSO().damage, 0f);
            OnPlayerDamaged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HandleUpgradeShrine() {
        float capsuleWidth = 0f;
        float capsuleHeight = 0f;

        foreach (var status in playerAgesSOList) {
            if (status == null) continue;

            if (status.ageName == state.ToString()) {
                capsuleWidth = status.width;
                capsuleHeight = status.height;
            }
        }

        Vector2 capsuleSize = new Vector2(capsuleWidth, capsuleHeight);

        Collider2D collider = Physics2D.OverlapCapsule((Vector2)transform.position, capsuleSize, CapsuleDirection2D.Vertical, 0f, shrineLayerMask);

        if (collider != null && UpgradeShrine.Instance.IsShrineActive() && !isUpgraded) {
            OnGotUpgrade?.Invoke(this, EventArgs.Empty);
            UpgradeShrine.Instance.SetShrineInactive();
            isUpgraded = true;
        }
    }

    private void pushBack() {
        dirPush = -2 * (enemyPosition - transform.position);
        dirPush.z = 0;
        isPushed = true;
    }

    private void pushBack(float pushStrength) {
        dirPush = -1 * pushStrength * (enemyPosition - transform.position);
        dirPush.z = 0;
        isPushed = true;
    }

    private void Recoil() {
        float recoilStrength = 2f;
        dirRecoil =  -1 * recoilStrength * playerAimWeapon.GetAimDirection();
        dirRecoil.z = 0;
        isRecoil = true;
    }

    private void AlternateRecoil() {
        float recoilStrength = 3.5f;
        dirRecoil = -1 * recoilStrength * playerAimWeapon.GetAimDirection();
        dirRecoil.z = 0;
        isRecoil = true;
    }

    private void UpdatePlayerStats(State state) {
        foreach (var status in playerAgesSOList) {
            if (status == null) continue;

            if (status.ageName == state.ToString()) {
                moveSpeed = status.speed;
            }
        }
    }

    public State GetPlayerState() {
        return state;
    }

    public Vector3 GetLastDirection() {
        return lastDirection;
    }
    
    public bool IsWalking() {
        return isWalking;
    }

    public float GetDamageMultiplier() {
        return damageMultiplier;
    }

    public float GetPlayerAge() {
        return age;
    }

}
