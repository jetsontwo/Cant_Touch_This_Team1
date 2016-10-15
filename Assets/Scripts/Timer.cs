using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour{

    public float timeLeft;
    public Text text;
    public int timeShow;
    public int regularFont;
    public int countDownFont;
    private Color white = new Color(255, 255, 255);
    private Color yello = new Color(255, 255, 0);
    private Color red = new Color(255, 0, 0);

    void Update()
    {
        if (timeLeft >= 0)
        {
            timeLeft -= Time.deltaTime;
            timeShow = (int)timeLeft;
            if (timeShow >= 30)
            {
                text.fontSize = regularFont;
                text.color = white;
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
                text.color = yello;
                text.text = timeShow.ToString();
            }
            else
            {
                text.fontSize = countDownFont;
                text.color = red;
                text.text = "0" + timeShow.ToString();
            }          
        }
    }
}