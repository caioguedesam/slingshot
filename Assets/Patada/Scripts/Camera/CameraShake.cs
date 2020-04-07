using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    [SerializeField] private float shakeDuration = .3f;
    [SerializeField] private float shakeMagnitude = .3f;
    private Coroutine routine = null;

    public void ShakeCamera() {
        ShakeCamera(-1f,-1f);
    }

    public void ShakeCamera(float duration = -1f, float magnitude = -1f) {
        if(routine != null) {
            StopCoroutine(routine);
        }

        if(!duration.Equals(-1f)) {
            shakeDuration = duration;
        }

        if(!magnitude.Equals(-1f)) {
            shakeMagnitude = magnitude;
        }

        routine = StartCoroutine(ShakeCameraHandler());
    }

    private IEnumerator ShakeCameraHandler() {
        var originalPos = transform.position;
        var nextPos = transform.position;
        var elapsedTime = 0f;

        while(elapsedTime < shakeDuration) {
            var currentPosition = transform.position;
            if(Vector3.Distance(currentPosition,nextPos) <= Vector3.kEpsilon) {
                nextPos = originalPos + new Vector3(Random.Range(-1f,1f) * shakeMagnitude, Random.Range(-1f,1f) * shakeMagnitude, 0f);
            }

            var deltaX = EasingFunction.EaseInOutSine(currentPosition.x, nextPos.x, Mathf.Min(1f, elapsedTime / (shakeDuration/2f)) );
            var deltaY = EasingFunction.EaseInOutSine(currentPosition.y, nextPos.y, Mathf.Min(1f, elapsedTime / (shakeDuration/2f)) );
            var deltaPosition = new Vector3(deltaX,deltaY,currentPosition.z);

            transform.position = deltaPosition;

            elapsedTime += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = originalPos;
        routine = null;
    }


}
