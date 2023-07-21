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
        PlatformManager.active_platforms = new List<GameObject>();
        EndingScreenManager.endScore = 0;
        PlatformManager.numIceFloes = ShopManager.numIceFloes;        

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
        
        this.updateUI = true;
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
            // updateUI = false;
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
            Spawn_Platform(target, nextPlatforms[selectedPlatform]);

            // Load new platform:
            GameObject newPlatform = Instantiate(this.platform_prefab);
            newPlatform.GetComponent<SpriteRenderer>().enabled = false;
            newPlatform.GetComponent<ShadowCaster2D>().enabled = false;
            nextPlatforms[selectedPlatform] = newPlatform;
            updateUI = true;
        }
    }

    public static BezierPlatform CollisionCheck(Vector3 p0, Vector3 p1)
    {
        foreach (GameObject g in PlatformManager.active_platforms)
        {
            BezierPlatform platform = g.GetComponent<BezierPlatform>();

            float t0 = g.transform.position.x - g.transform.localScale.x/2;
            float t1 = g.transform.position.x + g.transform.localScale.x/2;

            float tp0 = (p0.x - t0)/(t1 - t0);
            float tp1 = (p1.x - t0)/(t1 - t0);

            if (tp0 < 0.0f)
            {
                continue;
            }

            float yp0 = g.transform.position.y + (platform.GetBezierPath(tp0).y/2) * g.transform.localScale.y;
            if (yp0 < p0.y && yp0 + 0.1f > p0.y)
            {
                // return platform;
            }

            if (tp1 > 1.0f)
            {
                continue;
            }

            float yp1 = g.transform.position.y + (platform.GetBezierPath(tp1).y/2) * g.transform.localScale.y;

            if ((yp0 < p0.y) && (yp1 >= p1.y))
            {
                return platform;
            }
        }
        return null;
    }

    /// <summary>
    /// Spawns new platform (ice floe) at specified position at water level.
    /// </summary>
    private void Spawn_Platform(Transform target, GameObject platform) {
        Vector3 spawn_point = new Vector3(target.position.x, WORLD_COORD_WATER_LEVEL-5, target.position.z);
        Debug.Log("create new platform at " + spawn_point + ", target " + target.position);
        // GameObject p = Instantiate(this.platform_prefab, spawn_point, Quaternion.identity);
        platform.GetComponent<BezierPlatform>().update = false;
        platform.GetComponent<BezierPlatform>().target = target;
        platform.GetComponent<BezierPlatform>().Teleport(spawn_point);
        platform.GetComponent<BezierPlatform>().transform.position = spawn_point;
        platform.GetComponent<SpriteRenderer>().enabled = true;
        platform.GetComponent<ShadowCaster2D>().enabled = true;
        platform.GetComponent<BezierPlatform>().update = true;
        
        // p.GetComponent<BezierPlatform>().target = target;

       active_platforms.Add(platform);
    }
}
