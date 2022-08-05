using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartMenu : MonoBehaviour
{
   public void ModeA()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
