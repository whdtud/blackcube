using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    public PlayerController Player { get; private set; }

    public static WorldManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
            return;

        Instance = this;
    }
}
