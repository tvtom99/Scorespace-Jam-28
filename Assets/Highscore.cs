using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Highscore : MonoBehaviour
{
    int score, highscore;

    [SerializeField]
    TextMeshPro scoreText, highscoreText;

    [SerializeField]
    Transform camTransform;

    [SerializeField]
    Vector3 offset;

    // Start is called before the first frame update
    void Awake()
    {
        score = 0;
        highscore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        PositionUI();

        scoreText.text = "Score: " + score;
    }

    public void AddScore()
    {
        score++;
    }

    public void GameOver()
    {
        if (score > highscore)
        {
            highscore = score;
            highscoreText.text = "Highscore: " + highscore;
        }

        score = 0;
    }

    void PositionUI()
    {
        
        
        Transform t = gameObject.GetComponent<Transform>();
        t.position = camTransform.position + offset;
        
    }
}
