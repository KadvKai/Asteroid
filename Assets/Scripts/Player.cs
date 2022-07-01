using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _speedRotation;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private int _bulletsInMinute = 3;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private bool _controlKeyboard;
    private Vector3 _movementDirection;
    private Pool _bulletsPool;
    private float _timeToShot;
    private float _currentTimeToShot;
    private float _fieldHeight;
    protected float _fieldWidth;
    public float Speed => _maxSpeed;

    private void Awake()
    {
        _timeToShot = 1 / _bulletsInMinute;
        _bulletsPool = new Pool(_bullet, _bulletSpeed);
    }

    private void Start()
    {
        var camera = Camera.main;
        _fieldHeight = camera.orthographicSize * 2;
        _fieldWidth = _fieldHeight * camera.aspect;
        transform.position = new Vector2(camera.transform.position.x, camera.transform.position.y);
    }
    private void Update()
    {
        if (_currentTimeToShot > 0) _currentTimeToShot -= Time.deltaTime;
        InputController();
        Movement();
    }
    private void InputController()
    {
        if (_controlKeyboard)
        {
            if (Input.GetKeyDown(KeyCode.Space)) Shot();
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow)) Rotation(transform.up);
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow)) Rotation(-transform.up);
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) _movementDirection += _acceleration * Time.deltaTime * transform.right;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) Shot();
            var direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            Rotation(direction.normalized);
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetMouseButton(1)) _movementDirection += _acceleration * Time.deltaTime * transform.right;
        }
    }

    private void Shot()
    {
        if (_currentTimeToShot <= 0)
        {
            _currentTimeToShot = _timeToShot;
            var bullet = _bulletsPool.Spawn();
            bullet.transform.SetPositionAndRotation(_shotPoint.position, _shotPoint.rotation);
        }
    }

    private void Rotation(Vector2 direction)
    {
        var delta = Vector2.SignedAngle(transform.right, direction);
        if (delta> _speedRotation*Time.deltaTime) 
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.x+ _speedRotation * Time.deltaTime);
        else if 
            (delta < -_speedRotation * Time.deltaTime) transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.x - _speedRotation * Time.deltaTime);
        else
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.x +delta);
        //transform.right = Vector3.RotateTowards(transform.right, new Vector3(direction.x, direction.y, 0), _speedRotation * Time.deltaTime,0);
    }

    private void Movement()
    {
        if (_movementDirection.sqrMagnitude > _maxSpeed * _maxSpeed) _movementDirection = _movementDirection.normalized * _maxSpeed;

        var positionX = transform.position.x + Time.deltaTime * _movementDirection.x;
        positionX = Mathf.Repeat(positionX, _fieldWidth);
        var positionY = transform.position.y + Time.deltaTime * _movementDirection.y;
        positionY = Mathf.Repeat(positionY, _fieldHeight);
        transform.position = new Vector2(positionX, positionY);
        //transform.position += _movementDirection;
    }
}
