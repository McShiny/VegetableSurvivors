using UnityEngine;
using UnityEngine.UI;

public class FireCooldownUI : MonoBehaviour
{

    [SerializeField] private Image fireCooldownCircle;

    private void Start() {
        PlayerAimWeapon.Instance.OnFireCooldown += PlayerAimWeapon_OnFireCooldown;

        fireCooldownCircle.fillAmount = 1f;
    }

    private void PlayerAimWeapon_OnFireCooldown(object sender, PlayerAimWeapon.OnFireCooldownEventArgs e) {
        fireCooldownCircle.fillAmount = e.cooldownNormalized;
    }

}
