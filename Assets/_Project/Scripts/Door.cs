using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string _requiredPath;
    [SerializeField] private int _pathStep;

    private void Start()
    {
        Debug.Log($"[Door] Start — RunManager: {RunManager.Instance}, path: {RunManager.Instance?.ChosenPath}");
        RunManager.Instance.OnPathChosen += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        string chosenPath = RunManager.Instance.ChosenPath;
        Debug.Log($"[Door] Refresh — chosenPath: {chosenPath}, requiredPath: {_requiredPath}, step: {_pathStep}");

        if (string.IsNullOrEmpty(chosenPath))
        {
            gameObject.SetActive(true);
            return;
        }

        string[] steps = chosenPath.Split('_');
        Debug.Log($"[Door] steps: {string.Join(",", steps)}, step[{_pathStep}]: {(steps.Length > _pathStep ? steps[_pathStep] : "N/A")}");

        if (_pathStep < steps.Length && steps[_pathStep] == _requiredPath)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (RunManager.Instance != null)
            RunManager.Instance.OnPathChosen -= Refresh;
    }
}
