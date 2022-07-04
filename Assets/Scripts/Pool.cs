using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private readonly SpaceObject _spaceObject;
    private readonly float _spaceObjectSpeed;
    private readonly Stack<SpaceObject> _spaceObjects = new Stack<SpaceObject>();
    private readonly List<SpaceObject> _spaceObjectsOnField = new List<SpaceObject>();
    private readonly List<SpaceObject> _objects = new List<SpaceObject>();
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
        _spaceObjectsOnField.Add(spaceObject);
        spaceObject.gameObject.SetActive(true);
        spaceObject.Destruction += SpaceObjectDestruction;
        return spaceObject;
    }

    public void HideAll()
    {
        if (_spaceObjectsOnField.Count != 0)
        {
            _objects.Clear();
            _objects.AddRange(_spaceObjectsOnField);
            foreach (var obj in _objects)
            {
                SpaceObjectDestruction(obj);
            }
        }
    }

    private void SpaceObjectDestruction(SpaceObject spaceObject)
    {
        spaceObject.Destruction -= SpaceObjectDestruction;
        spaceObject.gameObject.SetActive(false);
        _spaceObjects.Push(spaceObject);
        _spaceObjectsOnField.Remove(spaceObject);
    }
}
