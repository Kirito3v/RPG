using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public new GameObject camera;

    private void Awake()
    {
        Instance = Instance == null ? this : DestroyAndReturnNull();
    }

    private GameManager DestroyAndReturnNull()
    {
        Destroy(gameObject);
        return null;
    }
}
