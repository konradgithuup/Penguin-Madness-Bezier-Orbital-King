using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// t3ssel8r got me acting unwise https://youtu.be/KPoeNZZ6H4s
public class SecondOrderDynamics : MonoBehaviour
{
    private Vector3 xp; // previous input
    private Vector3 y, yd; // state variables
    private float k1, k2, k3; // dynamics constraints

    public Transform target;
    [Range(0.1f, 20f)]
    public float freq = 10f;
    [Range(0f, 1f)]
    public float damping_coeff = 0.5f;
    [Range(-3f, 3f)]
    public float response_coeff = 1f;

    public void Awake()
    {
        if (target == null)
        {
            target = transform;
        }

    }

    public void Start()
    {
        if (target == null)
        {
            target = transform;
        }

        k1 = damping_coeff / (Mathf.PI * freq);
        k2 = 1 / ((2 * Mathf.PI * freq) * (2 * Mathf.PI * freq));
        k3 = response_coeff * damping_coeff / (2 * Mathf.PI * freq);

        xp = transform.position;
        y = transform.position;
        yd = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    public void Update()
    {
        if (Is_Violating_Reality()) {
            Debug.Log(this.transform.gameObject.name + ": Aborting dynamics update until parameters are restored");
            return;
        }
        Update_Constraints();
        this.transform.position = Update(Time.deltaTime, this.target.position);
    }

    // friendship with trusting myself has ended, paranoia is my new best friend
    private bool Is_Violating_Reality() {
        if (freq == 0f) {
            return true;
        }
        return false;
    }

    private void Update_Constraints()
    {
        k1 = damping_coeff / (Mathf.PI * freq);
        k2 = 1 / ((2 * Mathf.PI * freq) * (2 * Mathf.PI * freq));
        k3 = response_coeff * damping_coeff / (2 * Mathf.PI * freq);
    }

    private Vector3 Update(float T, Vector3 x)
    {
        Vector3 xd = (x - xp) / T;
        xp = x;

        float k2_stable = Mathf.Max(Mathf.Max(k2, T * T / 2 + T * k1 / 2), T * k1);
        y = y + T * yd;
        yd = yd + T * (x + k3 * xd - y - k1 * yd) / k2_stable;

        return y;
    }
}
