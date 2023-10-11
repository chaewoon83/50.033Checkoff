using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    GameObject[] gameObjects;
    // Start is called before the first frame update
    void Start()
    {
        gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        foreach (GameObject child in gameObjects)
        {
            child.GetComponent<EnemyMovement>().GameRestart();
        }
    }

    void Awake()
    {
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }
}