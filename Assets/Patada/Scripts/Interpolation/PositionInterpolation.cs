using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PositionInterpolation : MonoBehaviour {
    [System.Serializable] 
    public class PositionCurve {
        public AnimationCurve x = new AnimationCurve();
        public AnimationCurve y = new AnimationCurve();
        public AnimationCurve z = new AnimationCurve();

        public AnimationCurve xAdditional = new AnimationCurve();
        public AnimationCurve yAdditional = new AnimationCurve();
        public AnimationCurve zAdditional = new AnimationCurve();

        public float xMaxNoise = 0f;
        public float yMaxNoise = 0f;
        public float zMaxNoise = 0f;

        public enum NoiseMode {Standart, Proportional, InvProportional};
        public NoiseMode noiseMode = NoiseMode.Standart;
        public bool onlyPerpendicularNoise = false;

        public bool expanded = true;
    }

    [SerializeField] private bool threeDInterpolation = false;
    [SerializeField] private List<PositionCurve> smooths = new List<PositionCurve>() {new PositionCurve()};
    [SerializeField] private PositionCurve defaultPositionCurve = new PositionCurve();
    
    [SerializeField] private float duration = 1f;

    [Space]
    [SerializeField] private RectTransform holder = null;
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private CanvasScaler canvasScaler = null;
    
    [Space]
    [SerializeField] private UnityEvent onFinished = null;
    [SerializeField] private UnityEvent onStart = null;
    [SerializeField] private float startDelay = 0f;

    [SerializeField] private bool debugging = false;

    private RectTransform rTransform = null;

    private Coroutine moveCoroutine = null;

    private Vector3 targetPos = Vector3.zero;

    void Start() {
        this.rTransform = GetComponent<RectTransform>();
    }

    void Update() {
        if(debugging) {
            if(Input.GetMouseButtonDown(0)) {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0f;
                MoveTo(mousePos);
            }
        }
    }

    public void MoveTo(Transform target) {
        if(moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }
        

        moveCoroutine = StartCoroutine(MoveToTarget3D(target.position));
    }

    public void MoveTo(RectTransform target) {
        if(moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }

        var screenPoint = Camera.main.WorldToScreenPoint(target.transform.position);
        var targetPosition = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(holder, screenPoint,Camera.main, out targetPosition);
        

        moveCoroutine = StartCoroutine(MoveToTarget(targetPosition));
    }

    public void MoveTo(GameObject target) {
        if(moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }


        if(!threeDInterpolation) {
            var targetPosition = Vector2.zero;
            var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(holder, screenPoint,Camera.main, out targetPosition);

            moveCoroutine = StartCoroutine(MoveToTarget(targetPosition));
        } else {
            moveCoroutine = StartCoroutine(MoveToTarget3D(target.transform.position));
        }
    }

    public void MoveTo(Vector3 worldPosition) {
        if(moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }


        if(!threeDInterpolation) {
            var targetPosition = Vector2.zero;
            var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(holder, screenPoint,Camera.main, out targetPosition);

            moveCoroutine = StartCoroutine(MoveToTarget(targetPosition));
        } else {
            moveCoroutine = StartCoroutine(MoveToTarget3D(worldPosition));
        }
    }

    public void MoveToTarget() {
        if(moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
            onFinished.Invoke();
        }

        if(!threeDInterpolation) {
            moveCoroutine = StartCoroutine(MoveToTarget(targetPos));
        } else {
            moveCoroutine = StartCoroutine(MoveToTarget3D(targetPos));
        }
    }

    public void SetTargetPosition(GameObject go) {
        if(!threeDInterpolation) {
            var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, go.transform.position);

            var targetPos2d = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(holder, screenPoint,Camera.main, out targetPos2d);

            targetPos = targetPos2d;
        } else {
            targetPos = go.transform.position;
        }
    }

    private IEnumerator MoveToTarget(Vector2 target) {
        yield return new WaitForSeconds(startDelay);

        onStart.Invoke();
        var currentPosition =  this.rTransform.anchoredPosition;
        var startTime = Time.time;
        var elapsedTime = 0f;

        var randomCurve = smooths[Random.Range(0,smooths.Count)];

        var totalMoveDir = target - rTransform.anchoredPosition;

        while(elapsedTime <= duration) {
            var percentage = elapsedTime / duration;

            var tempPosition = currentPosition;
            tempPosition.x = Mathf.Lerp(currentPosition.x, target.x, randomCurve.x.Evaluate(percentage)) + randomCurve.xAdditional.Evaluate(percentage);
            tempPosition.y = Mathf.Lerp(currentPosition.y, target.y, randomCurve.y.Evaluate(percentage)) + randomCurve.yAdditional.Evaluate(percentage);
            
            var noise = new Vector2(Random.Range(-randomCurve.xMaxNoise,randomCurve.xMaxNoise), Random.Range(-randomCurve.yMaxNoise,randomCurve.yMaxNoise));
            var moveDir = (tempPosition - currentPosition);
            if(randomCurve.noiseMode == PositionCurve.NoiseMode.Proportional) {
                noise = Vector2.Lerp(Vector2.zero, noise, moveDir.magnitude / totalMoveDir.magnitude);
            } else if(randomCurve.noiseMode == PositionCurve.NoiseMode.InvProportional) {
                noise = Vector2.Lerp(noise, Vector2.zero, moveDir.magnitude / totalMoveDir.magnitude);
            }

            if(randomCurve.onlyPerpendicularNoise) {
                var perpendicularMoveDir = Vector2.Perpendicular(moveDir.normalized);

                var perpendicularNoise = noise.magnitude * Mathf.Cos(Vector2.Angle(noise,perpendicularMoveDir)) * perpendicularMoveDir;

                tempPosition += Vector2.Lerp(perpendicularNoise, Vector2.zero, percentage + .1f);
                
            } else {
                tempPosition += Vector2.Lerp(noise, Vector2.zero, percentage + .1f);
            }

            this.rTransform.anchoredPosition = (tempPosition);

            elapsedTime += Time.deltaTime;
            
            yield return new WaitForEndOfFrame();
        }

        this.rTransform.anchoredPosition = (target);
        
        yield return new WaitForEndOfFrame();

        moveCoroutine = null;
        onFinished.Invoke();
    }

    private IEnumerator MoveToTarget3D(Vector3 target) {
        yield return new WaitForSeconds(startDelay);

        onStart.Invoke();
        var currentPosition =  this.transform.position;
        var startTime = Time.time;
        var elapsedTime = 0f;

        var randomCurve = smooths[Random.Range(0,smooths.Count)];

        var totalMoveDir = target - this.transform.position;

        while(elapsedTime <= duration) {
            var percentage = elapsedTime / duration;

            var tempPosition = currentPosition;
            tempPosition.x = Mathf.Lerp(currentPosition.x, target.x, randomCurve.x.Evaluate(percentage)) + randomCurve.xAdditional.Evaluate(percentage);
            tempPosition.y = Mathf.Lerp(currentPosition.y, target.y, randomCurve.y.Evaluate(percentage)) + randomCurve.yAdditional.Evaluate(percentage);
            tempPosition.z = Mathf.Lerp(currentPosition.z, target.z, randomCurve.z.Evaluate(percentage)) + randomCurve.zAdditional.Evaluate(percentage);
            
            var noise = new Vector3(Random.Range(-randomCurve.xMaxNoise,randomCurve.xMaxNoise), Random.Range(-randomCurve.yMaxNoise,randomCurve.yMaxNoise));
            var moveDir = (tempPosition - currentPosition);
            if(randomCurve.noiseMode == PositionCurve.NoiseMode.Proportional) {
                noise = Vector3.Lerp(Vector3.zero, noise, moveDir.magnitude / totalMoveDir.magnitude);
            } else if(randomCurve.noiseMode == PositionCurve.NoiseMode.InvProportional) {
                noise = Vector3.Lerp(noise, Vector3.zero, moveDir.magnitude / totalMoveDir.magnitude);
            }

            // if(randomCurve.onlyPerpendicularNoise) {
            //     var perpendicularMoveDir = Vector3.(moveDir.normalized);

            //     var perpendicularNoise = noise.magnitude * Mathf.Cos(Vector3.Angle(noise,perpendicularMoveDir)) * perpendicularMoveDir;

            //     tempPosition += Vector3.Lerp(perpendicularNoise, Vector3.zero, percentage + .1f);
                
            // } else {
            //     tempPosition += Vector3.Lerp(noise, Vector3.zero, percentage + .1f);
            // }

            this.transform.position = (tempPosition);

            elapsedTime += Time.deltaTime;
            
            yield return new WaitForEndOfFrame();
        }

        this.transform.position = (target);
        
        yield return new WaitForEndOfFrame();

        moveCoroutine = null;
        onFinished.Invoke();
    }
}
