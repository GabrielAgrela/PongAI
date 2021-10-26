using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public Text ScoreText;
    public GameObject AI;
    public GameObject Ball;
    private int Score;
    public int AvgFrameRate;

    private int XVelocity;
    
    void Start()
    {
        // Resets score, Instantiates ball at 0,0 as the AI's public GameObject Ball and sets it's FrameCounter as 0, so it can predict the ball trajectory again 
        SpawnBall();
    }

    void SpawnBall()
    {
        AI.GetComponent<AIController>().Ball = Instantiate(Ball, new Vector2(0f, 0f), Quaternion.identity);
        AI.GetComponent<AIController>().FrameCounter=0;
        ResetScore();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateScore();
        
        // If Ball is past player/AI's X position, then respawn it.
        CheckBallXBounds();
        
    }

    
    void CheckBallXBounds()
    {
        if (AI.GetComponent<AIController>().Ball.transform.position.x < -8f || AI.GetComponent<AIController>().Ball.transform.position.x > 8f)
		{
			Destroy(AI.GetComponent<AIController>().Ball);
            SpawnBall();
		}
    }

    void UpdateScore()
    {
        // Averaging framerate
        float Current = 0;
        Current = Time.frameCount / Time.time;
        AvgFrameRate = (int)Current;

        // Setting scores and fps
        ScoreText.text = "Score: " + Score + "\nVelocity: "+ XVelocity +" \nFPS: "+AvgFrameRate.ToString();
    }
    
    // Called and incremented when ball touches player
    public void IncrementScore(int XVelocity)
    {
        this.XVelocity = XVelocity;
        Score++; 
    }

    public void ResetScore()
    {
        XVelocity=0;
        Score=0;
    }
}
