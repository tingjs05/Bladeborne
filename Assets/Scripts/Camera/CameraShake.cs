using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // store original camera position
    [SerializeField] private Vector3 originalPos = new Vector3(0f, 0f, -10f);

    public IEnumerator Shake(float duration, float magnitude)
    {
        // keep track of the time elapsed
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            // set new camera position
            transform.localPosition = new Vector3(x, y, originalPos.z);

            // increment time elapsed
            timeElapsed += Time.deltaTime;

            // yield control after moving camera
            yield return null;
        }

        // reset camera position to original position
        transform.localPosition = originalPos;
    }
}
