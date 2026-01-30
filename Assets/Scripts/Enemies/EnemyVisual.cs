using UnityEngine;

public class EnemyVisual : MonoBehaviour
{

    [SerializeField] private EnemyAI enemy;
    [SerializeField] private GameObject damagedVisual;

    private float damageVisualTime = 0.5f;
    private bool startDamageTime = false;

    private void Start() {
        enemy.OnEnemyHit += Enemy_OnEnemyHit;

        Hide();
    }

    private void Enemy_OnEnemyHit(object sender, System.EventArgs e) {
        Show();
        startDamageTime = true;
    }

    private void Update() {

        if (startDamageTime) {
            if (damageVisualTime <= 0f) {
                Hide();
                startDamageTime = false;
                damageVisualTime = 0.5f;
            }
            damageVisualTime -= Time.deltaTime;
        }
    }

    private void Hide() {
        damagedVisual.SetActive(false);
    }

    private void Show() {
        damagedVisual.SetActive(true);
    }

}
