using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    private GameObject platform;

    void Start()
    {
        GameObject gameObject = Resources.Load("platform", typeof(GameObject)) as GameObject;
        Debug.Log(gameObject);
        this.platform = Instantiate(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            this.platform.GetComponent<BezierPlatform>().Remesh();
        }
    }
}
