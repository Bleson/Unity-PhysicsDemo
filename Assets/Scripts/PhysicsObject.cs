using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class PhysicsObject : MonoBehaviour {

    public List<PhysicsMaterialComponent> affectingMaterials = 
        new List<PhysicsMaterialComponent>();

    public Rigidbody2D rb2D;

    public float mass;
    public float bounciness = -0.5f;    // Coefficient of restitution ("bounciness")
    public float drag = 0.1f;           // Coeffecient of drag for a ball

    float terminalVelocity = 200; // m/s
    float frontArea;

    List<Vector2> forces = new List<Vector2>();

    Vector2 lastPosition = Vector2.zero;
    Vector2 lastAcceleration = Vector2.zero;

    Vector2 _currentVelocity = Vector2.zero;
    Vector2 currentVelocity
    {
        get
        {
            return _currentVelocity;
        }
        set
        {
            if (value.magnitude > terminalVelocity)
            {
                value = value.normalized * terminalVelocity;
            }
            _currentVelocity = value;
        }
    }
    Vector2 currentAcceleration = Vector2.zero;

    //===========================================================
    void Awake()
    {
        if (!rb2D)
        {
            rb2D = GetComponent<Rigidbody2D>();
        }
        rb2D.isKinematic = true;

        if (GetComponent<CircleCollider2D>())
        {
            float r =  GetComponent<CircleCollider2D>().radius;
            frontArea = Mathf.PI * r * r;
        }
        else if (GetComponent<BoxCollider2D>())
        {
            //TODO:
        }
    }

    void Start()
    {
        //InvokeRepeating("PrintCurrentVelo", 0f, 1f);
    }

    void FixedUpdate()
    {
        PhysicsUpdate(Time.fixedDeltaTime);
    }

    //===========================================================
    //Collision
    void OnCollisionEnter2D(Collision2D coll)
    {
        Vector2 bounceDir = Vector2.zero;
        foreach (ContactPoint2D cp in coll.contacts)
        {
            bounceDir += (Vector2)transform.position - cp.point;
        }
        Bounce(-bounceDir);
    }

    void Bounce(Vector2 surfaceNormal)
    {
        transform.position = lastPosition;
        if (surfaceNormal.magnitude == 0f)
        {
            currentVelocity = Vector2.Reflect(currentVelocity, Vector3.up) * -bounciness;
        }
        else
        {
            print(surfaceNormal);
            surfaceNormal.Normalize();
            currentVelocity = Vector2.Reflect(currentVelocity, surfaceNormal) * -bounciness;
        }
    }

    //===========================================================
    //Applying motion & forces
    void PhysicsUpdate(float timeStep)
    {
        ApplyForce(PhysicsConstants.GravitationForce(mass));
        ApplyForce(PhysicsConstants.Resistance(PhysicsConstants.airDensity, drag, frontArea, currentVelocity));

        if (affectingMaterials.Count > 0) {
            foreach (var item in affectingMaterials) {
                ApplyForce(PhysicsConstants.CurrentForce(
                    item.density, item.currentForce / affectingMaterials.Count, frontArea));
                ApplyForce(PhysicsConstants.Resistance(
                    item.density / affectingMaterials.Count, drag, frontArea, currentVelocity));
            }
        }

        //Figure out forces on object
        //Apply acceleration & velocity depending on total forces
        lastAcceleration = currentAcceleration;
        UpdateAcceleration(TotalForces(true));

        //Velocity Verlet integration
        Vector2 avgAcceleration = (lastAcceleration + currentAcceleration) / 2;
        UpdateVelocity(avgAcceleration, timeStep);

        Move(currentVelocity, timeStep);
        lastPosition = transform.position;
        
        Debug.DrawRay(transform.position, avgAcceleration.normalized, Color.blue, Time.fixedDeltaTime);
    }

    internal void ApplyForce(Vector2 force)
    {
        forces.Add(force);
    }

    internal void ApplyVelocity(Vector2 velocity)
    {
        currentVelocity = velocity;
    }

    Vector2 TotalForces(bool clearForces = false)
    {
        Vector2 totalForce = Vector2.zero;
        foreach (Vector2 v in forces)
        {
            totalForce += v;
        }

        if (clearForces)
        {
            forces.Clear();
        }

        return totalForce;
    }

    void UpdateAcceleration(Vector2 totalForces)
    {
        currentAcceleration = PhysicsConstants.ForceToAcceleration(totalForces, mass);
    }

    void UpdateVelocity(Vector2 acceleration, float timeStep)
    {
        currentVelocity += acceleration * timeStep;
    }

    void Move(Vector2 velocity, float timeStep)
    {
        rb2D.MovePosition(transform.position + (Vector3)velocity * timeStep + (0.5f * (Vector3)lastAcceleration * Mathf.Pow(timeStep, 2)));
    }

    //===========================================================
    //Test functions

    void PrintCurrentVelo()
    {
        //print(Time.time + ": Speed: " + currentVelocity.magnitude);
        print(Time.time + ": Speed: " + currentVelocity.x 
            + ", " + currentVelocity.y);
    }
}
