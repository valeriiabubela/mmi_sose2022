using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerCountdown : MonoBehaviour
{
    public static bool gameOver = false;
    public TextMeshProUGUI textDisplay_timer;
    public TextMeshProUGUI textDisplay_highscore;
    public static int totalSeconds = 60;
    public int secondsLeft = totalSeconds;

    public bool takingAway = false;

     void Start ()
     {
        textDisplay_timer.text = "00:" + secondsLeft;
        textDisplay_highscore.text = "";
     }
     void Update()
     {
        if (takingAway == false && secondsLeft > 0 && gameOver == false)
        {
            StartCoroutine(TimerTake());
        }
     }

    IEnumerator TimerTake()
    {
        takingAway = true;
        yield return new WaitForSeconds(1);
        secondsLeft -= 1;
        if (secondsLeft == 0)
        {
            textDisplay_timer.text = "";
            textDisplay_highscore.text = "TIME IS UP!!! \n Your score is: \n          " + ScoreScript.scoreValue;
            Destroy(GameObject.FindWithTag("Score"));
            gameOver = true;
        }
        else if (ScoreScript.scoreValue == ScoreScript.numberOfCoins)
        {
            int bestTime = totalSeconds-secondsLeft;
            textDisplay_timer.text = "";
            textDisplay_highscore.text = "You gathered all the coins in " + bestTime + " seconds!";
            Destroy(GameObject.FindWithTag("Score"));
            gameOver = true;
        }
        else if (secondsLeft < 10)
        {
            textDisplay_timer.text = "00:0" + secondsLeft;
        }
        else
        {
            textDisplay_timer.text = "00:" + secondsLeft;
        }
        takingAway = false;
    }
}
