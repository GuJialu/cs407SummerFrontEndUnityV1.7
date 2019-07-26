using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToGame()
    {
        SceneManager.LoadScene("DummyGame");
    }

    public void ToModEditor()
    {
        SceneManager.LoadScene("game");
    }

    public void ToWorkShop()
    {
        SceneManager.LoadScene("WorkShopSence");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
