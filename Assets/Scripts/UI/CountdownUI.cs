using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start() {
        VegetableGameManager.Instance.OnStateChanged += VegetableGameManager_OnStateChanged;

        Hide();
    }

    private void VegetableGameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (VegetableGameManager.Instance.IsCountdownToStartActive()) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Update() {
        countdownText.text = VegetableGameManager.Instance.GetCountrdownToStartTimer().ToString("#");
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
}
