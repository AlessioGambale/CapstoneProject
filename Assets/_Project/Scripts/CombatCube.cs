using UnityEngine;

public class CombatCube : MonoBehaviour
{
    [SerializeField] private GameObject _uiPanel;

    private static CombatCube _currentOpen;

    private void OnMouseDown()
    {
        if (_currentOpen == this)
        {
            Close();
            return;
        }

        if (_currentOpen != null)
            _currentOpen.Close();

        Open();
    }

    private void Open()
    {
        _currentOpen = this;
        _uiPanel.SetActive(true);
    }

    private void Close()
    {
        _currentOpen = null;
        _uiPanel.SetActive(false);
    }
}
