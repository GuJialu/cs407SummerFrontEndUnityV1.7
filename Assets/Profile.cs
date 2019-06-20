using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoadUserInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadUserInfo()
    {
        // Load User Icon

        // Load User Description
    }

    public void UploadButtonPressed()
    {
        // Go to Uploads page
        Debug.Log("Pressed Upload Button in Profile Page");
    }

    public void NextPage()
    {

        Debug.Log("Next Page");
    }

    public void PreviousPage()
    {
        Debug.Log("Previous Page");
    }
}
