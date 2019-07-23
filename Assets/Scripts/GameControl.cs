using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.IO.Compression;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public GameObject dino;
    public GameObject rect;

    public GameObject StartButton;
    public GameObject ReStartButton;
    public GameObject ExitButton;
    public GameObject ScoreText;

    public GameObject localFileButtonPrefab;
    public GameObject FileScrollViewContent;
    public GameObject FileNameText;
    public GameObject DescriptionText;
    
    public Image fileCoverImage;

    private GameObject newRect;
    private Rigidbody2D dinoBody;
    private float thrust = 300.0f;
    private float rectmovement = 0.3f;
    private float myTime = 1.5F;
    private float genTime = 0.0f;
    public float nextJump = 1.5F;
    private float nextGen = 5.0F;

    private int score = 0;

    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;

    private string filePath;

    public GameObject modInfoPanel;


    void Awake()
    {
        dinoBody = dino.GetComponent<Rigidbody2D>();
        newRect = Instantiate(rect, rect.transform.position + Vector3.left * 10.0f, rect.transform.rotation);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameConfig.LoadMods(WebReq.objectFolderPath);

        StartButton.GetComponentInChildren<Text>().text = GameConfig.startStr;
        ReStartButton.GetComponentInChildren<Text>().text = GameConfig.reStartStr;
        ExitButton.GetComponentInChildren<Text>().text = GameConfig.exitStr;
        newRect.GetComponentInChildren<SpriteRenderer>().color = GameConfig.barColor;

        rectmovement = GameConfig.speed/100.0f;
    }

    // Update is called once per frame
    void Update()
    {
        myTime = myTime + Time.deltaTime;
        genTime = genTime + Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && myTime > nextJump)
        {
            if (dinoBody != null)
                dinoBody.AddForce(dino.transform.up * thrust);

            myTime = 0.0F;
        }

        if (genTime > nextGen)
        {
            newRect = Instantiate(rect, rect.transform.position + Vector3.left * 10.0f, rect.transform.rotation);
            newRect.GetComponentInChildren<SpriteRenderer>().color = GameConfig.barColor;

            genTime = 0.0f;
            score += 1;
        }
        else if (nextGen - genTime > 1.0f)
        {
            if (newRect != null)
                newRect.transform.position += Vector3.left * rectmovement;
        }
        else
        {
            if (newRect != null)
                Destroy(newRect);
        }

        ScoreText.GetComponent<Text>().text = "Score: " + score;
    }

    public void ReStart()
    {
        Application.LoadLevel(Application.loadedLevel);
        Time.timeScale = 1;
    }

    public void Exit()
    {
        SceneManager.LoadScene("WorkShopSence");
    }

    public void modInfoLoad()
    {
        modInfoPanel.SetActive(true);

        filePath = Application.dataPath + "/StreamingAssets/DownloadedGameFiles/";
        DirectoryInfo d = new DirectoryInfo(filePath);

        foreach (var currDirectory in d.GetDirectories())
        {
            Debug.Log(currDirectory.Name);
            GameObject fileButton = Instantiate(localFileButtonPrefab, FileScrollViewContent.transform);
            fileButton.GetComponentInChildren<Text>().text = currDirectory.Name;
            var button = fileButton.GetComponent<Button>();

            button.onClick.AddListener(delegate { FolderSelected(currDirectory.Name); });
        }
    }

    public void modInfoLoadCancel()
    {
        modInfoPanel.SetActive(false);

        foreach (Transform transform in FileScrollViewContent.transform)
        {
            Destroy(transform.gameObject);
        }
    }

    public void FolderSelected(string name)
    {
        Debug.Log(name);
        string path = filePath + name;
        string InfoPath = path + "/info";
        string desPath = InfoPath + "/des.txt";
        string coverPath = InfoPath + "/workingspace.PNG";

        FileNameText.GetComponentInChildren<Text>().text = name;
        
        string description = "";
        using (StreamReader sr = new StreamReader(desPath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                description = description + line + "\n";
            }
        }
        DescriptionText.GetComponentInChildren<Text>().text = description;

        byte[] filedata;
        if (File.Exists(coverPath))
        {
            filedata = File.ReadAllBytes(coverPath);

            Texture2D Tex2D;
            Tex2D = new Texture2D(2, 2);
            Tex2D.LoadImage(filedata);
            fileCoverImage.sprite = Sprite.Create(Tex2D, new Rect(0, 0, Tex2D.width, Tex2D.height), new Vector2(0, 0));
        }
    }
}
