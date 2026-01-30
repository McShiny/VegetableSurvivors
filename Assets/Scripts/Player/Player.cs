using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance {  get; private set; }

    [SerializeField] private Transform player;
    [SerializeField] private GameInput gameInput;

    [SerializeField] private float moveSpeed = 7f;

    private Vector3 lastDirection;

    private void Awake() {
        Instance = this;
    }

    void Update()
    {

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0);
        if (moveDir != Vector3.zero) {
            lastDirection = moveDir;
        }
        player.transform.position += moveDir * moveSpeed * Time.deltaTime;

        
    }

    public Vector3 GetLastDirection() {
        return lastDirection;
    }

    public Vector3 GetPlayerPosition() {
        return transform.position;
    }

}
