using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject indicator;
    private static List<GameObject> active_platforms = new List<GameObject>();
    private List<GameObject> active_controllers = new List<GameObject>();

    private GameObject platform_prefab;

    private const float WORLD_COORD_WATER_LEVEL = -3.7f;

    void Start()
    {
        this.platform_prefab = Resources.Load("platform", typeof(GameObject)) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }
        Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v3.y = WORLD_COORD_WATER_LEVEL;
        v3.z = 1f;
        this.indicator.transform.position = v3;
        this.indicator.transform.position -= new Vector3(0,0.5f,0);

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            GameObject controller = new GameObject("platform_controller");
            controller.transform.position = v3;
            active_controllers.Add(controller);
            Transform target = controller.transform;
            Spawn_Platform(target);
        }
    }

    public static BezierPlatform CollisionCheck(Vector3 p0, vector3 p1)
    {
        foreach (GameObject g in PlatformManager.active_platforms)
        {
            if (p1.x < g.transform.position.x || p0.x > g.transform.position.x + g.transform.globalScale.x)
            {
                continue;
            }
            BezierPlatform platform = p0.GetComponent<BezierPlatform>();
            if (platform.GetBezierPath(p0.x).y > p0.x && platform.GetBezierPath(p1.x).y <= platform.GetBezierPath);
        }

        return null;
    }

    private void Spawn_Platform(Transform target) {
        Vector3 spawn_point = new Vector3(target.position.x, WORLD_COORD_WATER_LEVEL-5, target.position.z);
        Debug.Log("create new platform at " + spawn_point + ", target " + target.position);
        GameObject p = Instantiate(this.platform_prefab, spawn_point, Quaternion.identity);
        p.GetComponent<BezierPlatform>().target = target;

        this.active_platforms.Add(p);
    }
}
