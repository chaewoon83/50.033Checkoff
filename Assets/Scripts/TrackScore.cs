using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrackScore : MonoBehaviour
{
    public JumpOverGoomba jumpOverGoomba;
    public TextMeshProUGUI scoreText;


    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnEnable()
    {
        scoreText.text = "Score : " + jumpOverGoomba.score.ToString();
        Debug.Log("Update Score");
    }

}
