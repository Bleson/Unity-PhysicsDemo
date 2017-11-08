using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer), typeof(PhysicsObject))]
public class Ball : MonoBehaviour {

    PhysicsObject po;

    void Awake() {
        po = GetComponent<PhysicsObject>();
        Time.timeScale = 2.0f;
    }
    
    void OnTriggerEnter2D(Collider2D col) {
        PhysicsMaterialComponent p = col.gameObject.GetComponent<PhysicsMaterialComponent>();
        if (p) {
            po.affectingMaterials.Add(p);
        } else {
            if (col.CompareTag("Finish")) {
                // Win
                Debug.Log("Victory!");
            } else if (col.CompareTag("Respawn")) {
                // Lose
                Debug.Log("Loss!");
            }
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        PhysicsMaterialComponent p = col.gameObject.GetComponent<PhysicsMaterialComponent>();
        if (p) {
            po.affectingMaterials.Remove(p);
        } 
    }
}
