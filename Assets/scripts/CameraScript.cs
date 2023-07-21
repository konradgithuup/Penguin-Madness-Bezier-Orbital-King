using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // words cannot describe how shit this is
    public GameObject target;
    public GameObject background;
    public GameObject water;
    public GameObject clouds;

    // Start is called before the first frame update
    void Start()
    {
        this.target = (GameObject)Instantiate(Resources.Load("Penguin"));
        // this.target = Instantiate(penguinPrefab, new Vector3(-5.0f,0.0f,0.0f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        // camera pos
        float y = Mathf.Max(0.0f, this.target.transform.GetChild(0).position.y);
        float zoom = 5.0f + Mathf.Max(0.0f, this.target.transform.GetChild(0).position.y);
        this.GetComponent<Camera>().orthographicSize = zoom;
        this.transform.position = new Vector3(this.target.transform.GetChild(0).position.x, y, this.transform.position.z);

        // update scenery
        this.water.transform.position = new Vector3(this.transform.position.x, this.water.transform.position.y, this.water.transform.position.z);
        this.clouds.transform.position = new Vector3(this.transform.position.x, this.clouds.transform.position.y, this.clouds.transform.position.z);
        this.background.transform.position = new Vector3(this.transform.position.x, this.background.transform.position.y, this.background.transform.position.z);

        this.water.GetComponent<SpriteRenderer>().material.SetFloat("_Speed", 0.0f + this.target.GetComponentInChildren<PlayerMovement>().vel.x);
    }
}
