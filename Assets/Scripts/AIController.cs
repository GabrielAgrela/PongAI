using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIController : MonoBehaviour
{
    public GameObject Ball;
    public GameObject SignalPrefab;
    public GameObject GameManager;
    private GameObject LastSignalPrefab;
    private GameObject PredictionProjection;
    public GameObject PredictionProjectionPrefab;
    
    private float LastXPos = 0f;
    private float LastYPos = 0f;

    private float XPosPrediction = 0f;
    private float YPosPrediction = 0f;

    private float YPosDest = 0f;
    public int FrameCounter = 0;
    public int Resolution = 5;
    public int Speed = 2;
    float Step;

    void Start() 
    {
        // Set value for Lerp 
        Step = Speed * Time.deltaTime;
    }

    // Ball Trajectory is predicted with distance per frame, requiring a fixed frame rate for the rigid body physics operations
    void FixedUpdate()
    {
        // After getting 10 samples from the ball's positions, projects prediction. 
        PrepareBallTrajectoryPrediction();
        // If Ball Colided with Player, project trajectory again
        CheckColisionWithPlayer();
        // Move AI Bar to the Y position chosen by YPosDest
        MoveAIToYPostDest();
    }

    void CheckColisionWithPlayer()
    {
        if (Ball.GetComponent<BallScript>().Collided == true)
        {
            PredictBallTrajectory();
            Ball.GetComponent<BallScript>().Collided = false;
        }
    }

    void MoveAIToYPostDest()
    {
        // Using Lerp for a smoother travel
        transform.position = Vector3.Lerp(transform.position, new Vector2(transform.position.x,YPosDest), Speed*0.01f);
    }
    
    void PrepareBallTrajectoryPrediction()
    {
        print((Ball.transform.position.x-LastXPos)+ " "+ (Ball.transform.position.y-LastYPos));
        // Predict ball position in the next frame based on the current and last frame positions
        XPosPrediction = Ball.transform.position.x+((Ball.transform.position.x-LastXPos)/Resolution);
        YPosPrediction = Ball.transform.position.y+((Ball.transform.position.y-LastYPos)/Resolution);

        // After 10 iterations project the trajectory if it's going in the AI's direction
        FrameCounter++;
        if (FrameCounter == 10 && (Ball.transform.position.x-LastXPos) > 0)
            PredictBallTrajectory();
		
		// Set ball position of the previous frame
        LastXPos = Ball.transform.position.x;
        LastYPos = Ball.transform.position.y;
    }

    // If ball collides with the AI bar, move AI bar to the center
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            Destroy(PredictionProjection);
            YPosDest = 0f;
        }     
    }

    void PredictBallTrajectory()
    {
		
        float SignalFrameTravelDistX=0f;
        float SignalFrameTravelDistY=0f;

        // If the prediction projection already exists, delete's it and instantiates it as a parent 
        Destroy(PredictionProjection);
        PredictionProjection = Instantiate(PredictionProjectionPrefab, new Vector2(0f, 0f), Quaternion.identity);

        for (int i=0;;i++)
        {
            // Instantiate red signal in predicted position with PredictionProjection as it's parent with the Z pos value at 0.94 so it stays behind the ball
            LastSignalPrefab = Instantiate(SignalPrefab, new Vector3(XPosPrediction, YPosPrediction,0.94f), Quaternion.identity,PredictionProjection.transform);

            // Travel distance per frame of the red signal
            SignalFrameTravelDistX = LastSignalPrefab.transform.position.x-LastXPos;
            SignalFrameTravelDistY = LastSignalPrefab.transform.position.y-LastYPos;

            // If the red Signal hits the upper/bottom bounds, change the Y direction of the prediction
            // This should be done with OnCollisionEnter2D(), but this is a close enough of an aproximation
            if (YPosPrediction >= 4.724 || YPosPrediction <= -4.498)
            {
                SignalFrameTravelDistY = SignalFrameTravelDistY*-1;
            }
            
            // If the red Signal hits the side bounds, set the AI bar X position destination and stop the prediction projection
            if (XPosPrediction >= 6.84 || XPosPrediction <= -6.84)
            {
                YPosDest = YPosPrediction;
                return;
            }

            // Prediction of the position of the ball in the next frame
            XPosPrediction = LastSignalPrefab.transform.position.x+SignalFrameTravelDistX;
            YPosPrediction = LastSignalPrefab.transform.position.y+SignalFrameTravelDistY;
            
            // Position from the red signal in previous frame 
            LastXPos = LastSignalPrefab.transform.position.x;
            LastYPos = LastSignalPrefab.transform.position.y;
        }
        
    }
}
