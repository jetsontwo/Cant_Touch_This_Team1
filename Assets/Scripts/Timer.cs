using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour{

    public float timeLeft;
    public Text text;
    public Text gameOver;
    public int timeShow;
    public int regularFont;
    public int countDownFont;

    void Update()
    {
        if (timeLeft >= 0)
        {
            timeLeft -= Time.deltaTime;
            timeShow = (int)timeLeft;
            if (timeShow >= 30)
            {
                text.fontSize = regularFont;
                text.color = new Color(255, 255, 255);
                if (timeShow >= 60)
                {
                    text.text = string.Format("{0:0}:{1:00}", Mathf.Floor(timeShow / 60), timeShow % 60);
                }
                else
                {
                    text.text = timeShow.ToString();
                }
            }
            else if (timeShow >= 10)
            {
               
                text.fontSize = countDownFont;
                text.color = new Color(255, 255, 0);
                text.text = timeShow.ToString();
            }
            else if (timeShow > 0)
            {
                text.fontSize = countDownFont;
                text.color = new Color(255, 0, 0);
                text.text = "0" + timeShow.ToString();
            }
            else
            {
                text.fontSize = countDownFont;
                text.color = new Color(255, 0, 0);
                text.text = "00";
                gameOver.color = text.color;
                gameOver.fontSize = 50;
                gameOver.text = "GAME OVER";
            }
            
        }
    }
}