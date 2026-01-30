using UnityEngine;
using UnityEngine.UI;

public class TimeProgressBarUI : MonoBehaviour
{

    [SerializeField] private Image barImage;

    private void Start() {
        Player.Instance.OnPlayerAged += Player_OnPlayerAged;

        barImage.fillAmount = 0f;
    }

    private void Player_OnPlayerAged(object sender, Player.OnPlayerAgedEventArgs e) {
        barImage.fillAmount = e.progressNormalized;
    }
}
