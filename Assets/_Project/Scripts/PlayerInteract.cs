using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float _distance;
    [SerializeField] private LayerMask _layerMask;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, _distance, _layerMask))

        {

            if (hit.collider.TryGetComponent(out IInteractable interactable))

            {

                interactable.Interact();

                Debug.Log("RAYCAST PARTE");

            }

        }
    }
}
