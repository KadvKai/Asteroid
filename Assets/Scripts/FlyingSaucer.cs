using UnityEngine;
using UnityEngine.Events;

public class FlyingSaucer : SpaceObject
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private AudioClip _explosionSound;
    private Player _player;
    private float _currentTimeToShot;
    private Pool _bulletsPool;
    public event UnityAction<bool> DestructionFlyingSaucer;

    override protected void Awake()
    {
        base.Awake();
        _player = FindObjectOfType<Player>();
        _bulletsPool = new Pool(_bullet, _player.Speed);
        _speed = _fieldWidth / 10;
    }

    public void NewGame()
    {
        _bulletsPool.HideAll();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _currentTimeToShot = Random.Range(2, 5);
    }

    private void Update()
    {
        Move();
        if (_currentTimeToShot > 0) _currentTimeToShot -= Time.deltaTime;
        else
        {
            _currentTimeToShot = Random.Range(2, 5);
            var bullet = _bulletsPool.Spawn();
            bullet.transform.position = transform.position;
            bullet.transform.right = _player.transform.position - transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>()) DestroyFlyingSaucer(true);
        else DestroyFlyingSaucer(false);
    }
    private void DestroyFlyingSaucer(bool earnScore)
    {
        DestructionFlyingSaucer?.Invoke(earnScore);
        AudioSource.PlayClipAtPoint(_explosionSound, transform.position);
    }
}
