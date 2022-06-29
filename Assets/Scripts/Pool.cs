using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private readonly SpaceObject _spaceObject;
    private readonly Stack<SpaceObject> _spaceObjects = new();
    public Pool(SpaceObject spaceObject)
    {
        _spaceObject = spaceObject;
    }

    public SpaceObject Spawn()
    {
        if (_spaceObjects.Count==0) _spaceObjects.Push(GameObject.Instantiate(_spaceObject));
        var spaceObject=_spaceObjects.Pop();
        spaceObject.gameObject.SetActive(true);
        spaceObject.Destruction += SpaceObjectDestruction;
        return spaceObject;
    }

    private void SpaceObjectDestruction(SpaceObject spaceObject)
    {
        spaceObject.Destruction -= SpaceObjectDestruction;
        spaceObject.gameObject.SetActive(false);
        _spaceObjects.Push(spaceObject);

    }
}
