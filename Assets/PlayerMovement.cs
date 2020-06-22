using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float xConstraint = 5f;
    public float initialForwardSpeed = 20f;
    public float speedIncreaseMult = 5f;
    public float force = 500f;

    public TextMeshProUGUI scoreText;
    private static bool gamePlaying = false;
    private float playerScore = 0f;

    public TextMeshProUGUI deathScoreText;
    public CanvasGroup deathScreen;
    private bool died = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!died)
        {
            float magnitude = force * Time.deltaTime;

            if (gamePlaying)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speedIncreaseMult / 1000 * rb.position.z + initialForwardSpeed); // go forward at linearly increasing speed

                playerScore = (float)Math.Floor(rb.position.z / 5);
                scoreText.SetText(playerScore.ToString("00000"));
            }
            else
            {
                if (Input.GetKey("w"))
                {
                    rb.AddForce(0, 0, magnitude);
                }
            }

            if (Input.GetKey("d"))
            {
                rb.AddForce(magnitude, 0, 0);
            }
            if (Input.GetKey("a"))
            {
                rb.AddForce(-magnitude, 0, 0);
            }
        }
        else
        {
            deathScoreText.SetText("Score: " + playerScore.ToString("00000"));
            deathScreen.alpha = (float)Math.Min(1, deathScreen.alpha + 0.5f * Time.deltaTime);
        }

        if (Input.GetKey("r"))
        {
            // restart scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // hacky bounce fix
        if (Math.Abs(rb.position.x) >= xConstraint)
        {
            this.rb.constraints = RigidbodyConstraints.None;
            gamePlaying = false;
        }

        if (rb.position.y <= 0)
        {
            died = true; // fell out of world
        }
    }

    // hacky bounce fix
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor")
        {
            this.rb.constraints = RigidbodyConstraints.FreezePositionY;

            if (!gamePlaying)
            {
                gamePlaying = true;
            }
        }

        if(collision.collider.tag == "Obst")
        {
            died = true; // touched a carrot
        }
    }
}
