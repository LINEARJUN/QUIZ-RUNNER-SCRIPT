using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadLevel(string name)
    {
        LoadingManager.instance.QuickLevel(name);
    }
}
