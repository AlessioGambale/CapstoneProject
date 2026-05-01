using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHighlight : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _highlightColor = Color.red;

    private Color _originalColor;

    private void Awake()
    {
        _originalColor = _renderer.material.color;
    }

    public void SetHighlight(bool active)
    {
        _renderer.material.color = active ? _highlightColor : _originalColor;
    }
}
