using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IceFloeMenu : MonoBehaviour
{
    public GameObject[] IceFloePanels;
    private GameObject[] activePanels;

    private const int NUM_ACTIVE_PANELS = 5;

    private Color defaultPanelColor = new Color(0.6117647f, 0.7607843f, 0.8f, 0.2980392f);
    private Color selectedPanelColor = new Color(0.7987421f, 0.9580713f, 1f, 0.6f);

    public int selectedPanel = 0;

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

    // Supported keyCodes:
    private KeyCode[][] keyCodes =
    {
        new KeyCode[2] { KeyCode.Alpha1, KeyCode.Keypad1 },
        new KeyCode[2] { KeyCode.Alpha2, KeyCode.Keypad2 },
        new KeyCode[2] { KeyCode.Alpha3, KeyCode.Keypad3 },
        new KeyCode[2] { KeyCode.Alpha4, KeyCode.Keypad4 },
        new KeyCode[2] { KeyCode.Alpha5, KeyCode.Keypad5 },
    };

    private double TOTAL_MOUSE_SCROLL_DELAY = 0.07;
    private double mouseScrollDelay = 0.0;

    // Update is called once per frame
    void Update()
    {
        // look for press of number buttons and select specified panel:
        for (int i = 0; i < NUM_ACTIVE_PANELS; i++)
        {
            if (Input.GetKeyDown(keyCodes[i][0]) || Input.GetKeyDown(keyCodes[i][1]))
            {
                updateSelectedPanel(i); break;
            }
        }

        // look for mouse scroll data and move selection up or down:
        if (mouseScrollDelay > 0.01) { mouseScrollDelay -= Time.deltaTime; }
        if (mouseScrollDelay <= 0.01)
        {
            // scrolling up:
            if (Input.mouseScrollDelta.y > 0.5f && selectedPanel > 0)
            {
                updateSelectedPanel(selectedPanel - 1);
                mouseScrollDelay = TOTAL_MOUSE_SCROLL_DELAY;
            }

            // scrolling down:
            else if (Input.mouseScrollDelta.y < -0.5f && selectedPanel < NUM_ACTIVE_PANELS - 1)
            {
                updateSelectedPanel(selectedPanel + 1);
                mouseScrollDelay = TOTAL_MOUSE_SCROLL_DELAY;
            }
        }
    }

    /// <summary>
    /// Updates selected panel by updating selectedPanel and changing panel colors.
    /// </summary>
    private void updateSelectedPanel(int newPanelID)
    {
        changePanelColor(activePanels[selectedPanel], defaultPanelColor);
        changePanelColor(activePanels[newPanelID], selectedPanelColor);
        selectedPanel = newPanelID;
    }

    /// <summary>
    /// Sets color of given panel to given color.
    /// </summary>
    private void changePanelColor(GameObject panel, Color newColor)
    {
        panel.GetComponent<Image>().color = newColor;
    }
}
