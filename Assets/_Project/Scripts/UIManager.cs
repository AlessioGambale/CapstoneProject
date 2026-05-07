using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : GenericSingleton<UIManager>
{
    [SerializeField] private GameObject _pausePanel;
    private bool _isPaused;
    private bool _isUIOpen;
    public bool IsUIOpen => _isUIOpen;
   
    public void OpenUI()
    {
        _isUIOpen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CameraOrbit.Instance?.LockCamera();
    }

    public void CloseUI()
    {
        _isUIOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CameraOrbit.Instance?.UnlockCamera();
    }

    public void Play()
    {
        SceneManager.LoadScene("ExplorationScene");
    }
    public void RestartGame()
    {
        RunManager.Instance.ResetRun();
        Time.timeScale = 1f;
        SceneManager.LoadScene("ExplorationScene");
    }

    public void BackToMenu()
    {
        RunManager.Instance.ResetRun();
        InventoryManager.Instance.ClearInventory();
        CoinManager.Instance.ResetCoins();
        RandomDropManager.Instance.ResetDrops();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
#if UNITY_WEBGL
        Application.OpenURL("about:blank");
#else
    Application.Quit();
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        _pausePanel.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0f : 1f;
        OpenUI();
        if (!_isPaused)
        {
            CloseUI();
        }
    }

    public void Resume()
    {
        _isPaused = false;
        _pausePanel.SetActive(false);
        Time.timeScale = 1f;
        CloseUI();
    }

}
