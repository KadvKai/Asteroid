using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UFO : SpaceObject
{
    [SerializeField] private Bullet _bullet;
    private SpaceShip _spaceShip;
    private float _currentTimeToShot;
    private Pool _bulletsPool;
    public event UnityAction<bool> DestructionUFO;

    private void Awake()
    {
        _spaceShip = FindObjectOfType<SpaceShip>();
        _bulletsPool = new Pool(_bullet, _spaceShip.Speed);
    }
    private void OnEnable()
    {
        _currentTimeToShot = Random.Range(2, 5);
    }

    private void Update()
    {
        if (_currentTimeToShot > 0) _currentTimeToShot -= Time.deltaTime;
        else 
        {
            _currentTimeToShot = Random.Range(2, 5);
            var bullet = _bulletsPool.Spawn();
            bullet.transform.position=transform.position;
            bullet.transform.right = _spaceShip.transform.position - transform.position;
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>()) DestroyUFO(true);
        else DestroyUFO(false);
    }
    private void DestroyUFO(bool earnScore)
    {
        //base.DestroySpaceObject();
        DestructionUFO?.Invoke(earnScore);
        Debug.Log("UFO Destroy");
    }
}
