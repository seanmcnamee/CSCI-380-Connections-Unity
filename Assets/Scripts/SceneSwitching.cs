using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSwitching : MonoBehaviour
{
    [SerializeField]
    private string menuSwitch = null;
    [SerializeField]
    private Dropdown dropdown = null;

    public void SwitchToMenu()
    {
        SceneManager.LoadScene(menuSwitch);
    }

    public void SwitchFromDropdown() {
        Debug.Log("Selected: " + dropdown.captionText.text);
        //Only load valid scenes
        if (dropdown.value > 0) {
            SceneManager.LoadScene(dropdown.captionText.text);
        }
    }
}
