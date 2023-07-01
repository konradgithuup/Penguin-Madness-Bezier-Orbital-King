using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stuff like the penguin.
/// Use force vector in air. While on ice, use magnitude of force vector but ignore direction. On ice, direction == the (approx.) tangent of the surface at the current position.
/// </summary>
public class ActivePhysicsObject : MonoBehaviour
{
    // leave the z coordinate alone, or else...
    public Vector3 force = new Vector3(0,0,0);
    public Vector3 previous_velocity = new Vector3(0,0,0);
    public bool airborne = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        if (airborne)
        {
            
            this.transform += force/2 * (Time.deltaTime*Time.deltaTime) + 
        }
    }

    float addForce(Vector3 f)
    {
        this.force += f;
    }
}
