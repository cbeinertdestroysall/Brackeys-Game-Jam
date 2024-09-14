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
    public bool upgradeTutorial1Done = false;
    public bool upgradeTutorial2Done = false;
    public bool turretTutorialDone = false;
    public bool combatTutorialDone = false;

    public GameObject enemySpawner;
    public GameObject coinSpawner;

    public GameObject healthUpgrade;
    public GameObject speedUpgrade;

    IEnumerator ChangeTutorialToDash()
    {
        
            yield return new WaitForSeconds(5f);
            tutorialText.GetComponent<TMP_Text>().text = "Press Space to Dash";
            movementTutorialDone = true;
        
    }

    IEnumerator ChangeTutorialToUpgrade1()
    {
        
            yield return new WaitForSeconds(3f);
            tutorialText.GetComponent<TMP_Text>().text = "Go to yellow upgrade box and press 'e' to increase health";
            dashTutorialDone = true;
        
    }

    IEnumerator ChangeTutorialToUpgrade2()
    {
        yield return new WaitForSeconds(1f);
        tutorialText.GetComponent<TMP_Text>().text = "Go to blue upgrade box and press 'e' to increase dash stamina";
        upgradeTutorial1Done = true;
    }

    IEnumerator ChangeTutorialToTurret()
    {
        
            yield return new WaitForSeconds(1f);
            tutorialText.GetComponent<TMP_Text>().text = "Go to turret and press 'e' to activate it";
            upgradeTutorial2Done = true;
        
        
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

    IEnumerator ChangeTutorialToGoal()
    {
        yield return new WaitForSeconds(1f);
        tutorialText.GetComponent<TMP_Text>().text = "DEFEND THE LIGHTHOUSE AT ALL COSTS";
        combatTutorialDone = true;
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
            StartCoroutine(ChangeTutorialToUpgrade1());
        }
        else if (Input.GetKeyDown(KeyCode.Space) && dashTutorialDone && turretTutorialDone && !combatTutorialDone)
        {
            StartCoroutine(ChangeTutorialToGoal());
        }

        if (player.GetComponent<Upgrading>().CanUpgradeHP == true && (Input.GetKeyDown(KeyCode.E)) && dashTutorialDone && !upgradeTutorial1Done)
        {
            StartCoroutine(ChangeTutorialToUpgrade2());
        }
        else if (player.GetComponent<Upgrading>().CanUpgradeHP == false && (Input.GetKeyDown(KeyCode.E)) && dashTutorialDone && !upgradeTutorial1Done)
        {
            coinSpawner.GetComponent<SpawnCoins>().Spawn();
        }

        if (player.GetComponent<Upgrading>().CanUpgradeSpeed == true && (Input.GetKeyDown(KeyCode.E)) && upgradeTutorial1Done && !upgradeTutorial2Done)
        {
            StartCoroutine(ChangeTutorialToTurret());
        }
        else if (player.GetComponent<Upgrading>().CanUpgradeSpeed == false && (Input.GetKeyDown(KeyCode.E)) && dashTutorialDone && !upgradeTutorial1Done)
        {
            coinSpawner.GetComponent<SpawnCoins>().Spawn();
        }

        if (turretActivation1.GetComponent<ActivationArea>().playerInArea || turretActivation2.GetComponent<ActivationArea>().playerInArea)
        {
            if (Input.GetKeyDown(KeyCode.E) && upgradeTutorial2Done && !turretTutorialDone)
            {
                StartCoroutine(ChangeTutorialToCombat());
                enemySpawner.SetActive(true);
            }
        }

        if (combatTutorialDone)
        {
            StartCoroutine(EndTutorial());
        }

        
    }
}
