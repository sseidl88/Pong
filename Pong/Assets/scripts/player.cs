﻿using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {


    public float moveSpeed = 8.0f;
    public float topBounds = 8.3f;
    public float bottomBounds = -8.3f;
    public Vector2 startingPosition = new Vector2 (-13.0f, 0.0f );

    private game game;
    //Game game;
	// Use this for initialization
	void Start () {
        //game = GameObject.Find("Game").GetComponent<Game>();
        game = GameObject.Find("Game").GetComponent<game>();
        transform.localPosition = (Vector3)startingPosition;
	}
	
	// Update is called once per frame
	void Update () {


        if (game.gameState == game.GameState.Playing)
        {
            // Move();
            CheckUserInput();
        }
       // CheckUserInput();
	}

    void CheckUserInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if(transform.localPosition.y >= topBounds)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, topBounds, transform.localPosition.z);
            }else
            {
                transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;
            }
        } else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (transform.localPosition.y <= bottomBounds)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, bottomBounds, transform.localPosition.z);
            }else
            {
                transform.localPosition += Vector3.down * moveSpeed * Time.deltaTime;
            }
        }
    }
}
