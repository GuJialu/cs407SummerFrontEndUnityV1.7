using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginAndSignUp : MonoBehaviour
{
    public GameObject signUpPanel;
    public GameObject loginPanel;

    //render the signUpPanel in the front
    public void ShowSignUpPanel()
    {
        signUpPanel.transform.SetAsLastSibling();
    }

    //render the loginPanel in the front
    public void ShowLoginPanel()
    {
        loginPanel.transform.SetAsLastSibling();
    }

    //send the sign up requset
    public void RequestSignUp()
    {

    }

    //send the login request
    public void RequestLogin()
    {

    }
}
