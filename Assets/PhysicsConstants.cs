using UnityEngine;
using System.Collections;

public static class PhysicsConstants {

    public static float airDensity = 1.2f; // kg / m^3

    public static Vector2 gravity = new Vector2(0f, -9.81f); // m / s^2

    public static Vector2 GravitationForce(float mass)
    {
        return Force(mass, gravity);
    }

    //==============================================
    //Newton’s 2nd Law of Motion
    // F = ma
    public static Vector2 Force(float mass, Vector2 acceleration)
    {
        return mass * acceleration;
    }

    public static Vector2 ForceToAcceleration(Vector2 force, float mass)
    {
        return force / BiggerThanZero(mass);
    }

    //==============================================
    //Air resistance

    public static Vector2 AirResistance(float drag, float frontAreaOfObject, Vector2 velocity)
    {
        return new Vector2(AirResistance(drag, frontAreaOfObject,velocity.x),
            AirResistance(drag, frontAreaOfObject, velocity.y));
    }

    public static float AirResistance(float drag, float frontAreaOfObject, float velocity)
    {
        if (velocity < 0f)
        {
            return Resistance(airDensity, -drag, frontAreaOfObject, velocity);
        }
        return Resistance(airDensity, drag, frontAreaOfObject, velocity);
    }

    public static float Resistance(float density, float drag, float frontAreaOfObject, float velocity)
    {
        return -1f * 0.5f * density * drag * frontAreaOfObject * Mathf.Pow(velocity, 2);
    }

    //==============================================
    //Momentum - NOT WORKING CORRECTLY
    
    public static Vector2 Momentum(float mass, Vector2 velocity)
    {
        return mass * velocity;
    }

    public static Vector2 VelocityFromMomentum(float mass, Vector2 momentum)
    {
        return momentum / BiggerThanZero(mass);
    }

    public static Vector2 KineticEnergyToVelocity(Vector2 kinematicEnergy, float mass)
    {
        return new Vector2(KineticEnergyToVelocity(kinematicEnergy.x, mass), KineticEnergyToVelocity(kinematicEnergy.y, mass));
    }

    public static float KineticEnergyToVelocity(float kinematicEnergy, float mass)
    {
        if (kinematicEnergy < 0f)
        {
            return -Mathf.Sqrt(Mathf.Abs((2 * kinematicEnergy) / BiggerThanZero(mass)));
        }
        return Mathf.Sqrt((2 * kinematicEnergy) / BiggerThanZero(mass));
    }

    public static Vector2 KinematicEnergy(float mass, Vector2 velocity)
    {
        return new Vector2(KinematicEnergy(mass, velocity.x), KinematicEnergy(mass, velocity.y));
    }

    public static float KinematicEnergy (float mass, float velocity)
    {
        return 0.5f * BiggerThanZero(mass) * Mathf.Pow(velocity, 2);
    }


    static float BiggerThanZero(float value)
    {
        return Mathf.Clamp(value, 0.0001f, float.MaxValue);
    }
}
