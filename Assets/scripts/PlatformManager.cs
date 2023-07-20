using gdg_playground.Assets.scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlatformManager : MonoBehaviour
{
    private IceFloeMenu iceFloeMenu;

    public GameObject indicator;
    private static List<GameObject> active_platforms = new List<GameObject>();
    private List<GameObject> active_controllers = new List<GameObject>();

    public static GameObject[] nextPlatforms;

    private GameObject platform_prefab;

    private const float WORLD_COORD_WATER_LEVEL = -3.7f;

    public static int numIceFloes = 5;
    public static int selectedPlatform = 0;

    private Boolean updateUI = true;

    // Start is called before the first frame update:
    void Start()
    {
        this.platform_prefab = Resources.Load("platform", typeof(GameObject)) as GameObject;

        // "buffer" platforms to display in ice floe menu:
        nextPlatforms = new GameObject[numIceFloes];
        for (int i = 0; i < numIceFloes; i++)
        {
            nextPlatforms[i] = Instantiate(this.platform_prefab);
            nextPlatforms[i].GetComponent<SpriteRenderer>().enabled = false;
            nextPlatforms[i].GetComponent<ShadowCaster2D>().enabled = false;
        }

        // Save instance of iceFloeMenu:
        iceFloeMenu = GameObject.Find("Canvas").GetComponent<IceFloeMenu>();

        // Display platforms in ice floe menu:
        for (int i = 0; i < nextPlatforms.Length; i++)
        {
            iceFloeMenu.updateIceFloe(nextPlatforms[i].GetComponent<BezierPlatform>().surface, i);
        }
    }

    private GameObject child;

    // Update is called once per frame
    void Update()
    {
        // Update UI if neccessary:
        if (updateUI)
        {
            for (int i = 0; i < nextPlatforms.Length; i++)
            {
                iceFloeMenu.updateIceFloe(nextPlatforms[i].GetComponent<BezierPlatform>().surface, i);
            }
            updateUI = false;
        }

        // Skip method if game is paused:
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        // Get position of water surface below player's mouse position:
        Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v3.y = WORLD_COORD_WATER_LEVEL;
        v3.z = 1f;

        // Display indicator right below player's mouse position:
        this.indicator.transform.position = v3;
        this.indicator.transform.position -= new Vector3(0,0.5f,0);

        // Spawn ice floe if right mouse button was pressed down:
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            GameObject controller = new GameObject("platform_controller");
            controller.transform.position = v3;
            active_controllers.Add(controller);
            Transform target = controller.transform;
            Spawn_Platform(target);
        }
    }

    public static BezierPlatform CollisionCheck(Vector3 p0, Vector3 p1)
    {
        /*
        foreach (GameObject g in PlatformManager.active_platforms)
        {
            if (p1.x < g.transform.position.x || p0.x > g.transform.position.x + g.transform.globalScale.x)
            {
                continue;
            }
            BezierPlatform platform = p0.GetComponent<BezierPlatform>();
            if (platform.GetBezierPath(p0.x).y > p0.x && platform.GetBezierPath(p1.x).y <= platform.GetBezierPath);
        }
        */

        return null;
    }

    /// <summary>
    /// Spawns new platform (ice floe) at specified position at water level.
    /// </summary>
    private void Spawn_Platform(Transform target) {
        Vector3 spawn_point = new Vector3(target.position.x, WORLD_COORD_WATER_LEVEL-5, target.position.z);
        Debug.Log("create new platform at " + spawn_point + ", target " + target.position);
        GameObject p = Instantiate(this.platform_prefab, spawn_point, Quaternion.identity);
        p.GetComponent<BezierPlatform>().target = target;

       active_platforms.Add(p);
    }
}
