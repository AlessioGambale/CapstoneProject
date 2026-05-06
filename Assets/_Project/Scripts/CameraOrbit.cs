using Cinemachine;
using UnityEngine;
public class CameraOrbit : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private CinemachineFreeLook _freeLook;

    [Header("Zoom Settings")]
    [SerializeField] private float _zoomSpeed = 5f;
    [SerializeField] private float _minZoom = 2f;
    [SerializeField] private float _maxZoom = 8f;

    [Header("Zoom Multiplier")]
    [SerializeField] private float _topRadiusMultiplier = 0.5f;
    [SerializeField] private float _topHeightMultiplier = 0.5f;

    [SerializeField] private float _middleHeightMultiplier = 0.3f;

    [SerializeField] private float _bottomRadiusMultiplier = 0.5f;
    [SerializeField] private float _bottomHeightMultiplier = 0.1f;

    private float _currentZoom;
    private Camera _camera;
    public static CameraOrbit Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _camera = Camera.main;
    }

    private void Start()
    {
        LockMouse();
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void LockCamera()
    {
        _freeLook.m_XAxis.m_MaxSpeed = 0f;
        _freeLook.m_YAxis.m_MaxSpeed = 0f;
    }

    public void UnlockCamera()
    {
        _freeLook.m_XAxis.m_MaxSpeed = 300f;
        _freeLook.m_YAxis.m_MaxSpeed = 2f;  
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        _currentZoom -= scroll * _zoomSpeed;
        _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);

        SetZoom(_currentZoom);
    }

    private void SetZoom(float zoom)
    {
        float middleRadius = zoom;
        float topRadius = middleRadius * _topRadiusMultiplier;
        float bottomRadius = middleRadius * _bottomRadiusMultiplier;

        _freeLook.m_Orbits[0].m_Radius = topRadius;
        _freeLook.m_Orbits[1].m_Radius = middleRadius;
        _freeLook.m_Orbits[2].m_Radius = bottomRadius;

        _freeLook.m_Orbits[0].m_Height = zoom * _topHeightMultiplier;
        _freeLook.m_Orbits[1].m_Height = zoom * _middleHeightMultiplier;
        _freeLook.m_Orbits[2].m_Height = zoom * _bottomHeightMultiplier;
    }

    public Vector3 ConvertInputToCameraDirection(Vector3 input)
    {
        Vector3 cameraForward = _camera.transform.forward;
        Vector3 cameraRight = _camera.transform.right;

        Vector3 moveDir = cameraForward * input.z + cameraRight * input.x;
        moveDir.y = 0;

        if (moveDir.magnitude > 0.01f) moveDir.Normalize();

        return moveDir;
    }

    private void LateUpdate()
    {
        HandleZoom();
    }
}