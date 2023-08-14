using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    Vector3 startPos;
    Rigidbody rb;
    [SerializeField] //makes the properties of the script public to the inspector, but not public to other game objects
    TMP_Text scoreTxt;
    int score;
    int highScore = 0;
    [SerializeField]
    GameObject gameOverHud;
    public TimerScript clock;
    public AudioClip[] sfx;
    AudioSource soundSource;
    public AudioClip endSound;

    // Start is called before the first frame update
    void Start()
    {
        
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        gameOverHud.SetActive(false);
        soundSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -10)
        {
           if (!clock.isGameOver)
             {
               soundSource.PlayOneShot(endSound);
            }

            clock.isGameOver = true;
            gameOverHud.SetActive(true);
            Cursor.visible = true;

            //HighScore stuff
            if(score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("highscore", highScore);
            }

            if (clock.seconds > 9)
            {
                gameOverHud.GetComponent<TMP_Text>().text = $"Game Over \nYour high score is {PlayerPrefs.GetInt("highscore")} \nYour time was: {clock.minutes}:{clock.seconds}";
            }
            else
            {
                gameOverHud.GetComponent<TMP_Text>().text = $"Game Over \nYour high score is {PlayerPrefs.GetInt("highscore")} \nYour time was: {clock.minutes}:0{clock.seconds}";
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Paddle")
        {
            soundSource.PlayOneShot(sfx[Random.Range(0,2)]);
            score++;
            //Update the score text
            scoreTxt.text = $"Score: {score}"; //string literal
        }
    }

    public void RestartGame()
    {
        gameOverHud.SetActive(false);
        transform.position = startPos;
        rb.velocity = Vector3.zero;
        score = 0;
        clock.timeStart = 0;
        clock.isGameOver = false;
        scoreTxt.text = $"Score: {score}";
        Cursor.visible = false;
    }
}
