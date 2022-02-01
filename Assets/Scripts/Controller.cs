using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public Rigidbody Ball;

    public Text ScoreText;
    public Text bestScoreText;
    public GameObject GameOverText;

    private void Initialization()
    {
        StartCoroutine(MainManager.instance.SpawnBricksCorrotine());
        bestScoreText.text = $"Best Score: {MainManager.instance.namePlayerBest} : {MainManager.instance.maxPoints}";
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }

    // Update is called once per frame
    void Update()
    {
        MainManager.instance.Ball = Ball;
        MainManager.instance.ScoreText = ScoreText;
        MainManager.instance.GameOverText = GameOverText;

        if (MainManager.instance.m_GameOver)
        {
           bestScoreText.text = $"Best Score: {MainManager.instance.namePlayerBest} : {MainManager.instance.maxPoints}";           
        }

       
    }
}
