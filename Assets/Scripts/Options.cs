using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Options : MonoBehaviour {
    public Button option1; // 30 sec
    public Button option2; // 1 min default
    public Button option3; // 2 min
    public float gameMaxTimer;

    private Color selected_color = new Color(0, 0, 255, 255);
    private Color default_normal = new Color(255, 255, 255);
    private Color default_highlight = new Color(118, 181, 247);
    private ColorBlock color_1;
    private ColorBlock color_2;
    private ColorBlock color_3;

	// Use this for initialization
	void Start () {
        color_1 = option1.colors;
        color_2 = option2.colors;
        color_3 = option3.colors;
        getCurrentOption();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void getCurrentOption()
    {
        if (gameMaxTimer == 30)
            setOption1();
        else if (gameMaxTimer == 60)
            setOption2();
        else
            setOption3();
    }

    public void setOption1()
    {
            color_2.normalColor = default_normal;
            color_2.highlightedColor = default_highlight;
            option2.colors = color_2;
            color_3.normalColor = default_normal;
            color_3.highlightedColor = default_highlight;
            option3.colors = color_3;
            color_1.normalColor = selected_color;
            color_1.highlightedColor = selected_color;
            option1.colors = color_1;
            gameMaxTimer = 30;
    }

    public void setOption2()
    {
            color_1.normalColor = default_normal;
            color_1.highlightedColor = default_highlight;
            option1.colors = color_1;
            color_3.normalColor = default_normal;
            color_3.highlightedColor = default_highlight;
            option3.colors = color_3;
            color_2.normalColor = selected_color;
            color_2.highlightedColor = selected_color;
            option2.colors = color_2;
            gameMaxTimer = 60;
    }

    public void setOption3()
    {
            color_2.normalColor = default_normal;
            color_2.highlightedColor = default_highlight;
            option2.colors = color_2;
            color_1.normalColor = default_normal;
            color_1.highlightedColor = default_highlight;
            option1.colors = color_1;
            color_3.normalColor = selected_color;
            color_3.highlightedColor = selected_color;
            option3.colors = color_3;
            gameMaxTimer = 120;
    }
}
