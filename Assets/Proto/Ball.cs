using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer), typeof(PhysicsObject))]
public class Ball : MonoBehaviour {

    public PhysicsObject po;

    void Awake() {
        po = GetComponent<PhysicsObject>();
    }
    
    void OnTriggerEnter2D(Collider2D col) {
        PhysicsMaterialComponent p = col.gameObject.GetComponent<PhysicsMaterialComponent>();
        if (p) {
            po.affectingMaterials.Add(p);
        } else {
            if (col.CompareTag("Finish")) {
                // Win
                Debug.Log("Victory!");
                po.mass = 0;
                po.enabled = false;
                GameManager.Instance.RoundOver(true);
            } else if (col.CompareTag("Respawn")) {
                // Lose
                Debug.Log("Loss!");
                GameManager.Instance.RoundOver(false);
                Destroy(gameObject);
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
