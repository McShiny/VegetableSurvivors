using UnityEngine;
using UnityEngine.UI;

public class AlternateFireCooldownUI : MonoBehaviour
{
    [SerializeField] private Image fireCooldownCircle;

    private void Start() {
        PlayerAimWeapon.Instance.OnAlternateFireCooldown += PlayerAimWeapon_OnAlternateFireCooldown;

        fireCooldownCircle.fillAmount = 1f;
    }

    private void PlayerAimWeapon_OnAlternateFireCooldown(object sender, PlayerAimWeapon.OnAlternateFireCooldownEventArgs e) {
        fireCooldownCircle.fillAmount = e.alternateCooldownNormalized;
    }
}
