using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeProgressBarUI : MonoBehaviour
{

    [SerializeField] private Image barImage;
    [SerializeField] private TextMeshProUGUI ageText;

    private void Start() {
        Player.Instance.OnPlayerAged += Player_OnPlayerAged;

        barImage.fillAmount = 0f;
    }

    private void Player_OnPlayerAged(object sender, Player.OnPlayerAgedEventArgs e) {
        barImage.fillAmount = e.progressNormalized;
        ageText.text = "Current Age: " + Player.Instance.GetPlayerAge();
    }
}
