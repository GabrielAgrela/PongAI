using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int Speed;
    public GameObject GameManager;
    private Rigidbody2D _RB;

    void Start() 
    {
        _RB = GetComponent<Rigidbody2D>();
    }

    // Move up and down with W and S
    void FixedUpdate()
    {
        // If W set rigid body Speed positive, S Negative, else 0
        PlayerMovement();
    }

    void PlayerMovement()
    {
            // Move up
            if (Input.GetKey(KeyCode.W) && _RB.velocity.y < 10)
            {
                //_RB.velocity = new Vector2(0, Speed);
                _RB.AddForce(new Vector2(0, Speed));
            } 
            // Move down
            else if (Input.GetKey(KeyCode.S) && _RB.velocity.y > -10 )
            {
                _RB.AddForce(new Vector2(0, -Speed));
            } 
            // Stop
            else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.W))
            {
                // If it's slow enough stop
                if (_RB.velocity.y > -1 && _RB.velocity.y <1)
                    _RB.velocity = new Vector2(0, 0);
                // Add a little bit of opposite force every frame so it stops exponentially
                else if (_RB.velocity.y > 0)
                    _RB.AddForce(new Vector2(0, -Speed));
                else if (_RB.velocity.y < 0)
                    _RB.AddForce(new Vector2(0, Speed));
            }    
        
    }

    // If Ball collides, increments it's velocity and the Score.
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ball")
		{
            GameManager.GetComponent<GameManager>().IncrementScore(Mathf.RoundToInt(Mathf.Abs(col.gameObject.GetComponent<Rigidbody2D>().velocity.x)));
		}
    }
}
