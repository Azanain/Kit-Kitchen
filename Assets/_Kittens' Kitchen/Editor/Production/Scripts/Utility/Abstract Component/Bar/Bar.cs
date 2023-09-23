using System;
using UnityEngine.UI;

public class Bar : AbstractBar
{
    private void Awake()
    {
        Bar = GetComponent<Image>();
    }
}