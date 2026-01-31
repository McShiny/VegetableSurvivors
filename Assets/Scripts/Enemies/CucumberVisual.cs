using UnityEngine;

public class CucumberVisual : MonoBehaviour
{
    [SerializeField] private CucumberAI cucumber;
    [SerializeField] private GameObject damagedVisual;

    private float damageVisualTime = 0.5f;
    private bool startDamageTime = false;

    private void Start() {
        cucumber.OnCucumberEnemyHit += Cucumber_OnCucumberEnemyHit;

        Hide();
    }

    private void Cucumber_OnCucumberEnemyHit(object sender, System.EventArgs e) {
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
