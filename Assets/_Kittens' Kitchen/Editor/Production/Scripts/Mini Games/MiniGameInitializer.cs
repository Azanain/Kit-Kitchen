using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameInitializer : MonoBehaviour
{
    [SerializeField] private List<GameObject> miniGames;
    public List<GameObject> MiniGames => miniGames;
}
