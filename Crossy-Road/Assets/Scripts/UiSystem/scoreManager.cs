using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour
{
    public GameObject Player;
    public Text scoreText;
    public int Score;
    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        Score = (int)Player.transform.position.z;
        scoreText.text = "" + Score;
    }
}
