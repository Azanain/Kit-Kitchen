using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductItem : MonoBehaviour
{
    [SerializeField] private ProductData product;
    [SerializeField] private ProductInfoUI productUI;
    private GameObject _currentProductUIInstance;
    public static event Action OnProductSelected;
    
    private bool _isMouseDownProcessed;

    public void OnMouseDown()
    {
        if (_currentProductUIInstance == null && !_isMouseDownProcessed)
        {
            OnProductSelected += ProductSelected;
            OnProductSelected?.Invoke();
            _currentProductUIInstance = Instantiate(productUI.gameObject, transform.position + Vector3.up * 100, Quaternion.identity);
            _currentProductUIInstance.transform.SetParent(transform);

            if (productUI != null)
            {
                ProductInfoUI productInfo = _currentProductUIInstance.GetComponent<ProductInfoUI>();
                productInfo.DisplayProduct(product);
            }
        }
        else
        {
            OnProductSelected -= ProductSelected;
            Destroy(_currentProductUIInstance);
            _currentProductUIInstance = null;
        }

        _isMouseDownProcessed = false;
    }

    private void ProductSelected()
    {
        if (_currentProductUIInstance != null)
        {
            Destroy (_currentProductUIInstance);
            _currentProductUIInstance = null;
        }
        _isMouseDownProcessed = true;
    }
}
