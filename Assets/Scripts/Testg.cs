using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testg : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject loginAndSignUpPanel = (GameObject)Resources.Load("Prefabs/LoginAndSignUp Panel");
        //LoginAndSignUp loginAndSignUp = loginAndSignUpPanel.GetComponent<LoginAndSignUp>();
        Debug.Log(loginAndSignUpPanel.GetComponent<LoginAndSignUp>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
