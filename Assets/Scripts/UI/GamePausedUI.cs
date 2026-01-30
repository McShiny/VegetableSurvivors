using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour
{

    public static GamePausedUI Instance { get; private set; }

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;

    private void Awake() {
        Instance = this;

        resumeButton.onClick.AddListener(() => {
            VegetableGameManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenu);
        });
        optionsButton.onClick.AddListener(() => {
            OptionsUI.Instance.Show();
            Hide();
        });
    }

    private void Start() {
        VegetableGameManager.Instance.OnGamePaused += VegetableGameManager_OnGamePaused;
        VegetableGameManager.Instance.OnGameUnPaused += Vegetable_OnGameUnPaused;

        Hide();
    }

    private void Vegetable_OnGameUnPaused(object sender, System.EventArgs e) {
        Hide();
    }

    private void VegetableGameManager_OnGamePaused(object sender, System.EventArgs e) {
        Show();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}
