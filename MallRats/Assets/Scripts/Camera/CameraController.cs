using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _mouseSensitivity = 2.0f;

    [SerializeField]
    private float _positionSmoothing = 4.0f;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Transform _cameraAnchor;

    [SerializeField]
    private InputHandler _inputHandler;

    private Vector3 _inputRotationDelta;
    private Vector3 _targetRotation;
    private Vector3 _velocity;

    protected void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected void LateUpdate()
    {
        _inputRotationDelta = _inputHandler.MousePositionDelta * _mouseSensitivity;

        _targetRotation.x = Mathf.Clamp(_targetRotation.x - _inputRotationDelta.y, -90.0f, 90.0f);
        _targetRotation.y += _inputRotationDelta.x;

        _camera.transform.localEulerAngles = _targetRotation;

        _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, _cameraAnchor.position, ref _velocity, _positionSmoothing * Time.deltaTime);
    }
}
