using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] protected GameObject[] Tabs;

    protected virtual void Start()
    {
        if (Tabs.Length > 0)
            SwitchTo(Tabs[0]);
    }

    public virtual void SwitchTo(GameObject menu)
    {
        for (int i = 0; i < Tabs.Length; i++)
        {
            Tabs[i].gameObject.SetActive(false);
        }

        if (menu != null)
        {
            menu.SetActive(true);
        }
    }
}
