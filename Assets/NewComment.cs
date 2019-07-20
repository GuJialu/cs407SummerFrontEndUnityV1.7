using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



public class NewComment : MonoBehaviour
{
    public InputField commentInput;
    public GameObject commentPrefab;

    FileDetailView fileDetailView;

    // Start is called before the first frame update
    void Start()
    {
        fileDetailView = GetComponentInParent<FileDetailView>();
    }

}
