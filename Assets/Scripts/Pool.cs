using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private readonly SpaceObject _spaceObject;
    private readonly float _spaceObjectSpeed;
    private readonly Stack<SpaceObject> _spaceObjects = new Stack<SpaceObject>();
    public Pool(SpaceObject spaceObject)
    {
        _spaceObject = spaceObject;
    }
    public Pool(SpaceObject spaceObject, float speed) : this(spaceObject)
    {
        _spaceObjectSpeed = speed;
    }

    public SpaceObject Spawn()
    {
        if (_spaceObjects.Count == 0)
        {
            var newSpaceObject = GameObject.Instantiate(_spaceObject);
            newSpaceObject.SetSpeed(_spaceObjectSpeed);
            _spaceObjects.Push(newSpaceObject);
        }
        var spaceObject = _spaceObjects.Pop();
        spaceObject.gameObject.SetActive(true);
        spaceObject.Destruction += SpaceObjectDestruction;
        return spaceObject;
    }

    private void SpaceObjectDestruction(SpaceObject spaceObject)
    {
        Debug.Log("Pool");
        spaceObject.Destruction -= SpaceObjectDestruction;
        spaceObject.gameObject.SetActive(false);
        _spaceObjects.Push(spaceObject);

    }
}
