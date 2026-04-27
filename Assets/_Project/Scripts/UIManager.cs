using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] Canvas _canvas;
    [SerializeField] private bool _isActive = false;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private Camera _mainCamera;

    private bool _isUIOpen;
    public bool IsUIOpen => _isUIOpen;
    private void Awake()
    {

        _isUIOpen = false;
        Time.timeScale = 1.0f;
    }
    public void OpenUI()
    {
        _isUIOpen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

    }

    private void Update()
    {
        Pause();
    }
    public void CloseUI()
    {
        _isUIOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        _mainCamera.GetComponent<CinemachineBrain>().enabled = true;
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

    public void Pause()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            if (_isActive)
            {
                OpenUI();
            }
            else
            {
                CloseUI();
            }
            _canvas.gameObject.SetActive(_isActive);
        }
    }
}
