using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SpaceObject
{
    [SerializeField] private AudioClip _fireSound;

    private void OnEnable()
    {
        AudioSource.PlayClipAtPoint(_fireSound,transform.position);
        Invoke(nameof(DestroySpaceObject), _fieldWidth/ _speed);
    }
}
