using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpaceObject : MonoBehaviour
{
    [SerializeField] protected float _speed;
    private float _fieldHeight;
    protected float _fieldWidth;
    public event UnityAction<SpaceObject> Destruction;

    private void Awake()
    {
        var camera = Camera.main;
        _fieldHeight = camera.orthographicSize * 2;
        _fieldWidth = _fieldHeight * camera.aspect;
    }
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        var positionX = transform.position.x + _speed * Time.deltaTime * transform.right.x;
        positionX = Mathf.Repeat(positionX, _fieldWidth);
        var positionY = transform.position.y + _speed * Time.deltaTime * transform.right.y;
        positionY = Mathf.Repeat(positionY, _fieldHeight);
        transform.position = new Vector2(positionX, positionY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroySpaceObject();
    }
    protected void DestroySpaceObject()
    { 
        //gameObject.SetActive (false);
        Destruction?.Invoke(this);
    }

    
}
