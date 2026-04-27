using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : GenericSingleton<ScreenFader>
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 1f;

    public event Action OnFadeInComplete;
    public event Action OnFadeOutComplete;

    public void FadeIn() => StartCoroutine(Fade(0f, 1f, OnFadeInComplete));
    public void FadeOut() => StartCoroutine(Fade(1f, 0f, OnFadeOutComplete));

    public void FadeInOut(Action onBlack = null) => StartCoroutine(FadeInOutRoutine(onBlack));

    private IEnumerator FadeInOutRoutine(Action onBlack)
    {
        yield return Fade(0f, 1f, null);
        onBlack?.Invoke();
        yield return Fade(1f, 0f, null);
    }

    private IEnumerator Fade(float from, float to, Action onComplete)
    {
        float elapsed = 0f;
        Color color = _fadeImage.color;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(from, to, elapsed / _fadeDuration);
            _fadeImage.color = color;
            yield return null;
        }

        color.a = to;
        _fadeImage.color = color;
        onComplete?.Invoke();
    }
}
