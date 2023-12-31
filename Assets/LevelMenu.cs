using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;

    private void Awake()
    {
        //PlayerPrefs.SetInt("UnlockedLevel", 1);
      //  PlayerPrefs.SetInt("ReachedIndex", 1);
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
        
    }
    

    public void LoadLevel(int levelId)
    {
        string LevelName = "Level " + levelId;
        SceneManager.LoadScene(LevelName);
    }
}
