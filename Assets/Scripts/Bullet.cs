using UnityEngine;

public class Bullet : SpaceObject
{
    [SerializeField] private AudioClip _fireSound;

    private void OnEnable()
    {
        AudioSource.PlayClipAtPoint(_fireSound, transform.position);
        CancelInvoke();
        Invoke(nameof(DestroySpaceObject), _fieldWidth / _speed);
    }
}
