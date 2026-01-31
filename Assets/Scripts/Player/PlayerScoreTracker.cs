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
        EnemyAI.OnEnemyKilled += EnemyAI_OnEnemyKilled;
    }

    private void EnemyAI_OnEnemyKilled(object sender, System.EventArgs e) {
        score++;
        OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs {
            score = score
        });
    }

    public int GetScore() {
        return score;
    }

}
