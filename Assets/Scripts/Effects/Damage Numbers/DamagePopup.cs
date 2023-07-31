using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private float moveSpeed;
    private float disappearTime;
    private float disappearSpeed;
    private float disappearTimer;
    private TextMeshPro textMesh;
    private Color textColor;

    // Update is called once per frame
    void Update()
    {
        // slowly move upwards
        transform.position += new Vector3(0, moveSpeed) * Time.deltaTime;

        // reduce disappear timer
        disappearTimer -= Time.deltaTime;

        // start disappearing if disappear timer <= 0s
        if (disappearTimer <= 0f)
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
        }

        // when text cannot be seen, destroy game object
        if (textColor.a <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void setText(int damage, Color color, float moveSpeed, float disappearTime, float disappearSpeed)
    {
        // get text mesh pro component
        textMesh = transform.GetComponent<TextMeshPro>();

        // set the text to the amount of damage dealt
        textMesh.SetText(damage.ToString());

        // set the original text color
        textMesh.color = color;

        // cache the text color
        textColor = textMesh.color;

        // set variables
        this.moveSpeed = moveSpeed;
        this.disappearTime = disappearTime;
        this.disappearSpeed = disappearSpeed;

        // set disappear timer to time it takes for the text to disappear
        disappearTimer = disappearTime;
    }
}
