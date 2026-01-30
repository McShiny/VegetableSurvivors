using System;
using UnityEditor;
using UnityEngine;


public class PlayerAimWeapon : MonoBehaviour
{

    public event EventHandler OnPlayerFire;

    [SerializeField] private Transform aimTransform;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform shootPosition;

    private Vector3 aimDirection;

    private void Start() {
        GameInput.Instance.OnFiredProjectile += GameInput_OnFiredProjectile;
    }

    private void GameInput_OnFiredProjectile(object sender, System.EventArgs e) {
        Instantiate(bulletPrefab, shootPosition.transform.position, Quaternion.identity);
        OnPlayerFire?.Invoke(this, EventArgs.Empty);
    }

    private void Update() {

        HandleAiming();

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
