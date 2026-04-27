using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    private bool _playerInRange;

    private void Update()
    {
        if (_playerInRange && Input.GetKeyDown(KeyCode.E))
            OnInteract();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
           _playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
            _playerInRange = false;
    }

    protected abstract void OnInteract();
}
