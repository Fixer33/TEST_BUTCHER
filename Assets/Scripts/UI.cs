using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance { get; private set; }

    public int Score = 0;

    [SerializeField] private Text ScoreText;

    private void Start()
    {
        instance = this;
        Score = PlayerPrefs.GetInt("Score", 0);
    }

    private void Update()
    {
        ScoreText.text = Score.ToString();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Score", Score);
    }
}
