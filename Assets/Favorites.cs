using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.Networking;
using System.IO.Compression;

public class Favorites : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject FavoritesScrollViewContent;
    public GameObject FavoritesEntryPrefab; // TODO switch with the prefab probably built by Jialu for directory element.

    public GameObject exitButton;
    public GameObject removeFavoriteButton;

    public Text errorMessage;

    void Start()
    {
        // Access database for favorites

        // foreach ( var element in request.json )
        // add element to content
        // add listener
    }

    public void ElementSelected(string name)
    {
        // TODO, call database for more info, redirect user or pop up with more information.
    }

    public void RemoveFromFavorites()
    {
        // TODO, call database to remove favorites from list. Reload page.
    }

    public void ConfirmRemoveFromFavorites()
    {
        // TODO be called by 'RemoveFromFavorites'

        // TODO create pop up panel.
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
