using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public GameConstants gameConstants;
    float deathImpulse;
    float upSpeed;
    float maxSpeed;
    float speed;

    public Rigidbody2D marioBody;
    private bool onGroundState = true;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    private bool moving = false;
    private bool jumpedState = false;
    // animation
    public Animator marioAnimator;

    // other variables

    // for audio
    public AudioSource marioAudio;
    public AudioSource marioDieAudio;
    private bool marioDiePitchDown = false;

    public AudioClip marioDeath;
    public AudioClip marioDamaged;
    public AudioClip mariokick;


    [System.NonSerialized]
    public bool alive = true;

    //mask for collision
    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    // gamemanager
    GameManager gameManager;

    // for powerup
    bool IsPowerup = false;
    bool IsMushroom = false;
    bool IsBowser = false;
    bool IsInvincible = false;
    float InvincibleTime = 1.0f;
    float InvincibleCurTime = 0.0f;

    // box collider size

    private Vector2 mariocolldersize = 
        new Vector2(0.85f, 1.0f);
    private Vector2 bigmariocolldersize = 
        new Vector2(0.85f, 2.0f);

    private Vector2 mariotopcollderoffset = 
        new Vector2(0.0f, 0.49f);
    private Vector2 bigmariotopcollderoffset = 
        new Vector2(0.0f, 1.0f);


    private Vector2 mariobotcollderoffset =
        new Vector2(0.0f, -0.6f);
    private Vector2 bigmariobotcollderoffset =
        new Vector2(0.0f, -1.1f);


    BoxCollider2D MarioCollider;
    BoxCollider2D MarioTopCollider;
    BoxCollider2D MarioBotCollider;



    // Start is called before the first frame update
    void Start()
    {
        speed = gameConstants.speed;
        maxSpeed = gameConstants.maxSpeed;
        deathImpulse = gameConstants.deathImpulse;
        upSpeed = gameConstants.upSpeed;
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator.SetBool("onGround", onGroundState);
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        marioDieAudio = GameManager.instance.GetComponentInChildren<AudioSource>();

        MarioCollider = GetComponent<BoxCollider2D>();
        MarioTopCollider = GameObject.FindGameObjectWithTag("Mario_Top").GetComponent<BoxCollider2D>();
        MarioBotCollider = GameObject.FindGameObjectWithTag("Mario_Bot").GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));

        if (marioDiePitchDown == true)
        {
            marioDieAudio.pitch -= 0.08f * Time.deltaTime;
        }

        if (IsInvincible == true)
        {
            InvincibleCurTime += Time.deltaTime;
            if (InvincibleCurTime > InvincibleTime)
            {
                IsInvincible = false;
                InvincibleCurTime = 0.0f;
                MarioBotCollider.enabled = true;
            }
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


        if (other.gameObject.CompareTag("Enemy") && alive && IsInvincible == false && IsBowser == false)
        {
            if (IsPowerup == true)
            {
                marioAudio.PlayOneShot(marioDamaged);
                GetDamaged();
            }
            else
            {
                Debug.Log("Collided with goomba!");

                // play death animation
                marioAnimator.Play("Mario-die");
                marioDieAudio.PlayOneShot(marioDeath);
                alive = false;
                marioDiePitchDown = true;
            }

        }
        else if (other.gameObject.CompareTag("Enemy_Head") && alive && IsInvincible == false)
        {
            Debug.Log("kill Goomba!");
            GameObject EnemyObject = other.gameObject;
            EnemyObject.transform.parent.GetComponent<Transform>().localScale = new Vector3(1.0f, 0.3f, 1.0f);
            EnemyObject.transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            EnemyObject.GetComponent<BoxCollider2D>().enabled = false;
            other.enabled = false;
            GameManager.instance.IncreaseScore(1);
            marioBody.velocity.Set(marioBody.velocity.x, 0.0f);
            marioAudio.PlayOneShot(mariokick);
            if (IsBowser == false)
            {
                marioBody.AddForce(Vector2.up * 40.0f, ForceMode2D.Impulse);
            }

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
        marioAnimator.SetBool("IsSmallMario", true);
        alive = true;
        marioDiePitchDown = false;
        marioDieAudio.pitch = 1.0f;
        MarioCollider.size = mariocolldersize;
        MarioTopCollider.offset = mariotopcollderoffset;
        MarioBotCollider.offset = mariobotcollderoffset;
        IsInvincible = false;
        InvincibleCurTime = 0.0f;
        IsBowser = false;
        IsPowerup = false;
    }
    void Awake()
    {
        // other instructions
        // subscribe to Game Restart event
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    public void GetMushroom()
    {
        IsPowerup = true;
        IsMushroom = true;
        marioAnimator.SetBool("IsSmallMario", false);
        marioAnimator.SetTrigger("GetMushroom");
        transform.position = transform.position + Vector3.up * 0.5f;
        MarioCollider.size = bigmariocolldersize;
        MarioTopCollider.offset = bigmariotopcollderoffset;
        MarioBotCollider.offset = bigmariobotcollderoffset;
    }

    public void GetBowser()
    {
        IsPowerup = true;
        IsBowser = true;
        marioAnimator.SetBool("IsSmallMario", false);
        marioAnimator.SetTrigger("GetBowser");

        //TODO
        transform.position = transform.position + Vector3.up * 0.5f;
        MarioCollider.size = bigmariocolldersize;
        MarioTopCollider.offset = bigmariotopcollderoffset;
        MarioBotCollider.offset = bigmariobotcollderoffset;
    }

    public void GetDamaged()
    {
        IsPowerup = false;
        IsInvincible = true;
        InvincibleCurTime = 0.0f;
        MarioBotCollider.enabled = false;
        transform.position = transform.position - Vector3.up * 0.5f;
        MarioCollider.size = mariocolldersize;
        MarioTopCollider.offset = mariotopcollderoffset;
        MarioBotCollider.offset = mariobotcollderoffset;
        marioAnimator.SetBool("IsSmallMario", true);

    }
}
