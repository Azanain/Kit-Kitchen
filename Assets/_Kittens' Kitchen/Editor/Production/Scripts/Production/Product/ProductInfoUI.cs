using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductInfoUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private TextMeshProUGUI count;

    private ProductData _productData;
    public void DisplayProduct(ProductData productData)
    {
        label.text = productData.Label;
        count.text = productData.Count.ToString();
        icon.sprite = productData.Icon;
    }
}
