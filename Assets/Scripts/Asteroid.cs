using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Asteroid : SpaceObject
{
    public enum AsteroidType { BigAsteroid , MediumAsteroid, SmallAsteroid }
    [SerializeField] private AsteroidType _asteroidType;
    public event UnityAction<Asteroid, AsteroidType> DestructionAsteroid;
    override protected void DestroySpaceObject() 
    {
        base.DestroySpaceObject();
        DestructionAsteroid?.Invoke(this, _asteroidType);
        Debug.Log("Asteroid Destroy");
    }
}
