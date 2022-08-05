using UnityEngine;

public class SuspensionController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    [Header("Suspension Info")]
    [SerializeField] private Transform _suspensionTop;
    [SerializeField] private float _originSuspensionLength;
    [SerializeField] private float _modifiableSuspensionLength;
    [SerializeField] private float _springFactor;
    [SerializeField] private float _damperFactor;
    [SerializeField] private float _wheelRadius;

    private float _previousSuspensionLength;
    private float _currentSuspensionLength;
    private float _springVelocity;
    private float _currentSpringForce;
    private float _currentDamperForce;
    private Vector3 _currentSuspensionForce;

    private float _length_with_maxTension
    {
        get
        {
            return _originSuspensionLength + _modifiableSuspensionLength;
        }
    }
    private float _rayLength
    {
        get
        {
            return _length_with_maxTension + _wheelRadius;
        }
    }

    private bool _isGrounded;
    private Vector3 _groundedPos;

    private void FixedUpdate()
    {
        if (Physics.Raycast(_suspensionTop.position, -transform.up, out RaycastHit hit, _rayLength)){
            if (!_isGrounded){
                _groundedPos = transform.position;
                _isGrounded = true;
            }
            else
                transform.position = _groundedPos;

            _previousSuspensionLength = _currentSuspensionLength;
            _currentSuspensionLength = hit.distance - _wheelRadius;
            _springVelocity = (_previousSuspensionLength - _currentSuspensionLength) / Time.fixedDeltaTime;

            _currentSpringForce = _springFactor * (_originSuspensionLength - _currentSuspensionLength);
            _currentDamperForce = _damperFactor * _springVelocity;

            _currentSuspensionForce = (_currentSpringForce + _currentDamperForce) * transform.up;
            _rb.AddForceAtPosition(_currentSuspensionForce, hit.point);
        }
    }
}
