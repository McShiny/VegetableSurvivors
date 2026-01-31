using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{

    private void Awake() {
        CucumberAI.ResetStaticData();
        CabbageAI.ResetStaticData();
    }

}
