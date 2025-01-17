using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(Transform cameraTransform, float duration, float magnitude)
    {
        Vector3 originalPos = cameraTransform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;

            cameraTransform.localPosition = new Vector3(originalPos.x + xOffset, originalPos.y + yOffset, originalPos.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalPos;
    }
}
