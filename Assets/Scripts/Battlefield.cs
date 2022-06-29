using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battlefield : MonoBehaviour
{
    [SerializeField] private SpaceObject _bigAsteroid;
    [SerializeField] private SpaceObject _mediumAsteroid;
    [SerializeField] private SpaceObject _smallAsteroid;
    private Pool _bigAsteroidPool;
    private Pool _mediumAsteroidPool;
    private Pool _smallAsteroidPool;
    private float _fieldHeight;
    private float _fieldWidth;

    private void Awake()
    {
        _bigAsteroidPool = new Pool(_bigAsteroid);
        _mediumAsteroidPool = new Pool(_mediumAsteroid);
        _smallAsteroidPool = new Pool(_smallAsteroid);
    }
    private void Start()
    {
        var camera = Camera.main;
        _fieldHeight = camera.orthographicSize*2;
        _fieldWidth = _fieldHeight * camera.aspect;
        SpawnAsteroid(2);
    }

    private void SpawnAsteroid(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            var asteroid = _bigAsteroidPool.Spawn();
            asteroid.transform.SetPositionAndRotation(new Vector2(Random.Range(0, _fieldWidth), Random.Range(0, _fieldHeight)), Quaternion.Euler(0,0, Random.Range(0, 360)));
            //gameObject.SetActive(true);
        }
    }
}
