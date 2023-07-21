using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gdg_playground.Assets.scripts;

public class PlayerMovement : MonoBehaviour
{
    public bool airborne = true;
    public Vector3 starting_pos = new Vector3(0.0f, -3.7f, 0.0f);
    public Vector3 vel = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 force = new Vector3(0.0f, 0.0f, 0.0f);

    private float G_factor = 1.0f;
    private Vector3 G = new Vector3(0.0f, -9.81f, 0.0f);
    private BezierPlatform currentPlatform = null;
    private float collision_timeout = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        G_factor = ShopManager.g_factor;
        G *= G_factor;
        this.transform.position = starting_pos;

        if (ShopManager.rocket)
        {
            vel = new Vector3(10.0f, 34.0f, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // füs-fiss-visicks update
    void FixedUpdate()
    {
        if (collision_timeout < 0.3f) { collision_timeout += Time.fixedDeltaTime; }
        // dead lol
        if (this.transform.position.y < -5.0f)
        {
            return;
        }
        // state switching
        Vector3 flightPath = this.transform.position + (this.vel + (this.force + this.G) * Time.fixedDeltaTime) * Time.fixedDeltaTime;
        BezierPlatform intersect = PlatformManager.CollisionCheck(transform.position, flightPath);
        if (collision_timeout > 0.3f)
        {
            if (airborne)
            {
                if (intersect != null)
                {
                    airborne = false;
                    this.currentPlatform = intersect;

                    float t0 = this.currentPlatform.transform.position.x - this.currentPlatform.transform.localScale.x / 2;
                    float t1 = this.currentPlatform.transform.position.x + this.currentPlatform.transform.localScale.x / 2;

                    float tp0 = (transform.position.x - t0) / (t1 - t0);
                    float yp0 = this.currentPlatform.transform.position.y + (this.currentPlatform.GetBezierPath(tp0).y / 2) * this.currentPlatform.transform.localScale.y;
                    this.transform.position = new Vector3(this.transform.position.x, yp0, transform.position.z);
                    EndingScreenManager.endScore += 10;
                    return;
                }
            }
            else
            {
                Vector2 prox = this.currentPlatform.GetBezierPath(0.95f);
                Vector2 end = this.currentPlatform.GetBezierPath(1.0f);
                this.transform.position = this.currentPlatform.transform.position - this.currentPlatform.transform.lossyScale / 2 + new Vector3(end.x, end.y, this.transform.position.z);
                Vector2 trajectory = (end - prox);
                trajectory *= (this.vel.magnitude / trajectory.magnitude);
                this.vel = new Vector3(trajectory.x, trajectory.y, 0.0f) * 1.15f;
                airborne = true;
                collision_timeout = 0.0f;
            }

        }
        if (airborne)
        {
            Vector3 acc = this.force + this.G;
            this.vel += acc * Time.fixedDeltaTime;
            this.transform.position += this.vel * Time.fixedDeltaTime;
        }
        else
        {

        }
    }
}
