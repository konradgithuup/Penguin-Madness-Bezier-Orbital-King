using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stuff like the penguin.
/// Use force vector in air. While on ice, use magnitude of force vector but ignore direction. On ice, direction == the (approx.) tangent of the surface at the current position.
/// Actually this is all garbage and I hate it >:(
/// I propose, we don't touch forces whatsoever. Instead use the stuff in Trajectory Calculator.
/// </summary>
public class ActivePhysicsObject : MonoBehaviour
{
    // leave the z coordinate alone, or else...
    public Vector3 force = new Vector3(0,0,0);
    public Vector3 previous_velocity = new Vector3(0,0,0);

    // when grounded, get positions from here, should always be the most recent platform
    public BezierPlatform currentPlatform = null;
    public bool airborne = true;

    private Vector3 g = new Vector3(0, -9.81f, 0);
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        float dt = Time.deltaTime;
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        if (airborne)
        {
            Vector3 schmovement = (force*60)/2 * (dt*dt) + this.previous_velocity * dt;
            Debug.Log(force * dt*dt);
            Debug.Log("Schmoved by: " + schmovement);
            this.transform.position += schmovement;
            this.force += dt * g;
        } else
        {
            // TODO implement
        }
        this.previous_velocity = this.force * dt;
        Debug.Log("Prev Vel: " + previous_velocity);
    }

    /// <summary>
    /// Used while airborne, "accurate" physics
    /// </summary>
    public void addForce(Vector3 f)
    {
        this.force += f;
    }

    /// <summary>
    /// Used while grounded, completely made up, detached from reality, even
    /// </summary>
    public void overrideForce(Vector3 f)
    {
        this.force = f;
    }
}
