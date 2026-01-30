using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance {  get; private set; }

    [SerializeField] private Transform player;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask enemiesLayerMask;
    [SerializeField] private PlayerAimWeapon playerAimWeapon;

    [SerializeField] private float moveSpeed = 7f;

    private Vector3 lastDirection;
    private float health = 20f;
    private float immunityTime = 0.25f;
    private bool isImmune = false;
    private Vector3 dirRecoil;
    private Vector3 distanceRecoiled;
    private bool isRecoil;
    private float recoilSpeed = 15f;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        playerAimWeapon.OnPlayerFire += PlayerAimWeapon_OnPlayerFire;
    }

    private void PlayerAimWeapon_OnPlayerFire(object sender, EventArgs e) {
        Recoil();
    }

    void Update()
    {

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
            } else {
                isRecoil = false;
            }
        }

    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0);
        if (moveDir != Vector3.zero) {
            lastDirection = moveDir;
        }
        player.transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleTakeDamage() {
        float capsuleWidth = 1f;
        float cpsuleHeight = 2f;

        Collider2D collider = Physics2D.OverlapCapsule((Vector2)transform.position, new Vector2(capsuleWidth, cpsuleHeight), CapsuleDirection2D.Vertical, enemiesLayerMask);

        if (collider != null) {
            TakeDamage();
        }
    }

    private void TakeDamage() {
        if (!isImmune) {
            health -= 1;
            isImmune = true;
        }

        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    private void pushBack() {

    }

    private void Recoil() {
        dirRecoil =  -1 * playerAimWeapon.GetAimDirection();
        dirRecoil.z = 0;
        isRecoil = true;
    }

}
