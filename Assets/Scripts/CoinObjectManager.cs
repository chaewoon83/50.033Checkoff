using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObjectManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject parentObject;
    GameObject[] gameObjects;
    void Start()
    {
        gameObjects = GameObject.FindGameObjectsWithTag("CoinObject");

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
            child.GetComponent<BrownBrick_Coin>().Reset();
        }
    }

    void Awake()
    {
        GameManager.instance.gameRestart.AddListener(Reset);
    }
}
