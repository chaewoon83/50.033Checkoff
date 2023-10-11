using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    public Rigidbody2D marioBody;
    public float maxSpeed = 20;
    public float upSpeed = 10;
    private bool onGroundState = true;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    public float deathImpulse = 10;

    private bool moving = false;
    private bool jumpedState = false;
    // animation
    public Animator marioAnimator;

    // other variables
    public GameObject enemies;
    public GameObject GameOver;

    // for audio
    public AudioSource marioAudio;
    public AudioSource marioDieAudio;
    private bool marioDiePitchDown = false;

    public AudioClip marioDeath;


    [System.NonSerialized]
    public bool alive = true;

    //mask for collision
    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    // gamemanager
    GameManager gameManager;



    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator.SetBool("onGround", onGroundState);
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        SceneManager.activeSceneChanged += SetStartingPosition;
    }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));

        if (marioDiePitchDown == true)
        {
            marioDieAudio.pitch -= 0.08f * Time.deltaTime;
        }
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.05f)
            {
                marioAnimator.SetTrigger("onSkid");
            }
               

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.velocity.x < -0.05f)
            {
                marioAnimator.SetTrigger("onSkid");
            }
                
        }
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
            
        }
    }

    // FixedUpdate may be called once per frame. See documentation for details.

    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }

    }

    void Move(int value)
    {

        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (MathF.Abs(marioBody.velocity.x) < maxSpeed)
            marioBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy_Head") && alive)
        {
            Debug.Log("kill Goomba!");
            GameObject EnemyObject = other.gameObject;
            EnemyObject.transform.parent.GetComponent<Transform>().localScale = new Vector3(1.0f, 0.3f, 1.0f);
            EnemyObject.transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            EnemyObject.GetComponent<BoxCollider2D>().enabled = false;
            GameManager.instance.IncreaseScore(1);
            marioBody.velocity.Set(marioBody.velocity.x, 0.0f);
            marioBody.AddForce(Vector2.up* 40.0f , ForceMode2D.Impulse);
        }

        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            Debug.Log("Collided with goomba!");

            // play death animation
            marioAnimator.Play("Mario-die");
            marioDieAudio.PlayOneShot(marioDeath);
            alive = false;
            marioDiePitchDown = true;
        }
    }

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }


    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void GameOverScene()
    {
        gameManager.GameOver(); //
    }

    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }

    public void GameRestart()
    {
        // reset position
        marioBody.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;
        marioDiePitchDown = false;
        marioDieAudio.pitch = 1.0f;

    }
    public void SetStartingPosition(Scene current, Scene next)
    {
        if (next.name == "Mario 1-2")
        {
            // change the position accordingly in your World-1-2 case
            this.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    void Awake()
    {
        // other instructions
        // subscribe to Game Restart event
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }
}
