using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance {  get; private set; }

    public event EventHandler OnPlayerDamaged;
    public event EventHandler OnStateChanged;
    public event EventHandler OnGameOver;

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
    [SerializeField] private PlayerAimWeapon playerAimWeapon;

    [SerializeField] private List<PlayerAgesSO> playerAgesSOList;

    private State state;

    private float moveSpeed;

    private float immunityTime = 0.5f;
    private bool isImmune = false;
    private float maxAge = 100f;

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

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        state = State.Baby;
        UpdatePlayerStats(state);

        playerAimWeapon.OnPlayerFire += PlayerAimWeapon_OnPlayerFire;
        playerAimWeapon.OnPlayerAlternateFire += PlayerAimWeapon_OnPlayerAlternateFire;
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

        if (isImmune) {
            if (immunityTime <= 0) {
                isImmune = false;
                immunityTime = 0.25f;
            }
            immunityTime -= Time.deltaTime;
        }

        if(isRecoil) {
            if (Mathf.Sqrt(Vector3.Dot(distanceRecoiled, distanceRecoiled)) <= Mathf.Sqrt(Vector3.Dot(dirRecoil, dirRecoil))) {
                transform.position += dirRecoil * recoilSpeed * Time.deltaTime;
                distanceRecoiled += dirRecoil * recoilSpeed * Time.deltaTime;
            }
            else {
                distanceRecoiled = Vector3.zero;
                dirRecoil = Vector3.zero;
                isRecoil = false;
            }
        }

        if (isPushed) {
            if (Mathf.Sqrt(Vector3.Dot(distancePushed, distancePushed)) <= Mathf.Sqrt(Vector3.Dot(dirPush, dirPush))) {
                transform.position += dirPush * pushSpeed * Time.deltaTime;
                distancePushed += dirPush * pushSpeed * Time.deltaTime;
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

        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0);

        player.transform.position += moveDir * moveSpeed * Time.deltaTime;
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

    private void pushBack() {
        dirPush = -2 * (enemyPosition - transform.position);
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

}
