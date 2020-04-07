using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patada.Tween {
	public class TweenObject : MonoBehaviour {
		[System.Serializable] 
		public struct Position {
			public Vector3 position;
			public float rotation;
		}

		[SerializeField] private List<GameObject> tweenObjects = new List<GameObject>();
		[SerializeField] private List<Position> tweenPositions = new List<Position>();
		private Transform startTransform = null;
		
		public List<Vector3> Positions {
			get{
				var list = new List<Vector3>();
				foreach(var obj in tweenPositions) {
					list.Add(obj.position);
				}

				return list;
			}
		}
		private int tweenPositionIndex = 0;
		private bool tweenIncreasing = true;
		[SerializeField] private float duration = 1f;
		[SerializeField] private bool tweenLooping = false;
		[SerializeField] private bool tweenPingPong = false;

		[SerializeField] private EasingFunction.Ease easing;
		private EasingFunction.Function easingFunc = null;

		[SerializeField] private bool noise = false;
		[SerializeField] private Vector3 minNoise = Vector3.zero;
		[SerializeField] private Vector3 maxNoise = Vector3.zero;
		[SerializeField] private float rotationSpeed = 10f;
		[SerializeField] private float noiseSpeed = 20f;
		[SerializeField] private float speed = 10f;
		[SerializeField] private float minDist = .5f;
		[SerializeField] private float loopWaitTime = .5f;
		[SerializeField] private Vector3 currentNoise = Vector3.zero;
		private bool noiseIncreasing = true;

		[SerializeField] private UnityEngine.Events.UnityEvent beginEvents = null;
		[SerializeField] private UnityEngine.Events.UnityEvent endEvents = null;

		private bool playing = false;
		public bool IsPlaying {
			get{
				return playing;
			}
		}

	
		void Update() {

		}

		private Coroutine editorUpdateCoroutine = null;

		// private IEnumerator TweenHandler() {
		// 	while(tweenPositionIndex >= 0 && tweenPositionIndex < tweenPositions.Count) {
		// 		// Debug.Log("Step: " + tweenPositionIndex + "/" + tweenPositions.Count + " " + Time.time);

		// 		if(tweenPositionIndex == 0) {
					
		// 		}

		// 		var startPosition = new List<Vector3>();
		// 		foreach(var obj in tweenObjects){
		// 			startPosition.Add(obj.transform.position);
		// 		}

		// 		var startTime = 0f;
		// 		currentNoise = Vector3.zero;
		// 		var percentage = Mathf.Min(startTime / (duration / (tweenPositions.Count-1)), 1f);

		// 		while(percentage < 1f) {
		// 			startTime += Time.deltaTime;
		// 			percentage = Mathf.Min(startTime / (duration / (tweenPositions.Count-1)), 1f);

		// 			// Debug.Log("Percentage: " + Mathf.Round(percentage * 100f) + "%");
		// 			for(int i = 0 ; i < tweenObjects.Count ; i++) {
		// 				var obj = tweenObjects[i];

		// 				var newPos = new Vector3(
		// 					easingFunc.Invoke(startPosition[i].x, tweenPositions[tweenPositionIndex].position.x, percentage),
		// 					easingFunc.Invoke(startPosition[i].y, tweenPositions[tweenPositionIndex].position.y, percentage),
		// 					easingFunc.Invoke(startPosition[i].z, tweenPositions[tweenPositionIndex].position.z, percentage)
		// 				);

		// 				var movementRotation = Vector3.Angle(newPos - obj.transform.position, Vector3.right);
		// 				var targetRotation = tweenPositions[tweenPositionIndex].rotation;
		// 				// obj.transform.rotation = Quaternion.Euler(0f,0f, Mathf.Lerp(movementRotation, targetRotation, 0f));
		// 				if(noise) {
		// 					var moveDir = tweenPositions[tweenPositionIndex].position - startPosition[i];
		// 					var perpendicularDir = new Vector3(-moveDir.y, moveDir.x).normalized;
							
		// 					if(currentNoise.magnitude >= maxNoise.x) {
		// 						noiseIncreasing = !noiseIncreasing;
		// 					}

		// 					if(noiseIncreasing) {
		// 						currentNoise += perpendicularDir * Time.deltaTime;
		// 					} else {
		// 						currentNoise -= perpendicularDir * Time.deltaTime;
		// 					}
		// 				}

		// 				obj.transform.position = newPos + Vector3.Lerp(currentNoise, Vector3.zero, percentage);
		// 			}

		// 			yield return new WaitForEndOfFrame();

		// 		}

		// 		foreach(var obj in tweenObjects){
		// 			obj.transform.position = tweenPositions[tweenPositionIndex].position;
		// 		}

		// 		if(tweenPositionIndex == 0) {
		// 			foreach(var obj in tweenObjects){
		// 				// obj.transform.rotation = Quaternion.Euler(0f,0f, Vector3.Angle(tweenPositions[1].position - tweenPositions[0].position, Vector3.right));
		// 			}

		// 			beginEvents.Invoke();

		// 		} else if(tweenPositionIndex == tweenPositions.Count - 1) {
		// 			endEvents.Invoke();
		// 		}



		// 		if(tweenLooping && tweenPositionIndex == tweenPositions.Count - 1) {
		// 			tweenPositionIndex = -1;

		// 		} else if(tweenPingPong){
		// 			if(tweenPositionIndex == tweenPositions.Count - 1 || tweenPositionIndex == 0){
		// 				tweenIncreasing = !tweenIncreasing;
		// 			}
		// 		}

		// 		if(tweenIncreasing) {
		// 			tweenPositionIndex++;
		// 		} else {
		// 			tweenPositionIndex--;
		// 		}

		// 		// yield return new WaitForSeconds(duration / (tweenPositions.Count-1));
		// 	}

		// 	editorUpdateCoroutine = null;
		// }

		[SerializeField] private float delay = 0f;

		private IEnumerator TweenHandler() {
			var target = tweenPositions[tweenPositionIndex];
			var hasTarget = true;

			yield return new WaitForSeconds(delay);

			while(hasTarget) {
				target = tweenPositions[tweenPositionIndex];
				if(startTransform != null && tweenPositionIndex == 0){
					target = new Position() {
						position = startTransform.position,
						rotation = 0f
					};
				}

				// Debug.Log("Target: " + target.position);

				currentNoise = Vector3.zero;
				noiseIncreasing = !noiseIncreasing;
				while(Vector3.Distance(target.position, tweenObjects[0].transform.position) >= minDist) {
					target = tweenPositions[tweenPositionIndex];

					if(noise){
						if(Mathf.Abs(currentNoise.x) >= maxNoise.x) {
							noiseIncreasing = !noiseIncreasing;
						}

						if(noiseIncreasing) {
							currentNoise.x += noiseSpeed * Time.deltaTime;
						} else {
							currentNoise.x -= noiseSpeed * Time.deltaTime;
						}
					}
					// Debug.Log(Vector3.Distance(target.position, tweenObjects[0].transform.position) + " " + target.position + " " + tweenObjects[0].transform.position);
					// var forward = Quaternion.AngleAxis(Mathf.Lerp(0f,currentNoise.x, Vector3.Distance(tweenObjects[iOSActivityIndicatorStyle].transform.position, tweenPositions[tweenPositionIndex])))
					// Debug.DrawRay(tweenObjects[0].transform.position, tweenObjects[0].transform.right * 5f, Color.white, 1f);
					Debug.DrawRay(tweenObjects[0].transform.position, (target.position - tweenObjects[0].transform.position), Color.red, 1f);
					var desiredRotation = Vector3.SignedAngle(tweenObjects[0].transform.right, target.position - tweenObjects[0].transform.position, Vector3.back);
					if(noise) {
						desiredRotation += Mathf.Lerp(0f,currentNoise.x, Vector3.Distance(target.position, tweenObjects[0].transform.position));
					}

					// Debug.Log(tweenObjects[0].transform.rotation.eulerAngles.z + " " + desiredRotation + " " + (tweenObjects[0].transform.rotation.eulerAngles.z + desiredRotation));
					for(int i = 0 ; i < tweenObjects.Count ; i++){

						tweenObjects[i].transform.rotation = Quaternion.Euler(new Vector3(tweenObjects[i].transform.rotation.eulerAngles.x,tweenObjects[i].transform.rotation.eulerAngles.y,(tweenObjects[i].transform.rotation.eulerAngles.z - (rotationSpeed * desiredRotation * Time.deltaTime))));
						tweenObjects[i].transform.position += tweenObjects[i].transform.right * speed * Time.deltaTime;
					}
					// Debug.Log(tweenObjects[0].transform.rotation.eulerAngles.z + " " + desiredRotation + " " + (tweenObjects[0].transform.rotation.eulerAngles.z + desiredRotation));
					
					yield return new WaitForEndOfFrame();
				}

				tweenPositionIndex++;
				if(tweenPingPong && !tweenIncreasing) {
					tweenPositionIndex -= 2;
				}

				if(tweenPositionIndex >= tweenPositions.Count) {
					if(tweenLooping) {
						yield return RestartTweenHandler();
						
					} else if (tweenPingPong) {
						if(tweenIncreasing) {
							tweenPositionIndex -= 2;
						} else {
							tweenPositionIndex = 1;
						}

						tweenIncreasing = !tweenIncreasing;
					} else {
						hasTarget = false;
					}

				} else if(tweenPositionIndex <= 0 && tweenPingPong && !tweenIncreasing) {
					tweenPositionIndex = 1;
					tweenIncreasing = !tweenIncreasing;
				}

				yield return new WaitForEndOfFrame();
			}

			Debug.Log("Finished");
			editorUpdateCoroutine = null;
		}


		public void PlayTween(){
			StopTween();
			playing = true;

			foreach(var obj in tweenObjects){
				obj.transform.rotation = Quaternion.Euler(0f,0f, Vector3.Angle(tweenPositions[1].position - tweenPositions[0].position, Vector3.right));
				obj.transform.position = (startTransform != null) ? startTransform.position : tweenPositions[0].position;
			}

			tweenIncreasing = true;
			tweenPositionIndex = 0;
			currentNoise = Vector3.zero;
			easingFunc = EasingFunction.GetEasingFunction(easing);

			beginEvents.Invoke();
			editorUpdateCoroutine = StartCoroutine(TweenHandler());
			reseting = false;
		}

		private IEnumerator RestartTweenHandler() {
			tweenPositionIndex = 0;

			endEvents.Invoke();
			yield return new WaitForSeconds(loopWaitTime);
			foreach(var obj in tweenObjects){
				obj.transform.rotation = Quaternion.Euler(0f,0f, Vector3.Angle(tweenPositions[1].position - tweenPositions[0].position, Vector3.right));
				obj.transform.position = (startTransform != null) ? startTransform.position : tweenPositions[0].position;
			}

			// yield return new WaitForSeconds(loopWaitTime);
			beginEvents.Invoke();
		}

		private bool reseting = false;
		public void RestartTween() {
			if(!reseting) {
				reseting = true;
				Debug.Log("Restart Tween");
				StopTween();

				// foreach(var obj in tweenObjects){
				// 	obj.transform.rotation = Quaternion.Euler(0f,0f, Vector3.Angle(tweenPositions[1].position - tweenPositions[0].position, Vector3.right));
				// 	obj.transform.position = (startTransform != null) ? startTransform.position : tweenPositions[0].position;
				// }


				Invoke("PlayTween", loopWaitTime);
				// StartCoroutine(RestartTweenHandler());
			}
		}

		public void StopTween() {
			playing = false;
			endEvents.Invoke();
			if(editorUpdateCoroutine != null) {
				StopCoroutine(editorUpdateCoroutine);
			}
		}

		public void AddPosition() {
			tweenPositions.Add(new Position() {
				position = transform.localPosition,
				rotation = 0f
			});
		}

		public void AddPosition(Vector3 position) {
			tweenPositions.Add(new Position() {
				position = position,
				rotation = 0f
			});
		}

		public void AddStartPosition(Transform startTransform) {
			this.startTransform = startTransform;

			if(tweenPositions.Count == 0) {
				tweenPositions.Add(new Position() {
					position = startTransform.position,
					rotation = 0f
				});
			} else {
				tweenPositions[0] = new Position() {
					position = startTransform.position,
					rotation = 0f
				};
			}
		}

		public Vector3 GetPositionAt(int index) {
			return tweenPositions[index].position;
		}

		public void RemovePosition() {
			tweenPositions.RemoveAt(tweenPositions.Count-1);
		}

		public void RemovePosition(int index) {
			Debug.Log("Remove Position " + tweenPositions[1]);
			tweenPositions.RemoveAt(index);
			if(tweenPositionIndex > index) {
				tweenPositionIndex--;
			}
		}

		public void ClearPosition() {
			tweenPositions = new List<Position>();
		}
	}
}
