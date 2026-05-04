using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private bool _isUIOpen;
    public bool IsUIOpen => _isUIOpen;
    private void Awake()
    {
        _isUIOpen = false;
    }
    public void OpenUI()
    {
        _isUIOpen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseUI()
    {
        _isUIOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Play()
    {
        SceneManager.LoadScene("ExplorationScene");
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
