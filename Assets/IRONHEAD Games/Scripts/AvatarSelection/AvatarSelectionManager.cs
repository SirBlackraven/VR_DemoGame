using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelectionManager : MonoBehaviour
{
    //[SerializeField]
    //GameObject AvatarSelectionPlatformGameobject;


    public GameObject[] selectableAvatarModels;
    public GameObject[] loadableAvatarModels;

    public int avatarSelectionNumber = 0;

    public AvatarInputConverter avatarInputConverter;

    public GameObject XRRigPlatform;
    public GameObject MainViewpoint;

    private Vector3 PlayerOneSpawnPoint = new Vector3(26.78f, 1.4f, -49.5f);
    private Vector3 PlayerTwoSpawnPoint = new Vector3(142.8f, 1.4f, 949.6f);
    private Vector3 PlayerThreeSpawnPoint = new Vector3(-413.3f, 1.4f, 460f);
    private Vector3 PlayerFourSpawnPoint = new Vector3(571.2f, 1.4f, 425f);

    public GameObject playerController;

    /// <summary>
    /// Singleton Implementation
    /// </summary>
    public static AvatarSelectionManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        //Initially, de-activating the Avatar Selection Platform.
        //AvatarSelectionPlatformGameobject.SetActive(false);

        avatarSelectionNumber = 0;
        ActivateAvatarModelAt(avatarSelectionNumber);
        LoadAvatarModelAt(avatarSelectionNumber);
    }

    public void ActivateAvatarSelectionPlatform()
    {
        //AvatarSelectionPlatformGameobject.SetActive(true);
    }

    public void DeactivateAvatarSelectionPlatform()
    {
        //AvatarSelectionPlatformGameobject.SetActive(false);

    }

    public void NextAvatar()
    {
        avatarSelectionNumber += 1;
        if (avatarSelectionNumber >= selectableAvatarModels.Length)
        {
            avatarSelectionNumber = 0;
        }
        ActivateAvatarModelAt(avatarSelectionNumber);

    }

    public void PreviousAvatar()
    {
        avatarSelectionNumber -= 1;

        if (avatarSelectionNumber < 0)
        {
            avatarSelectionNumber = selectableAvatarModels.Length - 1;
        }
        ActivateAvatarModelAt(avatarSelectionNumber);

    }

    /// <summary>
    /// Activates the selected Avatar model inside the Avatar Selection Platform
    /// </summary>
    /// <param name="avatarIndex"></param>
    private void ActivateAvatarModelAt(int avatarIndex)
    {
        foreach (GameObject selectableAvatarModel in selectableAvatarModels)
        {
            selectableAvatarModel.SetActive(false);
        }

        selectableAvatarModels[avatarIndex].SetActive(true);
        Debug.Log(avatarSelectionNumber);

        LoadAvatarModelAt(avatarSelectionNumber);
    }

    public void EnterMech()
    {
        DebugManagerScript.Instance.AddMessage("Trying to board mech.");
        playerController.transform.position = new Vector3(54.8f, 1.4f, -46.01f);
        LoadAvatarModelAt(1);
    }

    public void LeaveMech()
    {
        DebugManagerScript.Instance.AddMessage("Trying to exit mech.");
        LoadAvatarModelAt(0);
    }

    //TODO Expand this
    public void ResetPlayer(int playerNumber)
    {
        LeaveMech();

        switch(playerNumber)
        {
            case 0:
                DebugManagerScript.Instance.AddMessage("Resetting player 1 to start point.");
                playerController.transform.position = PlayerOneSpawnPoint;
                playerController.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                break;

            default:
                playerController.transform.position = PlayerOneSpawnPoint;
                break;
        }
        
    }

    /// <summary>
    /// Loads the Avatar Model and integrates into the VR Player Controller gameobject
    /// </summary>
    /// <param name="avatarIndex"></param>
    private void LoadAvatarModelAt(int avatarIndex)
    {
        foreach (GameObject loadableAvatarModel in loadableAvatarModels)
        {
            loadableAvatarModel.SetActive(false);
        }

        loadableAvatarModels[avatarIndex].SetActive(true);

        avatarInputConverter.MainAvatarTransform = loadableAvatarModels[avatarIndex].GetComponent<AvatarHolder>().MainAvatarTransform;

        avatarInputConverter.AvatarBody = loadableAvatarModels[avatarIndex].GetComponent<AvatarHolder>().BodyTransform;
        avatarInputConverter.AvatarHead = loadableAvatarModels[avatarIndex].GetComponent<AvatarHolder>().HeadTransform;
        avatarInputConverter.AvatarHand_Left = loadableAvatarModels[avatarIndex].GetComponent<AvatarHolder>().HandLeftTransform;
        avatarInputConverter.AvatarHand_Right = loadableAvatarModels[avatarIndex].GetComponent<AvatarHolder>().HandRightTransform;

        //avatarInputConverter.XRHead = loadableAvatarModels[avatarIndex].GetComponent<AvatarHolder>().AvatarCameraOffset;
        DebugManagerScript.Instance.AddMessage("setting rig platform offset to:" + loadableAvatarModels[avatarIndex].GetComponent<AvatarHolder>().AvatarCameraOffset.position.ToString());
       // XRRigPlatform.transform.position = loadableAvatarModels[avatarIndex].GetComponent<AvatarHolder>().AvatarCameraOffset.position;
       //MainViewpoint. = loadableAvatarModels[avatarIndex].GetComponent<AvatarHolder>().AvatarCameraOffset;

        MainViewpoint.transform.SetPositionAndRotation(loadableAvatarModels[avatarIndex].GetComponent<AvatarHolder>().AvatarCameraOffset.position,
            loadableAvatarModels[avatarIndex].GetComponent<AvatarHolder>().AvatarCameraOffset.rotation);
    }
}
