using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Rigidbody2D _pivot;
    [SerializeField] private float _delayDuration = 0.15f;
    [SerializeField] private float _respawnDelay = 1f;
    
    private Rigidbody2D _currentBallRigidbody;
    private SpringJoint2D _currentBallSpringJoint;
    private Camera _mainCamera;
    private bool isDragging;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        SpawnBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentBallRigidbody == null)
            return;

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }
            isDragging = false;
            return;
        }

        isDragging = true;
        _currentBallRigidbody.isKinematic = true;
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(touchPosition);

        _currentBallRigidbody.position = worldPosition;
    }

    private void LaunchBall()
    {
        _currentBallRigidbody.isKinematic = false;
        _currentBallRigidbody = null;

        Invoke(nameof(DetachBall), _delayDuration);
    }

    private void DetachBall()
    {
        _currentBallSpringJoint.enabled = false;
        _currentBallSpringJoint = null;

        Invoke(nameof(SpawnBall), _respawnDelay);
    }

    private void SpawnBall()
    {
        GameObject ballInstance = Instantiate(_ballPrefab, _pivot.position, Quaternion.identity);

        _currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        _currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();
        _currentBallSpringJoint.connectedBody = _pivot;

    }
}
