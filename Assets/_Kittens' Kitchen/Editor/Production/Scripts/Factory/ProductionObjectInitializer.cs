using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProductionObjectInitializer : MonoBehaviour
{
    [Inject] private ProductionObjectFactory _productionObjectFactory;

    private void Start()
    {
        Debug.Log("Starting Factory Production...");
        MiniGameInitializer miniGameInitializer = _productionObjectFactory.Create();

        Debug.Log("Objects from Factory Production were created");
    }
}
