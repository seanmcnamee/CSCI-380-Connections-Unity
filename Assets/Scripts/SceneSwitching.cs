using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitching : MonoBehaviour
{
    [SerializeField]
    private string menuSwitch = null;
    //private  = null;

    public void SwitchToMenu()
    {
        SceneManager.LoadScene(menuSwitch);
    }
    
}
