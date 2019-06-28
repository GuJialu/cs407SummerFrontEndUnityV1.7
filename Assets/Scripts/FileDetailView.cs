using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileDetailView : MonoBehaviour
{
    public Image fileCoverImage;
    public Text authorName;
    public Text description;
    public Text downloads;
    public Text likes;
    public Text date;

    public void init(FileOverview fileOverview)
    {
        fileCoverImage.sprite = fileOverview.fileCoverImage.sprite;
        authorName.text = fileOverview.authorName.text;
        description.text = fileOverview.description.text;
        downloads.text = fileOverview.downloads.text;
        likes.text = fileOverview.likes.text;
        date.text = fileOverview.date.text;
    }
}
