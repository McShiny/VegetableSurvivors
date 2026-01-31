using System;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;


public class PlayerAimWeapon : MonoBehaviour
{

    public static PlayerAimWeapon Instance {  get; private set; }

    public event EventHandler OnPlayerFire;
    public event EventHandler OnPlayerAlternateFire;

    public event EventHandler<OnFireCooldownEventArgs> OnFireCooldown;
    public class OnFireCooldownEventArgs : EventArgs {
        public float cooldownNormalized;
    }
    public event EventHandler<OnAlternateFireCooldownEventArgs> OnAlternateFireCooldown;
    public class OnAlternateFireCooldownEventArgs : EventArgs {
        public float alternateCooldownNormalized;
    }

    [SerializeField] private Transform aimTransform;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform shootPosition;

    private Vector3 aimDirection;
    private float fireCooldown = 1f;
    private bool hasFired = false;
    private float fireAlternateCooldown = 5f;
    private bool hasAlternateFired = false;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GameInput.Instance.OnFiredProjectile += GameInput_OnFiredProjectile;
        GameInput.Instance.OnAlternateFiredProjectile += GameInput_OnAlternateFiredProjectile;
    }

    private void GameInput_OnAlternateFiredProjectile(object sender, EventArgs e) {
        if (VegetableGameManager.Instance.IsGamePlaying()) { 
            if (!hasAlternateFired) {
                AlternateFire();
                OnPlayerAlternateFire?.Invoke(this, EventArgs.Empty);
                OnAlternateFireCooldown?.Invoke(this, new OnAlternateFireCooldownEventArgs {
                    alternateCooldownNormalized = 1 - (fireAlternateCooldown / 5)
                });
                hasAlternateFired = true;
            }
        }
    }

    private void GameInput_OnFiredProjectile(object sender, System.EventArgs e) {
        if (VegetableGameManager.Instance.IsGamePlaying()) {
            if (!hasFired) {
                Instantiate(bulletPrefab, shootPosition.transform.position, Quaternion.identity);
                OnPlayerFire?.Invoke(this, EventArgs.Empty);
                OnFireCooldown?.Invoke(this, new OnFireCooldownEventArgs {
                    cooldownNormalized = 1 - fireCooldown
                });
                hasFired = true;
            }
        }
    }

    private void Update() {

        HandleAiming();

        if (hasFired) {
            if (fireCooldown <= 0f) {
                hasFired = false;
                fireCooldown = 1f;
            }
            fireCooldown -= Time.deltaTime;
            OnFireCooldown?.Invoke(this, new OnFireCooldownEventArgs {
                cooldownNormalized = 1 - fireCooldown
            });
        } else {
            OnFireCooldown?.Invoke(this, new OnFireCooldownEventArgs {
                cooldownNormalized = 1f
            });
        }

        if (hasAlternateFired) {
            if (fireAlternateCooldown <= 0f) {
                hasAlternateFired = false;
                fireAlternateCooldown = 5f;
            }
            fireAlternateCooldown -= Time.deltaTime;
            OnAlternateFireCooldown?.Invoke(this, new OnAlternateFireCooldownEventArgs {
                alternateCooldownNormalized = 1 - (fireAlternateCooldown / 5)
            });
        }
        else {
            OnAlternateFireCooldown?.Invoke(this, new OnAlternateFireCooldownEventArgs {
                alternateCooldownNormalized = 1f
            });
        }

    }

    private void HandleAiming() {
        if (VegetableGameManager.Instance.IsGamePlaying()) {
            Vector3 mousePosition = GetMousePosition.GetMouseWorldPosition();

            aimDirection = (mousePosition - transform.position).normalized;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            aimTransform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    private void AlternateFire() {
        if (VegetableGameManager.Instance.IsGamePlaying()) {
            float spreadDistance = 0.6f;
            float[] distances = { -spreadDistance, 0f, spreadDistance };

            foreach (float d in distances) {

                Instantiate(bulletPrefab, shootPosition.transform.position + new Vector3(d, d, 0f), Quaternion.identity);
            }
        }
    }

    public Vector3 GetAimDirection() {
        return aimDirection;
    }


}
