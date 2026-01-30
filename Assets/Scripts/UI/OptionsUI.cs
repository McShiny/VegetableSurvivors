using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{

    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button closeButton;

    private void Awake() {
        Instance = this;

        closeButton.onClick.AddListener(() => {
            Hide();
            GamePausedUI.Instance.Show();
        });
    }

    private void Start() {
        Hide();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

}
