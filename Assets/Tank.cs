using UnityEngine;
using System.Collections;

public class Tank : MonoBehaviour {

    public Projectile projectile;
    public Transform projectileSpawnPoint;

    public HealthDisplay hUI;
    public HealthDisplay pUI;
    public float maxHp = 100f;
    public float hp = 100f;

    public float minForce;
    public float maxForce;
    public float force;
    public float defaultForce = 30f;
    public float forcePerSecond = 10;

    Transform cannonPivot;
    public float cannonRotationSpeed = 1f;
    public float cannonRotationLimit = 95f;

    void Awake()
    {
        cannonPivot = transform.FindChild("CannonPivot");
        Init();
    }

    internal void Init()
    {
        hp = maxHp;
        cannonPivot.rotation = Quaternion.Euler(Vector3.zero);
        force = defaultForce;
        hUI.UpdateHealth(hp / maxHp);
        pUI.UpdateHealth(force / maxForce);
    }

    public void TakeDamage(float damage)
    {
        if (Alive())
        {
            hp = Mathf.Clamp(hp - damage, 0f, maxHp);
            hUI.UpdateHealth(hp / maxHp);
        }
    }

    internal bool Alive()
    {
        return hp > 0f;
    }

    public void RotateCannon(bool clockwise = true)
    {
        if (clockwise)
        {
            cannonPivot.Rotate(Vector3.forward, Time.deltaTime * cannonRotationSpeed);

            if (cannonPivot.rotation.eulerAngles.z > cannonRotationLimit &&
                cannonPivot.rotation.eulerAngles.z < 360f - cannonRotationLimit)
                cannonPivot.eulerAngles = new Vector3(0f, 0f, -cannonRotationLimit);
        }
        else
        {
            cannonPivot.Rotate(Vector3.forward, Time.deltaTime * cannonRotationSpeed * -1f);
            if (cannonPivot.rotation.eulerAngles.z < 360f - cannonRotationLimit && 
                cannonPivot.rotation.eulerAngles.z > cannonRotationLimit)
                cannonPivot.eulerAngles = new Vector3(0f, 0f, cannonRotationLimit);
        }
    }

    public void AddForce(bool increase = true)
    {
        if (increase)
        {
            force = Mathf.Clamp(force + forcePerSecond * Time.deltaTime, minForce, maxForce);
        }
        else
        {
            force = Mathf.Clamp(force - forcePerSecond * Time.deltaTime, minForce, maxForce);
        }
        pUI.UpdateHealth(force / maxForce);
    }

    internal void Shoot()
    {
        Projectile p = (Projectile)Instantiate(projectile, projectileSpawnPoint.position, Quaternion.Euler(Vector3.zero));
        p.ApplyVelocity(Quaternion.Euler(0f, 0f, cannonPivot.rotation.eulerAngles.z) * Vector3.up * force);
    }
}
