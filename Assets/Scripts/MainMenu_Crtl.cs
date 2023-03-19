using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Crtl : MonoBehaviour
{
    public void PlayGame(){
       
       SceneManager.LoadScene("Gameplay");

    }
}
