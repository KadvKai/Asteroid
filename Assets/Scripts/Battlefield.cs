using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battlefield : MonoBehaviour
{
    [SerializeField] private float _minSpeedAsteroid;
    [SerializeField] private float _maxSpeedAsteroid;
    [SerializeField] private Asteroid _bigAsteroid;
    //[SerializeField] private SpaceObject _mediumAsteroid;
    //[SerializeField] private SpaceObject _smallAsteroid;
    private Pool _bigAsteroidPool;
    //private Pool _mediumAsteroidPool;
    //private Pool _smallAsteroidPool;
    private float _fieldHeight;
    private float _fieldWidth;
    private readonly List<SpaceObject> _spaceObjects = new List<SpaceObject>();

    private void Awake()
    {
        _bigAsteroidPool = new Pool(_bigAsteroid);
        //_mediumAsteroidPool = new Pool(_mediumAsteroid);
        //_smallAsteroidPool = new Pool(_smallAsteroid);
    }
    private void Start()
    {
        var camera = Camera.main;
        _fieldHeight = camera.orthographicSize * 2;
        _fieldWidth = _fieldHeight * camera.aspect;
        SpawnBigAsteroid(2);
    }

    private void SpawnBigAsteroid(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            var spaceObject = _bigAsteroidPool.Spawn();
            spaceObject.transform.SetPositionAndRotation(new Vector2(Random.Range(0, _fieldWidth), Random.Range(0, _fieldHeight)), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            spaceObject.SetSpeed(Random.Range(_minSpeedAsteroid, _maxSpeedAsteroid));
            _spaceObjects.Add(spaceObject);
            var asteroid = spaceObject as Asteroid;
            asteroid.DestructionAsteroid += DestructionBigAsteroid;
        }
    }

    private void DestructionBigAsteroid(Asteroid asteroid, Asteroid.AsteroidType asteroidType)
    {
        Debug.Log("DestructionBigAsteroid");
    }
}
