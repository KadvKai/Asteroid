using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private SpaceObject _bullet;
    [SerializeField] private int _bulletsInMinute=3;
    [SerializeField] Transform _shotPoint;
    private Pool _bulletsPool;
    private float _timeToShot;
    private float _currentTimeToShot;

    private void Awake()
    {
        _timeToShot = 1 / _bulletsInMinute;
        _bulletsPool = new Pool(_bullet);
    }

    private void Start()
    {
        var camera = Camera.main;
        transform.position = new Vector2(camera.transform.position.x, camera.transform.position.y);
    }
    private void Update()
    {
        if (_currentTimeToShot > 0) _currentTimeToShot -= Time.deltaTime;
        if (_currentTimeToShot<=0&&Input.GetKeyDown(KeyCode.Space))
        {
            _currentTimeToShot = _timeToShot;
            var bullet = _bulletsPool.Spawn();
            bullet.transform.SetPositionAndRotation(_shotPoint.position, _shotPoint.rotation);
        }

    }
}
