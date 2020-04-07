using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformExtensions : MonoBehaviour {
    public void CopyPosition (Transform target) {
        this.transform.position = target.position;
    }
}
