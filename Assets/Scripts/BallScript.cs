using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BallScript : MonoBehaviour
{
    public int Speed;
    public bool Collided = false;
    

    // On start set direction and speed of the ball
    void Start()
    {
        // Flip a coin to determine if the ball starts left or right
        float x = Random.value < 0.5f ? -1f : 1f;

        // Flip a coin to determine if the ball goes up or down. Set the range
        // between 0.5 -> 1.0 to ensure it does not move completely horizontal.
        float y = Random.value < 0.5f ? Random.Range(-1f, -0.5f) : Random.Range(0.5f, 1f);
        print(x + " "+ y);
        
        GetComponent<Rigidbody2D>().velocity = new Vector2(Speed * x, Speed * y);
    }

    // If Ball Collided with player set Collided true and increase Ball Speed by 20% exponentially
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
		{
			Collided = true;
            if (GetComponent<Rigidbody2D>().velocity.x < 18)
			    GetComponent<Rigidbody2D>().velocity *= 1.2f;
		} 
    }
}
