using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameControl : MonoBehaviour
{
    public GameObject dino;
    public GameObject rect;

    private GameObject newRect;
    private Rigidbody2D dinoBody;
    private float thrust = 300.0f;
    private float rectmovement = 0.3f;
    private float myTime = 0.0F;
    private float genTime = 0.0f;
    public float nextJump = 1.5F;
    private float nextGen = 5.0F;

    void Awake()
    {
        dinoBody = dino.GetComponent<Rigidbody2D>();
        newRect = Instantiate(rect, rect.transform.position + Vector3.left * 10.0f, rect.transform.rotation);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameConfig.LoadMods(WebReq.objectFolderPath);
        //GameConfig.LoadMods(WebReq.downloadFolderPath);
        rectmovement = GameConfig.speed;

        if (rectmovement > 10.0f)
            rectmovement = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        myTime = myTime + Time.deltaTime;
        genTime = genTime + Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && myTime > nextJump)
        {
            dinoBody.AddForce(dino.transform.up * thrust);

            myTime = 0.0F;
        }

        if (genTime > nextGen)
        {
            newRect = Instantiate(rect, rect.transform.position + Vector3.left * 10.0f, rect.transform.rotation);

            genTime = 0.0f;
        }
        else if (nextGen - genTime > 1.0f)
        {
            if (newRect != null)
                newRect.transform.position += Vector3.left * rectmovement;
        }
        else
        {
            Destroy(newRect);
        }
    }

    void FixedUpdate()
    {
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "dino")
        {
            Destroy(coll.gameObject);
        }
    }
}
