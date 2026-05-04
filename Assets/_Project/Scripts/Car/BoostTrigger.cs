using UnityEngine;

public class BoostTrigger : MonoBehaviour
{
    [SerializeField] private float _boostForce;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Car")) return;
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(Vector3.up * _boostForce, ForceMode.Impulse);
    }
}