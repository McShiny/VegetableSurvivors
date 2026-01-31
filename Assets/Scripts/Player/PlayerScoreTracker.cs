using System;
using UnityEngine;

public class PlayerScoreTracker : MonoBehaviour
{

    public static PlayerScoreTracker Instance { get; private set; }

    public event EventHandler<OnScoreChangedEventArgs> OnScoreChanged;
    public class OnScoreChangedEventArgs : EventArgs {
        public int score;
    }

    private int score;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        CabbageAI.OnCabbageEnemyKilled += CabbageAI_OnCabbageEnemyKilled;
        CucumberAI.OnCucumberEnemyKilled += CucumberAI_OnCucumberEnemyKilled;
    }

    private void CucumberAI_OnCucumberEnemyKilled(object sender, EventArgs e) {
        score += 3;
        OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs {
            score = score
        });
    }

    private void CabbageAI_OnCabbageEnemyKilled(object sender, EventArgs e) {
        score++;
        OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs {
            score = score
        });
    }

    public int GetScore() {
        return score;
    }

}
