using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour{

    public float timeLeft;
    public Text text;

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                text.text = "0:00";
            }
            else
            {
                text.text = string.Format("{0:0}:{1:00}", Mathf.Floor(timeLeft / 60), timeLeft % 60);
            }
        }
    }
}