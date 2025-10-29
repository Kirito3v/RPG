using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;
    public GameObject camera;

    #region Skills
    public Skill skill { get; private set; }
    public DashSkill dashSkill { get; private set; }
    #endregion

    private void Awake()
    {
        Instance = Instance == null ? this : DestroyAndReturnNull();
    }

    private GameManager DestroyAndReturnNull()
    {
        Destroy(gameObject);
        return null;
    }

    private void Start()
    {
        #region Skills
        skill = GetComponent<Skill>();
        dashSkill = GetComponentInChildren<DashSkill>();
        #endregion
    }
}
