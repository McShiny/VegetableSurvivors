using System;
using UnityEditor;
using UnityEngine;


public class PlayerAimWeapon : MonoBehaviour
{

    public static PlayerAimWeapon Instance {  get; private set; }

    public event EventHandler OnPlayerFire;

    public event EventHandler<OnFireCooldownEventArgs> OnFireCooldown;
    public class OnFireCooldownEventArgs : EventArgs {
        public float cooldownNormalized;
    }

    [SerializeField] private Transform aimTransform;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform shootPosition;

    private Vector3 aimDirection;
    private float fireCooldown = 1f;
    private bool hasFired = false;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GameInput.Instance.OnFiredProjectile += GameInput_OnFiredProjectile;
    }

    private void GameInput_OnFiredProjectile(object sender, System.EventArgs e) {
        if (!hasFired) {
            Instantiate(bulletPrefab, shootPosition.transform.position, Quaternion.identity);
            OnPlayerFire?.Invoke(this, EventArgs.Empty);
            OnFireCooldown?.Invoke(this, new OnFireCooldownEventArgs {
                cooldownNormalized = 1 - fireCooldown
            });
            hasFired = true;
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

    }

    private void HandleAiming() {
        Vector3 mousePosition = GetMousePosition.GetMouseWorldPosition();

        aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    public Vector3 GetAimDirection() {
        return aimDirection;
    }


}
