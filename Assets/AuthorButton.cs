using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthorButton : MonoBehaviour
{
    public GameObject profilePanelPrefab;
    Transform canvasTrans;

    public void Start()
    {
        canvasTrans = GetComponentInParent<Canvas>().transform;
    }

    public void ShowAuthorProfile()
    {
        GameObject profilePanel = Instantiate(profilePanelPrefab, canvasTrans);
    }
}
