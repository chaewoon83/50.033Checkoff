using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagicBowserPowerup : BasePowerup
{
    // setup this object's type
    // instantiate variables
    public Animator powerup_animator;
    BoxCollider2D powerupcollider;
    Rigidbody2D poweruprigidbody;
    AudioSource BowserAudioSource;
    public AudioClip BowserClip;

    GameObject mario;
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.Bowser;
        powerupcollider = GetComponent<BoxCollider2D>();
        poweruprigidbody = GetComponent<Rigidbody2D>();
        mario = GameObject.FindGameObjectWithTag("Player");
        BowserAudioSource = this.transform.parent.GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(spawned && col.gameObject.CompareTag("Player"))
        {
            GameManager.instance.IncreaseScore(5);
            mario.GetComponent<PlayerMovement>().GetBowser();
            BowserAudioSource.PlayOneShot(BowserClip);
            DestroyPowerup();
        }
        

        if(col.gameObject.layer == 9 && spawned)
        {
            goRight = !goRight;
            rigidBody.velocity = new Vector2(0.0f, rigidBody.velocity.y);
            rigidBody.AddForce(Vector2.right * 3 * (goRight ? 1 : -1), ForceMode2D.Impulse);
        }
    }

    // interface implementation
    public override void SpawnPowerup()
    {
        spawned = true;
        powerup_animator.SetBool("Spawn", false);
        powerupcollider.enabled = true;
        rigidBody.gravityScale = 1;
        rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse); // move to the right
    }

    public void Reset()
    {
        if (spawned == true)
        {
            spawned = false;
            powerup_animator.SetBool("Spawn", false);
            powerupcollider.enabled = false;
            rigidBody.gravityScale = 0;
            rigidBody.velocity = Vector3.zero;
            this.transform.localPosition = Vector3.up;
            this.gameObject.SetActive(false);
        }

    }


    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object

    }

    public void Update()
    {
    }
}