using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SpaceObject
{
    private void OnEnable()
    {
        _audio.Play();
        Invoke(nameof(DestroySpaceObject), _fieldWidth/ _speed);
    }
}
