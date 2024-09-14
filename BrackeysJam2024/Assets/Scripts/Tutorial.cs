using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialText;
    public GameObject player;
    public GameObject turretActivation1;
    public GameObject turretActivation2;

    public Vector3 WorldSpaceTarget;

    Transform WorldSpaceTransform;

    Vector3 IndicatorPos;

    [SerializeField] Camera Cam;

    [SerializeField] int OffsetY;
    [SerializeField] int OffsetX;

    public bool movementTutorialDone = false;
    public bool dashTutorialDone = false;
    public bool upgradeTutorialDone = false;
    public bool turretTutorialDone = false;
    public bool combatTutorialDone = false;

    public GameObject enemySpawner;

    IEnumerator ChangeTutorialToDash()
    {
        
            yield return new WaitForSeconds(5f);
            tutorialText.GetComponent<TMP_Text>().text = "Press Space to Dash";
            movementTutorialDone = true;
        
    }

    IEnumerator ChangeTutorialToUpgrade()
    {
        
            yield return new WaitForSeconds(3f);
            tutorialText.GetComponent<TMP_Text>().text = "Go to yellow upgrade box and press 'e' to increase health";
            dashTutorialDone = true;
        
    }

    IEnumerator ChangeTutorialToTurret()
    {
        
            yield return new WaitForSeconds(1f);
            tutorialText.GetComponent<TMP_Text>().text = "Go to turret and press 'e' to activate it";
            upgradeTutorialDone = true;
        
        
    }

    IEnumerator EndTutorial()
    {
        yield return new WaitForSeconds(5f);
        tutorialText.SetActive(false);
    }

    IEnumerator ChangeTutorialToCombat()
    {
        yield return new WaitForSeconds(1f);
        tutorialText.GetComponent<TMP_Text>().text = "Dash into enemies to kill them";
        turretTutorialDone = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        //tutorialText.GetComponent<TextMeshPro>().text = "Press WASD to move and press the key in the boat's facing direction to start moving";
        tutorialText.GetComponent<TMP_Text>().text = "Press WASD to move";
    }

    // Update is called once per frame
    void LateUpdate()
    {
        

        WorldSpaceTarget = player.transform.position;

        IndicatorPos = Cam.WorldToScreenPoint(WorldSpaceTarget) + new Vector3(OffsetX, OffsetY, 0); 

        tutorialText.transform.position = IndicatorPos;

        if (Input.GetKeyDown(KeyCode.W) && !movementTutorialDone)
        {
            StartCoroutine(ChangeTutorialToDash());
        }

        if (Input.GetKeyDown(KeyCode.Space) && movementTutorialDone && !dashTutorialDone)
        {
            StartCoroutine(ChangeTutorialToUpgrade());
        }
        else if (Input.GetKeyDown(KeyCode.Space) && dashTutorialDone && turretTutorialDone && !combatTutorialDone)
        {
            StartCoroutine(EndTutorial());
        }

        if (player.GetComponent<Upgrading>().CanUpgradeHP == true && (Input.GetKeyDown(KeyCode.E)) && dashTutorialDone && !upgradeTutorialDone)
        {
            StartCoroutine(ChangeTutorialToTurret());
        }

        if (turretActivation1.GetComponent<ActivationArea>().playerInArea || turretActivation2.GetComponent<ActivationArea>().playerInArea)
        {
            if (Input.GetKeyDown(KeyCode.E) && upgradeTutorialDone && !turretTutorialDone)
            {
                StartCoroutine(ChangeTutorialToCombat());
                enemySpawner.SetActive(true);
            }
        }

        
    }
}
