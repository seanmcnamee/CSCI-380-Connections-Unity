using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	public void switchfromdropdown()
	{
		SceneManager.LoadScene(dropdown.OptionData().text);
	}
}
