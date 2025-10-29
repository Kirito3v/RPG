using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    #region Skills
    public DashSkill dashSkill { get; private set; }
    #endregion

    private void Awake()
    {
        Instance = Instance == null ? this : DestroyAndReturnNull();
    }

    private SkillManager DestroyAndReturnNull()
    {
        Destroy(gameObject);
        return null;
    }

    private void Start()
    {
        #region Skills
        dashSkill = GetComponent<DashSkill>();
        #endregion
    }
}
