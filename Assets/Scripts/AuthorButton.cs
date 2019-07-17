using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthorButton : MonoBehaviour
{
    public GameObject profilePanelPrefab;
    public string email;
    Transform canvasTrans;

    public void Start()
    {
        canvasTrans = GetComponentInParent<Canvas>().transform;
    }

    public void ShowAuthorProfile()
    {
        if (string.IsNullOrEmpty(email))
        {
            return;
        }
        GameObject profilePage = Instantiate(profilePanelPrefab, canvasTrans);
        profilePage.GetComponent<Profile>().Init(email);
    }
}
