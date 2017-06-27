using UnityEngine;
using System.Collections;

public class computer : MonoBehaviour {

    public float moveSpeed = 8.0f;
    public float topBounds = 8.3f;
    public float bottomBounds = -8.3f;
    public Vector2 startingPosition = new Vector2(13.0f, 0.0f);

    private GameObject ball;
    private Vector2 ballPos;

    //private Game game;
    private game game;
    // Use this for initialization
    void Start()
    {
        game = GameObject.Find("Game").GetComponent<game>();
        // game = GameObject.Find("Game").GetComponent<Game>();
        //  game = GameObject.Find("Game").GetComponent<game>();


        transform.localPosition = (Vector3)startingPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (game.gameState == game.GameState.Playing)
        {
            Move();
        }
    }

    void Move()
    {
        if (!ball)
        {
            //  ball = GameObject.FindGameObjectsWithTag(" ball");
            ball = GameObject.FindGameObjectWithTag("ball");

        }

        if (ball.GetComponent<Ball>().ballDirection == Vector2.right)
        {
            ballPos = ball.transform.localPosition;


            if(transform.localPosition.y > bottomBounds && ballPos.y < transform.localPosition.y)
            {
                transform.localPosition += new Vector3(0, -moveSpeed * Time.deltaTime, 0);
            }

            if(transform.localPosition.y < topBounds && ballPos.y > transform.localPosition.y){
                transform.localPosition += new Vector3(0, moveSpeed * Time.deltaTime, 0);
            }
        }
    }
 
 }



