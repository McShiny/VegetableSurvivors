using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    private const string IS_RIGHT = "isRight";
    private const string IS_WALK = "isWalk";

    private Animator babyAnimator;

    private void Awake() {
        babyAnimator = GetComponent<Animator>();
    }

    private void Update() {
        UpdatVisual();
    }

    private void UpdatVisual() {
        babyAnimator.SetBool(IS_RIGHT, Player.Instance.GetLastDirection().x > 0);
        babyAnimator.SetBool(IS_WALK, Player.Instance.IsWalking());
    }

}
