using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class QuestionboxMushroom : MonoBehaviour
{
    public Rigidbody2D QuestionblockRigidbody;
    public Animator QuestionblockAnimator;
    public EdgeCollider2D StaticBox;
    public BoxCollider2D DynamicBox;
    public GameObject PowerupGameObject;
    public Animator PowerupAnimation;
    public float endTime = 2.0f;

    // for audio
    public AudioClip BumpClip;
    public AudioClip MushroomClip;

    float currentTime;
    float currentBumpTime;
    bool IsActive = true;
    bool stopTime = false;
    bool IsBump = false;

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
                SpawnPowerup();
            }

            if (IsBump == false)
            {
                IsBump = true;
                stopTime = false;
            }

        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
    }

    void SpawnPowerup()
    {
        useInt.Invoke(parameter);
        PowerupGameObject.SetActive(true);
        PowerupAnimation.SetBool("Spawn", true);
        this.GetComponent<AudioSource>().PlayOneShot(MushroomClip);
    }

    void CalculateInActiveTime()
    {

        if (currentTime > endTime && stopTime == false)
        {
            stopTime = true;
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
        if(IsActive == false)
        {
            IsActive = true;
            stopTime = false;
            IsBump = false;
            QuestionblockAnimator.SetBool("IsActive", true);
            StaticBox.enabled = false;
            PowerupGameObject.GetComponent<MagicMushroomPowerup>().Reset();
            PowerupGameObject.transform.localPosition = Vector3.up;
        }

    }
}
