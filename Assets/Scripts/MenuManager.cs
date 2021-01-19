using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Panel currentPanel = null;
    public Panel resumePanel = null;
    public OVRInput.RawButton menuButton = OVRInput.RawButton.Start;
    public InputManager inputManager;

    private List<Panel> panelHistory = new List<Panel>();

    private void Start()
    {
        SetupPanels();
    }

    void Update()
    {
        // check for user input: Menu button down
        if (OVRInput.GetDown(menuButton, OVRInput.Controller.Touch))
        {
            // display the currentPanel (resumePanel)
            inputManager.disableTeleport();
            currentPanel.Show();
        }
    }

    private void SetupPanels()
    {
        Panel[] panels = GetComponentsInChildren<Panel>();

        foreach (Panel panel in panels)
            panel.Setup(this);

        currentPanel.Show();
    }

    private void UPdate()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
            GoToPrevious();
    }

    public void GoToPrevious()
    {
        if (panelHistory.Count == 0)
        {
            OVRManager.PlatformUIConfirmQuit();
            return;
        }
        int lastIndex = panelHistory.Count - 1;
        SetCurrent(panelHistory[lastIndex]);
        panelHistory.RemoveAt(lastIndex);
    }

    public void SetCurrentWithHistory(Panel newPanel)
    {
        panelHistory.Add(currentPanel);
        SetCurrent(newPanel);

    }

    private void SetCurrent(Panel newPanel)
    {
        currentPanel.Hide();

        currentPanel = newPanel;
        currentPanel.Show();
    }

    public void HideMenu()
    {
        inputManager.enableTeleport();
        currentPanel.Hide();
        currentPanel = resumePanel;
    }
}
