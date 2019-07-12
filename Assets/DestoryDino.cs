using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryDino : MonoBehaviour
{
    public GameObject dino;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Rectangle(Clone)")
        {
            Destroy(dino);
            Time.timeScale = 0;
        }
    }
}
