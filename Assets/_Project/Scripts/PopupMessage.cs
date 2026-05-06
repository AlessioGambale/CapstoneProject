using System.Collections;
using TMPro;
using UnityEngine;

public class PopupMessage : GenericSingleton<PopupMessage>
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _merchantUI;

    public void Show(string message, float duration = 2f)
    {
        if (_merchantUI != null)
            _merchantUI.SetActive(false);

        _panel.SetActive(true);
        _text.SetText(message);
        StopAllCoroutines();
        StartCoroutine(Hide(duration));
    }

    private IEnumerator Hide(float duration)
    {
        yield return new WaitForSeconds(duration);
        _panel.SetActive(false);
    }
}