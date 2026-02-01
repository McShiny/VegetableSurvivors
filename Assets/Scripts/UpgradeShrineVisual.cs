using UnityEngine;

public class UpgradeShrineVisual : MonoBehaviour
{

    [SerializeField] private UpgradeShrine shrine;
    [SerializeField] private GameObject shrineVisual;

    private float shrineVisualTime = 1f;
    private bool shrineVisualActive = false;
    private bool hide = false;

    private void Start() {
        Hide();
    }

    private void Update() {
        if (shrine.IsShrineActive()) {
            shrineVisualActive=true;
        } else {
            shrineVisualActive = false;
        }

        if (shrineVisualActive) {
            if (shrineVisualTime <= 0) {
                if(hide) {
                    Hide();
                } else {
                    Show();
                }
                hide = !hide;
                shrineVisualTime = 1f;
            }
            shrineVisualTime -= Time.deltaTime;
        } else {
            Hide();
        }
    }

    private void Show() {
        shrineVisual.SetActive(true);
    }

    private void Hide() {
        shrineVisual.SetActive(false);
    }
}
