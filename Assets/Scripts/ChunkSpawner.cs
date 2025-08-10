using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChunkSpawner : MonoBehaviour
{
    private int score = 0;

    [SerializeField] private Transform player;

    [System.Serializable]
    public class Chunk
    {
        public GameObject gameObject;
        public float probability;
    }
    [SerializeField] private List<Chunk> chunks;

    public List<GameObject> chunkList;
    private float chunkPos = 10;

    [SerializeField] private bool chase;
    [SerializeField] private float chaseSpeedMultiplier;
    private float chaseTime = 0;
    [SerializeField] private Transform chaseObject;

    [SerializeField] private Text scoreText;

    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private TextMeshProUGUI finalScore;

    // Start is called before the first frame update
    void Start()
    {
        ChangeScore(0);

        for(int i = 0; i < 5; i++)
        {
            SpawnChunk();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.y > chunkPos - 50)
        {
            SpawnChunk();
            ChangeScore(1);
        }

        if (chase)
        {
            chaseTime += Time.deltaTime;
            float speed = Mathf.Log(chaseTime + 1) * chaseSpeedMultiplier; //log(x+10)*5

            chaseObject.position += new Vector3(0,speed,0) * Time.deltaTime;

            if(chaseObject.position.y + 15f > player.position.y)
            {
                player.gameObject.GetComponent<PlayerController>().GameOver();
            }
            else if (chaseObject.position.y < chunkList[20].transform.position.y)
            {
                chaseObject.position += Vector3.up * 10;
            }
        }
    }

    private void SpawnChunk()
    {
        GameObject newChunk = RandomChunk();

        GameObject chunkObject = Instantiate(newChunk, new Vector2(0f, chunkPos), this.transform.rotation);
        chunkList.Add(chunkObject);
        chunkPos += 10;
        //transform.position += Vector3.up * 10;

        if (chunkList.Count > 20)
        {
            if(chaseObject.position.y < chunkList[0].gameObject.transform.position.y) 
            {
                chaseObject.position = chunkList[1].gameObject.transform.position;
            }

            Destroy(chunkList[0].gameObject);
            chunkList.RemoveAt(0);
        }
    }

    private GameObject RandomChunk()
    {
        //adds up the probabiilites from all the tles
        float totalProbability = 0;
        foreach (Chunk obj in chunks)
        {
            totalProbability += obj.probability;
        }
        // Convert the whole number probability values to values between 0 and 1
        foreach (Chunk obj in chunks)
        {
            obj.probability /= totalProbability;
        }
        //normalization complete

        //picks random chunk
        float randomNum = Random.Range(0f, 1f);
        float currentProbability = 0;
        GameObject selectedObject = null;

        foreach (Chunk obj in chunks)
        {
            currentProbability += obj.probability;

            if (randomNum <= currentProbability)
            {
                selectedObject = obj.gameObject;
                break;
            }
        }
        return selectedObject;
    }

    public void ChangeScore(int add)
    {
        score += add;
        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        int money = PlayerPrefs.GetInt("Money",0);
        money += score;
        PlayerPrefs.SetInt("Money", money);

        HUD.SetActive(false);
        gameOverMenu.SetActive(true);

        finalScore.text = "+" + score;

        enabled = false;
    }
}
