using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UIManagerScript : MonoBehaviour
{

    public GameObject UI_VRMenuGameobject;
    public GameObject UI_DebugPanel;
    public XRRayInteractor xRRayInteractor;

    //public GameObject UI_VRCockpitMenuGameobject;

    // Start is called before the first frame update
    void Start()
    {
        UI_VRMenuGameobject.SetActive(false);
        //UI_DebugPanel.SetActive(true);

        //UI_VRCockpitMenuGameobject.SetActive(false);
    }

    public void OnWorldsButtonClicked()
    {
        Debug.Log("Worlds button clicked");
    }

    public void OnGoHomeButtonClicked()
    {
        Debug.Log("Go Home button clicked");
    }

    public void OnAvatarChangeButtonClicked()
    {
        Debug.Log("Avatar Change button clicked");
    }

    public void OnQuitClicked()
    {
        Debug.Log("Quitting");
        DebugManagerScript.Instance.AddMessage("Quit application called");
        Application.Quit();
    }

    public void OnWristMenuCancel()
    {
        Debug.Log("Cancel wrist menu");
        DebugManagerScript.Instance.AddMessage("Cancel Wrist Menu");
        UI_VRMenuGameobject.SetActive(false);
        xRRayInteractor.enabled = false;
    }

    public void OnShowHideConsole()
    {
        if(UI_DebugPanel.activeSelf)
        {
            UI_DebugPanel.SetActive(false);
        }
        else
        {
            UI_DebugPanel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
