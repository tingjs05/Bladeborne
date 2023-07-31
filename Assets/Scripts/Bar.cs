using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private GameObject[] bars;
    private List<Slider> sliders;
    private Coroutine currentCoroutine;

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

        // shake camera
        shakeCamera(value);

        // set slider value to the value directly since there is only 1 slider
        if (sliders.Count == 1)
        {
            // set slider value to current value
            sliders[0].value = value;
            return;
        }

        // set the value of each slider in the bar
        foreach (Slider slider in sliders)
        {
            // set slider value to current value divided by the number of sliders
            slider.value = (value + slider.maxValue) / sliders.Count;
        }
    }

    // get sliders from the bars and add to sliders list
    private void getSliders()
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

    // shake camera
    private void shakeCamera(float value)
    {
        // do not shake the camera if camera shake is null
        if (cameraShake == null)
        {
            return;
        }

        // calculate change in value
        float valueChange;

        if (sliders.Count == 1)
        {
            // find change in value (damage dealt)
            valueChange = sliders[0].value - value;
        }
        else
        {
            // find change in value (value in all bars in a list should be equal, so just one would do)
            // calculate the actual value from the bar value to get the damage dealt
            valueChange = ((sliders[0].value * sliders.Count) - sliders[0].maxValue) - value;
        }

        // do not shake the camera if health is increased, or not changed
        if (valueChange <= 0)
        {
            return;
        }

        // destroy the current coroutine if there is a coroutine going on, so theres only 1 camera shake coroutine happening at once
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // start camera shake coroutine
        currentCoroutine = StartCoroutine(cameraShake.Shake(1f, valueChange / 5000f));
    }
}
