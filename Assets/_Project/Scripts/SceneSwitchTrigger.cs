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
        if (other.CompareTag("Player"))
        {
            LoadScene();
        }
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
