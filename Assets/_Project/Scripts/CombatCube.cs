using UnityEngine;

public class CombatCube : MonoBehaviour, IInteractable 
{
    [SerializeField] private GameObject _uiPanel;

    private static CombatCube _currentOpen;

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

    public void Interact()
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
}
