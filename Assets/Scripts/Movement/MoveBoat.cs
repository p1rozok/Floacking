using UnityEngine;

public class MoveBoat : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Rigidbody2D rb;

    private Vector3 _acceleration;
    private Vector3 _velocity;
    [SerializeField] private float fullStopSpeed;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private float stoppingPower = 0.01f;
    [SerializeField] private float maxSpeed = 0.15f;

    private Vector3 _direction = new Vector3();


    private void FixedUpdate()
    {
        if (controller.Docked)
        {
            _velocity = Vector3.zero;
            return;
        }
        
        HarvestInput();

        ApplyMove();
        ApplyRotation();
        ApplyStoppingPower();
        ApplyAcceleration();
        ApplyMaxSpeed();
        ApplyFullStop();
        ResetAcceleration();
    }

    private void ApplyMove()
    {
        ApplyForce(_direction);
    }

    private void ApplyMaxSpeed()
    {
        if (_velocity.magnitude > maxSpeed)
        {
            _velocity = _velocity.normalized * maxSpeed;
        }
    }

    private void ApplyFullStop()
    {
        if (_velocity.magnitude <= fullStopSpeed)
        {
            _velocity = Vector3.zero;
        }
    }

    private void ApplyRotation()
    {
        if (_velocity.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, (_velocity.x > 0 ? _velocity.magnitude : -_velocity.magnitude) * 20);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1);
        }
    }

    private void ApplyForce(Vector3 force)
    {
        _acceleration += force / rb.mass;
    }

    private void ApplyStoppingPower()
    {
        var friction = -_velocity.normalized * stoppingPower;
        ApplyForce(friction);
    }

    private void ApplyAcceleration()
    {
        _velocity += _acceleration;
        rb.MovePosition(transform.position + _velocity);
    }

    private void ResetAcceleration()
    {
        _acceleration = Vector3.zero;
    }

    private void HarvestInput()
    {
        switch (controller.transform.localPosition.x)
        {
            case >= 0.30f:
                _direction.Set(speed, 0, 0);
                break;
            case <= -0.10f:
                _direction.Set(-speed, 0, 0);
                break;
            default:
                _direction.Set(0, 0, 0);
                break;
        }
    }
   /* public void SetIsActive(bool value)
    {
        isActive = value;
    }*/
}

