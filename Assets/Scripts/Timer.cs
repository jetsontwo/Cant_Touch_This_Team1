using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour{

    private float timeLeft;
    public bool stop = true;
    public Text text;

    public void startTimer(float from)
    {
        stop = false;
        timeLeft = from;
    }

    void Update()
    {
        if (!stop)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                stop = true;
                text.text = "0:00";
            }
            else
            {
                text.text = string.Format("{0:0}:{1:00}", Mathf.Floor(timeLeft / 60), timeLeft % 60);
            }
        }
    }
}