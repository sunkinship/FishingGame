using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private TextMeshProUGUI scoreText;

    public static int score = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public void AddScore(BaseFish fish)
    {
        //rocket fish
        if (fish is RocketFish)
            score += 5;
        //rock fish
        else if (fish is RockFish rockFish)
        {
            //diomond version
            if (rockFish.IsShattered)
                score += 15;
            //stone version 
            else
                score += 100;
        }
        //normal fish
        else 
            score++;

        if (scoreText != null)
            scoreText.text = score.ToString();
    }
}
