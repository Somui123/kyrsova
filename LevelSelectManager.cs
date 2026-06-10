using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public void OnLVL1Click()
    {
        SceneManager.LoadScene("Level1");
    }

        public void OnToMenuClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
