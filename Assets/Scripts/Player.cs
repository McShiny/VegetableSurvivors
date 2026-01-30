using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private Transform player;

    [SerializeField] private float moveSpeed = 7f;

    void Update()
    {

        Vector2 inputVector = new Vector2 (0, 0);

        if (Input.GetKey(KeyCode.W)) {
            inputVector.y += 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            inputVector.y -= 1;
        }
        if (Input.GetKey(KeyCode.A)) {
            inputVector.x -= 1;
        }
        if (Input.GetKey(KeyCode.D)) {
            inputVector.x += 1;
        }

        inputVector = inputVector.normalized;

        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0);
        player.transform.position += moveDir * moveSpeed * Time.deltaTime;

        
    }
}
