using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start() {
        PlayerScoreTracker.Instance.OnScoreChanged += PlayerScoreTracker_OnScoreChanged;

        scoreText.text = "Current Score: " + 0;
    }

    private void PlayerScoreTracker_OnScoreChanged(object sender, PlayerScoreTracker.OnScoreChangedEventArgs e) {
        scoreText.text = "Current Score: " + e.score;
    }
}
