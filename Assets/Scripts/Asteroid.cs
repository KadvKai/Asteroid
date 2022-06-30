using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Asteroid : SpaceObject
{
    public event UnityAction<Asteroid, bool> DestructionAsteroid;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>()) DestroyAsteroid(true);
        else DestroyAsteroid(false);
    }
    private void DestroyAsteroid(bool earnScore)
    {
        base.DestroySpaceObject();
        _audio.Play();
        DestructionAsteroid?.Invoke(this, earnScore);
    }
}
