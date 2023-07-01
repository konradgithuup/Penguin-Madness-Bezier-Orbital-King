using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
attributes are used to change config in the inspector via the empty Debugger gameobject.
static fields are used to easily publish the current config to other scripts.
*/
public class Debugger : MonoBehaviour
{
    public bool renderGizmos = false;
    public static bool isPaused = false;
    
    private static bool gizmo_render_flag_publisher = false;

    void Update() {
        Debugger.gizmo_render_flag_publisher = this.renderGizmos;
    }

    public static bool Render_Gizmos() {
        return Debugger.gizmo_render_flag_publisher;
    }
}