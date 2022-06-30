using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Battlefield : MonoBehaviour
{
    [SerializeField] private FlyingSaucer _flyingSaucer;
    [SerializeField] private int _flyingSaucerScore;
    [SerializeField] private float _minSpeedAsteroid;
    [SerializeField] private float _maxSpeedAsteroid;
    [SerializeField] private Asteroid _largeAsteroid;
    [SerializeField] private int _largeAsteroidScore;
    [SerializeField] private SpaceObject _mediumAsteroid;
    [SerializeField] private int _mediumAsteroidScore;
    [SerializeField] private SpaceObject _smallAsteroid;
    [SerializeField] private int _smallAsteroidScore;
    [SerializeField] private TMP_Text _scoreIndicator;
    private Pool _largeAsteroidPool;
    private Pool _mediumAsteroidPool;
    private Pool _smallAsteroidPool;
    private float _fieldHeight;
    private float _fieldWidth;
    private readonly List<Asteroid> _asteroids = new List<Asteroid>();
    private readonly List<Asteroid> _largeAsteroids = new List<Asteroid>();
    private readonly List<Asteroid> _mediumAsteroids = new List<Asteroid>();
    private readonly List<Asteroid> _smallAsteroids = new List<Asteroid>();
    private int _quantityLargeAsteroid = 2;
    private int _score;

    private void Awake()
    {
        _flyingSaucer.gameObject.SetActive(false);
        _flyingSaucer.DestructionFlyingSaucer += DestructionFlyingSaucer;
        _largeAsteroidPool = new Pool(_largeAsteroid);
        _mediumAsteroidPool = new Pool(_mediumAsteroid);
        _smallAsteroidPool = new Pool(_smallAsteroid);
    }


    private void Start()
    {
        var camera = Camera.main;
        _fieldHeight = camera.orthographicSize * 2;
        _fieldWidth = _fieldHeight * camera.aspect;
        SpawnLargeAsteroid();
        Invoke(nameof(SpawnFlyingSaucer), Random.Range(20f, 40f));
    }

    private void SpawnFlyingSaucer()
    {
        _flyingSaucer.gameObject.SetActive(true);
        bool leftPosition;
        leftPosition = Random.Range(-1, 1f) < 0;
        _flyingSaucer.transform.localScale = new Vector3(leftPosition ? 1 : -1, 1, 1);
        _flyingSaucer.transform.position = new Vector2(leftPosition ? 0 : _fieldWidth, Random.Range(_fieldHeight * 0.2f, _fieldHeight * 0.8f));
    }
    private void DestructionFlyingSaucer(bool earnScore)
    {
        if (earnScore) ChangeScore(_flyingSaucerScore);
        _flyingSaucer.gameObject.SetActive(false);
        Invoke(nameof(SpawnFlyingSaucer), Random.Range(20f, 40f));
    }

    private void SpawnLargeAsteroid()
    {
        for (int i = 0; i < _quantityLargeAsteroid; i++)
        {
            var asteroid = _largeAsteroidPool.Spawn() as Asteroid;
            asteroid.transform.SetPositionAndRotation(new Vector2(Random.Range(0, _fieldWidth), Random.Range(0, _fieldHeight)), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            asteroid.SetSpeed(Random.Range(_minSpeedAsteroid, _maxSpeedAsteroid));
            _asteroids.Add(asteroid);
            _largeAsteroids.Add(asteroid);
            asteroid.DestructionAsteroid += DestructionAsteroid;
        }
        _quantityLargeAsteroid++;
    }

    private void DestructionAsteroid(Asteroid asteroid, bool earnScore)
    {
        if (_largeAsteroids.Contains(asteroid))
        {
            if (earnScore)
            {
                ChangeScore(_largeAsteroidScore);
                SpawnMediumOrSmallAsteroid(asteroid, true);
            }
            _largeAsteroids.Remove(asteroid);
        }
        if (_mediumAsteroids.Contains(asteroid))
        {
            if (earnScore)
            {
                ChangeScore(_mediumAsteroidScore);
                SpawnMediumOrSmallAsteroid(asteroid, false);
            }
            _mediumAsteroids.Remove(asteroid);
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
        if (_asteroids.Count == 0) Invoke(nameof(SpawnLargeAsteroid), 2);
    }

    private void ChangeScore(int score)
    {
        _score += score;
        _scoreIndicator.text = _score.ToString();
    }
}
