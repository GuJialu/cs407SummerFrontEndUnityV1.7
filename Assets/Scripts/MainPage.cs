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

    }

    public void ToModEditor()
    {

    }

    public void ToWorkShop()
    {
        SceneManager.LoadScene("WorkShopSence");
    }
}
