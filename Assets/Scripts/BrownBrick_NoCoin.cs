using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BrownBrick_NoCoin : MonoBehaviour
{
    public Rigidbody2D BrownBlockRigidbody;
    public BoxCollider2D StaticBox;
    public BoxCollider2D DynamicBox;
    public float endTime = 0.2f;

    // for audio
    public AudioSource AudioSource;
    public AudioClip BumpAudio;

    float currentTime;
    //mask for collision
    int collisionLayerMask = (1 << 8);
    bool IsActive = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (currentTime > endTime && IsActive == false) 
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
        }
    }

}
