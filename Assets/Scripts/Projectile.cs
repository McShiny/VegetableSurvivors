using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private LayerMask enemyLayerMask;

    private Vector3 moveDirection;
    private float moveSpeed = 12f;

    private void Start() {
        Vector3 target = GetMousePosition.GetMouseWorldPosition();
        target.z = transform.position.z;              
        moveDirection = (target - transform.position).normalized;


    }

    private void Update() {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

}
