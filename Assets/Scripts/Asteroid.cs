using UnityEngine;
using UnityEngine.Events;

public class Asteroid : SpaceObject
{
    [SerializeField] private AudioClip _explosionSound;
    public event UnityAction<Asteroid, bool> DestructionAsteroid;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>()) DestroyAsteroid(true);
        else DestroyAsteroid(false);
    }
    private void DestroyAsteroid(bool earnScore)
    {
        base.DestroySpaceObject();
        DestructionAsteroid?.Invoke(this, earnScore);
        AudioSource.PlayClipAtPoint(_explosionSound, transform.position);
    }
}
