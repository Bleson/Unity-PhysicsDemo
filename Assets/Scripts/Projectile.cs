using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer), typeof(PhysicsObject))]
public class Projectile : MonoBehaviour {

    PhysicsObject po;
    public SpriteRenderer sR;
    bool notUsed = true;
    float timeDisabled = 0f;
    float timeToDestroy = 0.5f;
    public float damage = 10f;

    void Awake()
    {
        po = GetComponent<PhysicsObject>();
    }

    void Update()
    {
        if (!enabled)
        {
            timeDisabled += Time.deltaTime;
            Color newColor = Color.Lerp(Color.white, Color.black, timeDisabled / timeToDestroy);
            sR.color = newColor;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        TankPart tankpart = coll.collider.GetComponent<TankPart>();
        if (tankpart && notUsed)
        {
            notUsed = false;
            tankpart.TakeDamage(damage);
        }
        Disable();
    }

    void OnTriggerEnter2D(Collider2D col) {

    }

    void Disable()
    {
        Invoke("Destroy", timeToDestroy);
    }

    void Destroy()
    {
        GameManager.Instance.ProjectileHit();
        Destroy(gameObject);
    }

    internal void ApplyVelocity(Vector2 velocity)
    {
        if (po)
        {
            po.ApplyVelocity(velocity);
        }
    }

    internal void ApplyForce(Vector2 force)
    {

    }
}
