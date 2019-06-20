using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeIcon()
    {
        Debug.Log("you clicked change icon!");
        //startcoror()
        
    }

    IEnumerator ChangeIconEnum()
    {
        //yield Webreq.changeicon();
        //change you icon locally
        yield return null;
    }
}
