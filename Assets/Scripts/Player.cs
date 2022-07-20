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
    private Vector3 _movementDirection;
    private Pool _bulletsPool;
    private float _timeToShot;
    private float _currentTimeToShot;
    private Camera _camera;
    private float _fieldHeight;
    protected float _fieldWidth;
    public float Speed => _maxSpeed;
    public bool ControlKeyboard;

    private void Awake()
    {
        _timeToShot = 1f / _bulletsInMinute;
        _bulletsPool = new Pool(_bullet, _bulletSpeed);
        _camera = Camera.main;
        _fieldHeight = _camera.orthographicSize * 2;
        _fieldWidth = _fieldHeight * _camera.aspect;
    }

    public void NewGame()
    {
        transform.position = new Vector2(_camera.transform.position.x, _camera.transform.position.y);
        _bulletsPool.HideAll();
    }

    private void Update()
    {
        if (_currentTimeToShot > 0) _currentTimeToShot -= Time.deltaTime;
        InputController();
        Movement();
    }
    private void InputController()
    {
        if (ControlKeyboard)
        {
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow)) Rotation(transform.up);
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow)) Rotation(-transform.up);
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) _movementDirection += _acceleration * Time.deltaTime * transform.right;
            if (Input.GetKeyDown(KeyCode.Space)) Shot();
        }
        else
        {
            var direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            Rotation(direction.normalized);
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetMouseButton(1)) _movementDirection += _acceleration * Time.deltaTime * transform.right;
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) Shot();
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
        var rotAngle = Mathf.MoveTowardsAngle(0, Vector2.SignedAngle(transform.right, direction), _speedRotation * Time.deltaTime);
        transform.Rotate(0, 0, rotAngle);
    }

    private void Movement()
    {
        if (_movementDirection.sqrMagnitude > _maxSpeed * _maxSpeed) _movementDirection = _movementDirection.normalized * _maxSpeed;

        var positionX = transform.position.x + Time.deltaTime * _movementDirection.x;
        positionX = Mathf.Repeat(positionX, _fieldWidth);
        var positionY = transform.position.y + Time.deltaTime * _movementDirection.y;
        positionY = Mathf.Repeat(positionY, _fieldHeight);
        transform.position = new Vector2(positionX, positionY);
    }
}
