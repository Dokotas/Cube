using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float minScale, maxScale, minSpeed, maxSpeed;
    [SerializeField] private int maxChangeIterations;
    private int _changeIterations;
    private float _speed;

    private Rigidbody _rb;
    private VariableJoystick _joystick;
    private GameManager _gameManager;
    private Vector3 _startPosition;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _joystick = FindObjectOfType<VariableJoystick>();
        _gameManager = FindObjectOfType<GameManager>();
        _startPosition = transform.position;

        _changeIterations = maxChangeIterations / 2;
    }

    void FixedUpdate()
    {
        var velocity = new Vector3(_joystick.Horizontal * _speed, _rb.velocity.y, _joystick.Vertical * _speed);
        _rb.velocity = velocity;
    }

    private void OnCollisionEnter(Collision other)
    {
        var otherCollider = other.collider;

        if (otherCollider.CompareTag("Killer") || otherCollider.CompareTag("DeathArea"))
            Death();

        if (_changeIterations > 0 && _changeIterations < maxChangeIterations)
        {
            if (otherCollider.CompareTag("Bigger"))
                _changeIterations++;
            if (otherCollider.CompareTag("Smaller"))
                _changeIterations--;

            ChangeSpeedAndScale();
        }
    }

    private void Death()
    {
        transform.position = _startPosition;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        _changeIterations = maxChangeIterations / 2;
        
        _gameManager.SelectLevel(1);
        ChangeSpeedAndScale();
    }

    private void ChangeSpeedAndScale()
    {
        var t = (float) _changeIterations / maxChangeIterations;
        transform.localScale = Vector3.one * (minScale + (maxScale - minScale) * t);
        _speed = maxSpeed - (maxSpeed - minSpeed) * t;
    }
}