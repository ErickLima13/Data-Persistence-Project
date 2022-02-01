using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public static string namePlayer;
    public  string namePlayerBest;
    public TextMeshProUGUI nameText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    public bool m_GameOver = false;

    public int m_Points;
    public int maxPoints;  

    public static MainManager instance;

    public void Initialization()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);      
    }

    private void Awake()
    {
        LoadScore();
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                m_Started = false;
            }
        }
    }   

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score {namePlayer} : {m_Points}";

    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (m_Points > maxPoints)
        {
            maxPoints = m_Points;
            namePlayerBest = namePlayer;
        }

        m_Points = 0;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        namePlayer = nameText.text;

    }

    public void SpawnBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    public IEnumerator SpawnBricksCorrotine()
    {
        SpawnBricks();
        yield return new WaitForSeconds(1);
        StopAllCoroutines();
    }

    [System.Serializable]
    class SaveData
    {
        public string namePlayerBest;
        public int maxPoints;
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.namePlayerBest = namePlayerBest;
        data.maxPoints = maxPoints;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText("Assets/savefile.json", json);
    }

    public void LoadScore()
    {
        string path = "Assets/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            namePlayerBest = data.namePlayerBest;
            maxPoints = data.maxPoints;           
        }
    }

    public void ResetScore()
    {
        string path = "Assets/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            namePlayerBest = null;
            maxPoints = 0;
        }
    }
}
