using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float swayAmount = 10f; 
    [SerializeField] private float swaySpeed = 2f;

    private float _startY;
    private Quaternion _startRotation;
    void Start()
    {
        _startY = transform.position.y;
        _startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        Quaternion targetRotation = _startRotation * Quaternion.Euler(0, 0, offset);
        
        transform.position += Vector3.up * speed * Time.deltaTime;
        
        transform.rotation = targetRotation;
    }
}
