using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchTrigger : MonoBehaviour
{
    public enum SceneType
    {
        Camp,
        Exploration,
        Castle
    }

    public SceneType sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (sceneToLoad == SceneType.Castle)
        {
            bool hasWeapon = InventoryManager.Instance.CurrentWeapon != null;
            bool hasAbility = InventoryManager.Instance.CurrentAbility != null;

            if (!hasWeapon || !hasAbility)
            {
                PopupMessage.Instance.Show("Make sure to grab your gear before heading into battle");
                return;
            }
        }

        LoadScene();
    }

    void LoadScene()
    {
        switch (sceneToLoad)
        {
            case SceneType.Camp:
                SceneManager.LoadScene("CampScene");
                break;
            case SceneType.Exploration:
                SceneManager.LoadScene("ExplorationScene");
                break;
            case SceneType.Castle:
                SceneManager.LoadScene("CastleScene");
                break;
        }
    }
}
