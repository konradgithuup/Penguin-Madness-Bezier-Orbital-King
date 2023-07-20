using gdg_playground.Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class IceFloeMenu : MonoBehaviour
{
    private PlatformManager platformManager;

    public GameObject[] IceFloePanels;
    public GameObject[] activePanels;

    private static Color defaultPanelColor = new Color(0.6117647f, 0.7607843f, 0.8f, 0.2980392f);
    private static Color selectedPanelColor = new Color(0.7987421f, 0.9580713f, 1f, 0.6f);

    private const double TOTAL_MOUSE_SCROLL_DELAY = 0.07;
    private double mouseScrollDelay = 0.0;

    public int id = 42;

    // Start is called before the first frame update
    void Start()
    {
        // Deactivate not unlocked panels:
        for (int i = IceFloePanels.Length - 1; i >= PlatformManager.numIceFloes; i--)
        {
            IceFloePanels[i].SetActive(false);
        }

        // Copy unlocked panels into new array (for convenience):
        activePanels = new GameObject[PlatformManager.numIceFloes];
        for (int i = 0; i < PlatformManager.numIceFloes; i++)
        {
            activePanels[i] = IceFloePanels[i];
        }

        // Save instance of PlatformManager:
        platformManager = GameObject.Find("PlatformManager").GetComponent<PlatformManager>();
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

    // Update is called once per frame
    void Update()
    {
        // look for press of number buttons and select specified panel:
        for (int i = 0; i < PlatformManager.numIceFloes; i++)
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
            if (Input.mouseScrollDelta.y > 0.5f && PlatformManager.selectedPlatform > 0)
            {
                updateSelectedPanel(PlatformManager.selectedPlatform - 1);
                mouseScrollDelay = TOTAL_MOUSE_SCROLL_DELAY;
            }

            // scrolling down:
            else if (Input.mouseScrollDelta.y < -0.5f && PlatformManager.selectedPlatform < PlatformManager.numIceFloes - 1)
            {
                updateSelectedPanel(PlatformManager.selectedPlatform + 1);
                mouseScrollDelay = TOTAL_MOUSE_SCROLL_DELAY;
            }
        }
    }

    /// <summary>
    /// Updates selected panel by updating selectedPanel and changing panel colors.
    /// </summary>
    private void updateSelectedPanel(int newPanelID)
    {
        changePanelColor(activePanels[PlatformManager.selectedPlatform], defaultPanelColor);
        changePanelColor(activePanels[newPanelID], selectedPanelColor);
        PlatformManager.selectedPlatform = newPanelID;
    }

    /// <summary>
    /// Sets color of given panel to given color.
    /// </summary>
    private void changePanelColor(GameObject panel, Color newColor)
    {
        panel.GetComponent<Image>().color = newColor;
    }

    public void updateIceFloe(BezierPath newPath, int panelID)
    {

        IceFloePolygon polygon = activePanels[panelID].transform.GetChild(0).gameObject.GetComponent<IceFloePolygon>();
        polygon.bezierPath = newPath;
        polygon.Redraw();
    }
}
