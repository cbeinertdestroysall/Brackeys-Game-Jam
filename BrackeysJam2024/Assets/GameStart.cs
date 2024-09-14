using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField] GameObject coinUI,lighthouseUI,healthUI,StartButton,ResetButton;

    [SerializeField] CinemachineVirtualCamera defaultCam,startCam;

    [SerializeField] Animation LightHouseDeath;

    public PlayerController PC;
    Quaternion AimStart;
    bool startedCam,endCam;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        startCam.enabled = true;
        defaultCam.enabled = false;
        PC.teleporting = true;
        AimStart.Set(0.7f,0,0,1);
    }

    public void StartGame()
    {
        coinUI.SetActive(true);
        lighthouseUI.SetActive(true);
        healthUI.SetActive(true);
        StartButton.SetActive(false);

        startCam.enabled = false;
        defaultCam.enabled = true;
        startedCam=true;
        Invoke("enablePC",2f);
    }
    public void EndGame()
    {
        //Debug.Log("ended Game");
        coinUI.SetActive(false);
        healthUI.SetActive(false);
        PC.teleporting = true;
        defaultCam.enabled = false;
        startCam.enabled = true;
        endCam = true;
        LightHouseDeath.Play();

        Invoke("ReloadScene",6f);
    }

    void FixedUpdate()
    {
        if(startedCam)
        {
            Debug.Log("Moving StartCam");
            PC.DefaultVcam.ForceCameraPosition(defaultCam.transform.position,AimStart);
            startedCam = false;
        }
    }
    public void enablePC()
    {
        PC.teleporting = false;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
