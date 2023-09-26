using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BrownBrick_Coin : MonoBehaviour
{
    public Rigidbody2D BrownBlockRigidbody;
    public BoxCollider2D StaticBox;
    public BoxCollider2D DynamicBox;
    public Rigidbody2D CoinRigidBody;
    public GameObject CoinGameObject;
    public float endTime = 2.0f;

    // for audio
    public AudioSource AudioSource;
    public AudioClip CoinAudio;
    public AudioClip BumpAudio;

    float currentTime;
    //mask for collision
    int collisionLayerMask = (1 << 8);
    bool IsActive = true;
    bool IsCoin = true;
    bool IsCoinSound = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (currentTime > endTime) 
        {
            //BrownBlockRigidbody.bodyType = RigidbodyType2D.Static;
            StaticBox.enabled = false;
            DynamicBox.enabled = true;
            IsActive = true;
            Debug.Log("Static block enabled");
        }
        else
        {
            if (IsActive == false)
            {
                currentTime += Time.deltaTime;
            }

        }

        //update function for coin 
        if (IsCoin == false)
        {
            if (CoinGameObject.transform.localPosition.y < 0.0f)
            {
                if (IsCoinSound == true)
                {
                    AudioSource.PlayOneShot(CoinAudio);
                    IsCoinSound = false;
                }
                CoinGameObject.SetActive(false);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Mario_Top") && IsActive == true)
        {
            AudioSource.PlayOneShot(BumpAudio);
            Debug.Log("Bumped");
            StaticBox.enabled = true;
            DynamicBox.enabled = false;
            IsActive = false;
            currentTime = 0.0f;
            if (IsCoin == true)
            {
                SpawnCoin();
                IsCoin = false;
            }
        }
    }

    void SpawnCoin()
    {
        CoinGameObject.SetActive(true);
        CoinRigidBody.AddForce(Vector2.up * 15.0f, ForceMode2D.Impulse);
  
    }
}
