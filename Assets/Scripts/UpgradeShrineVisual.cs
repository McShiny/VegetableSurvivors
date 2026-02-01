using UnityEngine;

public class UpgradeShrineVisual : MonoBehaviour
{

    [SerializeField] private UpgradeShrine shrine;
    [SerializeField] private GameObject shrineVisual;

    private void Start() {
        Hide();
    }

    private void Update() {
        if (shrine.IsShrineActive()) {
            Show();
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
