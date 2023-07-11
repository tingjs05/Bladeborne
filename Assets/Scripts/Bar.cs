using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public GameObject[] bars;
    private List<Slider> sliders;

    // Start is called before the first frame update
    void Start()
    {
        getSliders();
    }

    // get sliders from the bars and add to sliders list
    void getSliders()
    {
        // create empty list for sliders
        sliders = new List<Slider>();

        // if there is more than 0 bar
        if (bars.Length > 0)
        {
            // add the slider for each bar
            foreach (GameObject bar in bars)
            {
                sliders.Add(bar.GetComponent<Slider>());
            }
        }
        // otherwise, get the slider from current game object
        else
        {
            sliders.Add(GetComponent<Slider>());
        }
    }

    // set the max amount for the bar
    public void setMax(float maxAmount)
    {
        // add sliders to sliders list
        getSliders();

        // set each slider
        foreach (Slider slider in sliders)
        {
            // set max value of slider
            slider.maxValue = maxAmount;
            // set current value to max value
            slider.value = maxAmount;
        }
    }

    // change the value of the bar
    public void setValue(float value)
    {
        // add sliders to sliders list
        getSliders();

        // set each slider
        foreach (Slider slider in sliders)
        {
            // set slider value to current value
            slider.value = value / sliders.Count;
        }
    }
}
