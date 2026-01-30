using UnityEngine;


public class PlayerAimWeapon : MonoBehaviour
{

    [SerializeField] private Transform aimTransform;

    private void Update() {

        Vector3 mousePosition = GetMousePosition.GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0 , 0, angle);

    }

    private void HandleShooting () {

    }

}
