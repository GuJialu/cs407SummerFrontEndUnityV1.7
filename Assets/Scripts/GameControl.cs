using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public GameObject dino;
    public GameObject rect;

    public GameObject StartButton;
    public GameObject ReStartButton;
    public GameObject ExitButton;
    public GameObject ScoreText;

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
}
