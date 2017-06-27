﻿using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {


    public float moveSpeed = 12.0f;
    public Vector2 ballDirection = Vector2.left;

    public float topBounds = 9.4f;
    public float bottomBounds = -9.4f;

    public int speedIncreaseInterval = 20;
    public float speedIncreaseBy = 1.0f;
    private float speedIncreaseTimer = 0;

    private float playerPaddleHeight, playerPaddleWidth, computerPaddleHeight, computerPaddleWidth, playerPaddleMaxX, playerPaddleMaxY, playerPaddleMinX, playerPaddleMinY, computerPaddleMaxX, computerPaddleMaxY, computerPaddleMinX, computerPaddleMinY, ballWidth, ballHeight;

    private GameObject paddlePlayer, paddleComputer;

    private float bounceAngle;
    private float vx, vy;

    private float maxAngle = 45.0f;

    private bool collidedWithPlayer, collidedWithComputer, collidedWithWall;

    private game game;

    private bool assignedPoint ;

    // Use this for initialization
    void Start () {

        game = GameObject.Find("Game").GetComponent<game>();


        if (moveSpeed < 0)
        {
            moveSpeed = -1 * moveSpeed;
        }

        paddlePlayer = GameObject.Find("player_paddle");
        paddleComputer = GameObject.Find("computer_paddle");

        playerPaddleHeight = paddlePlayer.transform.GetComponent<SpriteRenderer>().bounds.size.y;
        playerPaddleWidth = paddlePlayer.transform.GetComponent<SpriteRenderer>().bounds.size.x;
        computerPaddleHeight = paddleComputer.transform.GetComponent<SpriteRenderer>().bounds.size.y;
        computerPaddleWidth = paddleComputer.transform.GetComponent<SpriteRenderer>().bounds.size.x;
        ballHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y;
        ballWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x;

        playerPaddleMaxX = paddlePlayer.transform.localPosition.x + playerPaddleWidth / 2;
        playerPaddleMinX = paddlePlayer.transform.localPosition.x - playerPaddleWidth / 2;

        computerPaddleMaxX = paddleComputer.transform.localPosition.x - computerPaddleWidth / 2;
        computerPaddleMinX = paddleComputer.transform.localPosition.x + computerPaddleWidth / 2;


        bounceAngle = GetRandomBounceAngle();

        vx = moveSpeed * Mathf.Cos(bounceAngle);
        vy = moveSpeed * -Mathf.Sin(bounceAngle);


    }
	
	// Update is called once per frame
	void Update () {
        if(game.gameState != game.GameState.Paused)
        {
            Move();

            UpdateSpeedIncrease();
        }
       
	}

    void UpdateSpeedIncrease()
    {
        if(speedIncreaseTimer >= speedIncreaseInterval)
        {
            speedIncreaseTimer = 0;

            if (moveSpeed > 0)
                moveSpeed += speedIncreaseBy;
            else
                moveSpeed -= speedIncreaseBy;

        }else{
            speedIncreaseTimer += Time.deltaTime;
        }
    }

    bool CheckCollision()  {

        playerPaddleMaxY = paddlePlayer.transform.localPosition.y + playerPaddleHeight / 2;
        playerPaddleMinY = paddlePlayer.transform.localPosition.y - playerPaddleHeight / 2;

        computerPaddleMaxY = paddleComputer.transform.localPosition.y + computerPaddleHeight / 2;
        computerPaddleMinY = paddleComputer.transform.localPosition.y - computerPaddleHeight / 2;

        if(transform.localPosition.x - ballWidth / 2 < playerPaddleMaxX && transform.localPosition.x + ballWidth/2 > playerPaddleMinX) {

            if(transform.localPosition.y - ballHeight / 2 < playerPaddleMaxY && transform.localPosition.y + ballHeight/2 > playerPaddleMinY) {

                ballDirection = Vector2.right;
                collidedWithPlayer = true;
                transform.localPosition = new Vector3(playerPaddleMaxX + ballWidth / 2, transform.localPosition.y, transform.localPosition.z);

                return true;

            }else {
               
                if (!assignedPoint)
                {
                    assignedPoint = true;
                    game.ComputerPoint();
                }
            }
        }

        if (transform.localPosition.x + ballWidth / 2 >  computerPaddleMaxX && transform.localPosition.x - ballWidth / 2 < computerPaddleMinX)
        {

            if (transform.localPosition.y - ballHeight / 2 < computerPaddleMaxY && transform.localPosition.y + ballHeight / 2 > computerPaddleMinY)
            {

                ballDirection = Vector2.left;
                collidedWithComputer = true;
                transform.localPosition = new Vector3(computerPaddleMaxX - ballWidth / 2, transform.localPosition.y, transform.localPosition.z);

                return true;
            }else {

                if (!assignedPoint)
                {
                    assignedPoint = true;
                    game.PlayerPoint();
                }
            }
        }

        if(transform.localPosition.y > topBounds)
        {

            transform.localPosition = new Vector3(transform.localPosition.x, topBounds, transform.localPosition.z);
            collidedWithWall = true;
            return true;
        }

        if(transform.localPosition.y < bottomBounds)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, bottomBounds, transform.localPosition.z);
            collidedWithWall = true;
            return true;
        }


        return false;
    }

    void Move()
    {
        // transform.localPosition += (Vector3)ballDirection * moveSpeed * Time.deltaTime;
        if (!CheckCollision())
        {
            vx = moveSpeed * Mathf.Cos(bounceAngle);

            if (moveSpeed > 0) {
                vy = moveSpeed * -Mathf.Sin(bounceAngle);
            }else{
                vy = moveSpeed * Mathf.Sin(bounceAngle);
            }

            //  transform.localPosition += (Vector3)ballDirection * moveSpeed * Time.deltaTime;
            transform.localPosition += new Vector3(ballDirection.x * vx * Time.deltaTime, vy * Time.deltaTime, 0);
        }else{

            if (moveSpeed < 0)
                moveSpeed = -1 * moveSpeed;
            if (collidedWithPlayer){
                collidedWithPlayer = false;
                float relativeIntersectY = paddlePlayer.transform.localPosition.y - transform.localPosition.y;
                float normalizedrelativeIntersectionY = (relativeIntersectY / (playerPaddleHeight / 2));

                bounceAngle = normalizedrelativeIntersectionY * (maxAngle * Mathf.Deg2Rad);

            } else if(collidedWithComputer){
                collidedWithComputer = false;
                float relativeIntersectY = paddleComputer.transform.localPosition.y - transform.localPosition.y;
                float normalizedrelativeIntersectionY = (relativeIntersectY / (computerPaddleHeight / 2));

                bounceAngle = normalizedrelativeIntersectionY * (maxAngle * Mathf.Deg2Rad);

            }else if (collidedWithWall)
            {
                collidedWithWall = false;

                bounceAngle = -bounceAngle;
            }
        }
       }


    float GetRandomBounceAngle(float minDegrees = 160f, float maxDegrees = 260f)
    {
        float minRad = minDegrees * Mathf.PI / 180;
        float maxRad = maxDegrees * Mathf.PI / 180;

        return Random.Range(minRad, maxRad);
    }
   
}
