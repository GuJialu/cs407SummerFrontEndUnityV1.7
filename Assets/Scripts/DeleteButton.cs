using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public void DeletePanel()
    {
        Destroy(transform.parent.gameObject);
    }
}
