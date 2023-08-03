using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowInArea : MonoBehaviour
{
    [SerializeField] private GameObject[] glowObjects;
    [SerializeField] private float flickerDelay = 5.0f;
    [SerializeField] private float flickerSpeed = 0.1f;
    private bool active = true;
    private float timeSinceLastFlicker = 0f;
    private int flickerCount = 0;
    private Coroutine currentCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        // disable glow object
        setActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            // increment time since last flicker
            timeSinceLastFlicker += Time.deltaTime;

            // start a flicker if time since last flicker is more than flicker delay
            if (timeSinceLastFlicker >= flickerDelay && currentCoroutine == null)
            {
                currentCoroutine = StartCoroutine(flicker());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // only detect player ground collider
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            setActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        // only detect player ground collider
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            // stop flicker coroutine if flickering
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }

            setActive(false);
        }
    }

    private void setActive(bool setValue)
    {
        // set active
        active = setValue;

        // reset variables if just switched to active
        if (active)
        {
            timeSinceLastFlicker = 0f;
            flickerCount = 0;
        }

        // set each glow object in glow objects list
        for (int i = 0; i < glowObjects.Length; i++)
        {
            glowObjects[i].SetActive(active);
        }
    }

    private IEnumerator flicker()
    {
        // set flicker count if it is 0
        if (flickerCount == 0)
        {
            flickerCount = new System.Random().Next(2, 4);
        }

        // keep track of time between each flicker
        float flickerTime = 0f;
        // keep track of number of flickers
        int flickers = 0;
        // keep track of if the glow is flickering
        bool flickering = false;

        while (flickers < flickerCount)
        {
            // check if need to flicker glow
            if (flickerTime >= flickerSpeed && !flickering)
            {
                // deactivate glow
                for (int i = 0; i < glowObjects.Length; i++)
                {
                    glowObjects[i].SetActive(false);
                }
                // set flickering to true
                flickering = true;
                // reset flicker time
                flickerTime = 0f;
            }
            else if (flickerTime >= flickerSpeed && flickering)
            {
                // deactivate glow
                for (int i = 0; i < glowObjects.Length; i++)
                {
                    glowObjects[i].SetActive(true);
                }
                // set flickering to true
                flickering = false;
                // reset flicker time
                flickerTime = 0f;
                // increment flickers
                flickers++;
            }

            // increment flicker time
            flickerTime += Time.deltaTime;

            yield return null;
        }

        // reset flicker count
        flickerCount = 0;
        // reset time since last flicker
        timeSinceLastFlicker = 0f;
        // reset current coroutine
        currentCoroutine = null;
    }
}
