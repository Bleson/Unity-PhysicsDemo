using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMaterialComponent : MonoBehaviour {

    public float density = 1000f;
    public Vector2 currentForce = new Vector2(0f, 0f);

    void Update() {
        //Debug.DrawRay(transform.position, currentForce.normalized, 
        //   Color.cyan, Time.fixedDeltaTime);
    }

    void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)currentForce.normalized);
        //Debug.DrawLine(transform.position, currentForce.normalized,
        //    Color.cyan, Time.fixedDeltaTime);
    }
}
