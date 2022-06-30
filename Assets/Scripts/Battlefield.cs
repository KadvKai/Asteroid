using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Battlefield : MonoBehaviour
{
    [SerializeField] private UFO _uFO;
    [SerializeField] private int _uFOScore;
    [SerializeField] private float _minSpeedAsteroid;
    [SerializeField] private float _maxSpeedAsteroid;
    [SerializeField] private Asteroid _bigAsteroid;
    [SerializeField] private int _bigAsteroidScore;
    [SerializeField] private SpaceObject _mediumAsteroid;
    [SerializeField] private int _mediumAsteroidScore;
    [SerializeField] private SpaceObject _smallAsteroid;
    [SerializeField] private int _smallAsteroidScore;
    [SerializeField] private TMP_Text _scoreIndicator;
    private Pool _bigAsteroidPool;
    private Pool _mediumAsteroidPool;
    private Pool _smallAsteroidPool;
    private float _fieldHeight;
    private float _fieldWidth;
    private readonly List<Asteroid> _asteroids = new List<Asteroid>();
    private readonly List<Asteroid> _bigAsteroids = new List<Asteroid>();
    private readonly List<Asteroid> _mediumAsteroids = new List<Asteroid>();
    private readonly List<Asteroid> _smallAsteroids = new List<Asteroid>();
    private int _quantityBigAsteroid = 2;
    private int _score;

    private void Awake()
    {
        _uFO.gameObject.SetActive( false);
        _uFO.DestructionUFO += DestructionUFO;
        _bigAsteroidPool = new Pool(_bigAsteroid);
        _mediumAsteroidPool = new Pool(_mediumAsteroid);
        _smallAsteroidPool = new Pool(_smallAsteroid);
    }


    private void Start()
    {
        var camera = Camera.main;
        _fieldHeight = camera.orthographicSize * 2;
        _fieldWidth = _fieldHeight * camera.aspect;
        SpawnBigAsteroid();
        Invoke(nameof(SpawnUFO), Random.Range(20f, 40f));
    }

    private void SpawnUFO()
    {
        _uFO.gameObject.SetActive(true);
        bool leftPosition;
        leftPosition = Random.Range(-1, 1f) < 0;
        _uFO.transform.localScale = new Vector3(leftPosition ? 1 : -1, 1, 1);
        _uFO.transform.position = new Vector2(leftPosition ? 0 : _fieldWidth, Random.Range(_fieldHeight * 0.2f, _fieldHeight * 0.8f));
    }
    private void DestructionUFO(bool earnScore)
    {
        if (earnScore) ChangeScore(_uFOScore);
        _uFO.gameObject.SetActive(false);
        Invoke(nameof(SpawnUFO), Random.Range(20f, 40f));
    }

    private void SpawnBigAsteroid()
    {
        for (int i = 0; i < _quantityBigAsteroid; i++)
        {
            var asteroid = _bigAsteroidPool.Spawn() as Asteroid;
            asteroid.transform.SetPositionAndRotation(new Vector2(Random.Range(0, _fieldWidth), Random.Range(0, _fieldHeight)), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            asteroid.SetSpeed(Random.Range(_minSpeedAsteroid, _maxSpeedAsteroid));
            _asteroids.Add(asteroid);
            _bigAsteroids.Add(asteroid);
            asteroid.DestructionAsteroid += DestructionAsteroid;
        }
        _quantityBigAsteroid++;
    }

    private void DestructionAsteroid(Asteroid asteroid, bool earnScore)
    {
        if (_bigAsteroids.Contains(asteroid))
        {
            if (earnScore) ChangeScore(_bigAsteroidScore);
            _bigAsteroids.Remove(asteroid);
            SpawnMediumOrSmallAsteroid(asteroid, true);
        }
        if (_mediumAsteroids.Contains(asteroid))
        {
            if (earnScore) ChangeScore(_mediumAsteroidScore);
            _mediumAsteroids.Remove(asteroid);
            SpawnMediumOrSmallAsteroid(asteroid, false);
        }
        if (_smallAsteroids.Contains(asteroid))
        {
            if (earnScore) ChangeScore(_smallAsteroidScore);
            _smallAsteroids.Remove(asteroid);
        }
        _asteroids.Remove(asteroid);
        CheckAsteroids();
    }

    private void SpawnMediumOrSmallAsteroid(Asteroid oldAsteroid, bool itMediumAsteroid)
    {
        var sped = Random.Range(_minSpeedAsteroid, _maxSpeedAsteroid);
        Asteroid[] asteroids = new Asteroid[2];
        for (int i = 0; i < asteroids.Length; i++)
        {
            if (itMediumAsteroid)
            {
                asteroids[i] = _mediumAsteroidPool.Spawn() as Asteroid;
                _mediumAsteroids.Add(asteroids[i]);
            }
            else
            {
                asteroids[i] = _smallAsteroidPool.Spawn() as Asteroid;
                _smallAsteroids.Add(asteroids[i]);
            }
            asteroids[i].transform.SetPositionAndRotation(oldAsteroid.transform.position, Quaternion.Euler(0, 0, oldAsteroid.transform.rotation.eulerAngles.z - Mathf.Pow(-1, i) * 45));
            asteroids[i].SetSpeed(sped);
            _asteroids.Add(asteroids[i]);
            asteroids[i].DestructionAsteroid += DestructionAsteroid;
        }
    }

    private void CheckAsteroids()
    {
        if (_asteroids.Count == 0) Invoke(nameof(SpawnBigAsteroid), 2);
    }

    private void ChangeScore(int score)
    {
        _score += score;
        _scoreIndicator.text = _score.ToString();
    }
}
