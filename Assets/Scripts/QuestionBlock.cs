using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class QuestionBlock : MonoBehaviour
{
    public Rigidbody2D QuestionblockRigidbody;
    public Animator QuestionblockAnimator;
    public EdgeCollider2D StaticBox;
    public BoxCollider2D DynamicBox;
    public Rigidbody2D CoinRigidBody;
    public GameObject CoinGameObject;
    public float endTime = 2.0f;

    // for audio
    public AudioSource CoinAudioSource;
    public AudioClip CoinAudioClip;
    public AudioClip BumpClip;

    float currentTime;
    float currentBumpTime;
    bool IsActive = true;
    bool stopTime = false;
    bool IsBump = false;
    bool IsCoin = false;

    public int parameter;
    public UnityEvent<int> useInt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateInActiveTime();
        CalculateBumpTime();
        //update function for coin 
        if (IsActive == false)
        {
            if (CoinGameObject.transform.localPosition.y < -0.0f)
            {
                if (IsCoin == false)
                {
                    CoinAudioSource.PlayOneShot(CoinAudioClip);
                    IsCoin = true;
                    Debug.Log("ASDASDASD");
                }

                CoinGameObject.SetActive(false);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Mario_Top"))
        {
            if (IsActive == true)
            {
                StaticBox.enabled = true;
                IsActive = false;
                currentTime = 0.0f;
                QuestionblockAnimator.SetBool("IsActive", false);
                SpawnCoin();
            }

            if (IsBump == false)
            {
                IsBump = true;
                CoinAudioSource.PlayOneShot(BumpClip);
                stopTime = false;
            }

        }
    }

    void SpawnCoin()
    {
        useInt.Invoke(parameter);
        CoinGameObject.SetActive(true);
        CoinRigidBody.AddForce(Vector2.up * 15.0f, ForceMode2D.Impulse);
    }

    void CalculateInActiveTime()
    {

        if (currentTime > endTime && stopTime == false)
        {
            stopTime = true;
            QuestionblockRigidbody.bodyType = RigidbodyType2D.Static;
            IsBump = false;
        }
        else
        {
            if (IsActive == false && stopTime == false)
            {
                currentTime += Time.deltaTime;
            }

        }


    }

    void CalculateBumpTime()
    {

        if (currentBumpTime > 0.15f)
        {
            IsBump = false;
        }
        else
        {
            if (IsActive == false)
            {
                currentBumpTime += Time.deltaTime;
            }
        }
    }

    public void Reset()
    {
        CoinGameObject.transform.localPosition = Vector2.zero;
        IsActive = true;
        stopTime = false;
        IsBump = false;
        IsCoin = false;
        QuestionblockAnimator.SetBool("IsActive", true);
        CoinGameObject.SetActive(false);
        StaticBox.enabled = false;

    }
}
