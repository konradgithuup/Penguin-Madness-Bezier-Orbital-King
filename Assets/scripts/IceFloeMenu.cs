using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFloeMenu : MonoBehaviour
{
    public GameObject[] IceFloePanels;
    private GameObject[] activePanels;

    private const int NUM_ACTIVE_PANELS = 4;

    // Start is called before the first frame update
    void Start()
    {
        // deactivate not unlocked panels:
        for (int i = IceFloePanels.Length - 1; i >= NUM_ACTIVE_PANELS; i--)
        {
            IceFloePanels[i].SetActive(false);
        }

        // copy unlocked panels into new array (for convenience):
        activePanels = new GameObject[NUM_ACTIVE_PANELS];
        for (int i = 0; i < NUM_ACTIVE_PANELS; i++)
        {
            activePanels[i] = IceFloePanels[i];
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }

}
