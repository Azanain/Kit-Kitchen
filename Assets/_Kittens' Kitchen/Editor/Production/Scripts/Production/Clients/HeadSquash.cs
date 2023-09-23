using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSquash : MonoBehaviour
{
    private Vector3 _initialScale;
    private float _squashSpeed;
    private float _squashAmount;

    private ClientMovement _clientMovement;

    void Start()
    {
        _initialScale = transform.localScale;
        
        _clientMovement = GetComponentInParent<ClientMovement>();
        _squashAmount = _clientMovement.GetSquashAmount();
        _squashSpeed = _clientMovement.GetSquashSpeed();
    }
    
    void Update()
    {
        float squash = Mathf.Abs(Mathf.Sin(Time.time * _squashSpeed));
        Vector3 newScale = _initialScale + Vector3.up * squash * _squashAmount;
        
        transform.localScale = newScale;
    }
}
