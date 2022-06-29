using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SpaceObject
{
    private void OnEnable()
    {
        Invoke(nameof(DestroySpaceObject), _fieldWidth/ _speed);
    }
}
