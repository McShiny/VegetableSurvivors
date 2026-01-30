using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI playerScoreText;

    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitGameButton;

    private void Awake() {
        playAgainButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.Gameplay);
        });
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenu);
        });
        quitGameButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }

    private void Start() {
        VegetableGameManager.Instance.OnStateChanged += VegetableGameManager_OnStateChanged;

        Hide();
    }

    private void VegetableGameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (VegetableGameManager.Instance.IsGameOver()) {
            Show();

            playerScoreText.text = "5";
        }
        else {
            Hide();
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
}
