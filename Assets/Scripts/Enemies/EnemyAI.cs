using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private float moveSpeed = 3f;

    private void Update() {
        
        Vector3 moveDir = (Player.Instance.transform.position - transform.position).normalized;

        transform.position += moveDir * moveSpeed * Time.deltaTime;

    }

}
