using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowserQuestionBoxManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] gameObjects;
    void Start()
    {

        gameObjects = GameObject.FindGameObjectsWithTag("BowserQuestionBox");

        if (gameObjects.Length == 0)
        {
            Debug.Log("No GameObjects are tagged with 'Enemy'");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reset()
    {
        foreach (GameObject child in gameObjects)
        {
            child.GetComponent<QuestionboxBowser>().Reset();
        }
    }

    void Awake()
    {
        GameManager.instance.gameRestart.AddListener(Reset);
    }
}
