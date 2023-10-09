using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;
    private BoxCollider2D enemyCollider;
    public BoxCollider2D enemyheadCollider;

    public Vector3 startPosition = new Vector3(10.0f, 0.0f, 0.0f);

    GameManager gameManager;

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<BoxCollider2D>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();

        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }
    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }
    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (enemyCollider.enabled == true)
        {
            if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
            {// move goomba
                Movegoomba();
            }
            else
            {
                // change direction
                moveRight *= -1;
                ComputeVelocity();
                Movegoomba();
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
    }

    public void GameRestart()
    {
        transform.localPosition = startPosition;
        originalX = transform.position.x;
        moveRight = -1;
        ComputeVelocity();
        transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
        enemyCollider.enabled = true;
        enemyheadCollider.enabled = true;
    }
}