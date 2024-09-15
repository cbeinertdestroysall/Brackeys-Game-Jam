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

    public GameObject turret1;
    public GameObject turret2;

    public Vector3 WorldSpaceTarget;

    Transform WorldSpaceTransform;

    Vector3 IndicatorPos;

    [SerializeField] Camera Cam;

    [SerializeField] int OffsetY;
    [SerializeField] int OffsetX;

    public bool startTurtorial = false;
    public bool tutOver = false;
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

    public GameObject lhUI;

    GameObject coinParent;



    IEnumerator ChangeTutorialToDash()
    {

        yield return new WaitForSeconds(5f);
        tutorialText.GetComponent<TMP_Text>().text = "Press Space to Dash";
        movementTutorialDone = true;

    }

    IEnumerator ChangeTutorialToUpgrade1()
    {

        yield return new WaitForSeconds(3f);
        tutorialText.GetComponent<TMP_Text>().text = "Go to yellow upgrade hut and press 'e' to increase health";
        healthUpgrade.SetActive(true);
        dashTutorialDone = true;

    }

    IEnumerator ChangeTutorialToUpgrade2()
    {
        yield return new WaitForSeconds(1f);
        tutorialText.GetComponent<TMP_Text>().text = "Go to blue upgrade hut and press 'e' to increase dash stamina";
        healthUpgrade.SetActive(false);
        speedUpgrade.SetActive(true);
        upgradeTutorial1Done = true;
    }

    IEnumerator ChangeTutorialToTurret()
    {

        yield return new WaitForSeconds(1f);
        tutorialText.GetComponent<TMP_Text>().text = "Go to a turret and press 'e' to activate it";
        healthUpgrade.SetActive(false);
        speedUpgrade.SetActive(false);
        turret1.SetActive(true);
        turret2.SetActive(true);
        upgradeTutorial2Done = true;


    }

    IEnumerator EndTutorial()
    {
        yield return new WaitForSeconds(5f);
        speedUpgrade.SetActive(true);
        healthUpgrade.SetActive(true);
        tutorialText.SetActive(false);
        tutOver = true;
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
        coinParent = GameObject.FindGameObjectWithTag("CoinParent");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !tutOver)
        {
            movementTutorialDone = true;
            dashTutorialDone = true;
            upgradeTutorial1Done = true;
            upgradeTutorial2Done = true;
            turretTutorialDone = true;
            combatTutorialDone = true;
            speedUpgrade.SetActive(true);
            healthUpgrade.SetActive(true);
            tutorialText.SetActive(false);
            turret1.SetActive(true);
            turret2.SetActive(true);
            coinSpawner.GetComponent<SpawnCoins>().Spawn();
            coinSpawner.GetComponent<SpawnCoins>().Spawn();
            lhUI.SetActive(true);
            enemySpawner.SetActive(true);
            tutOver=true;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (startTurtorial)
        {
            WorldSpaceTarget = player.transform.position;

            IndicatorPos = Cam.WorldToScreenPoint(WorldSpaceTarget) + new Vector3(OffsetX, OffsetY, 0);

            tutorialText.transform.position = IndicatorPos;

            if (Input.GetKeyDown(KeyCode.W) && !movementTutorialDone)
            {
                StartCoroutine(ChangeTutorialToDash());
            }

            if (Input.GetKeyDown(KeyCode.Space) && movementTutorialDone && !dashTutorialDone && !upgradeTutorial1Done)
            {
                StartCoroutine(ChangeTutorialToUpgrade1());
            }
            else if (Input.GetKeyDown(KeyCode.Space) && dashTutorialDone && turretTutorialDone && !combatTutorialDone)
            {
                StartCoroutine(ChangeTutorialToGoal());
                lhUI.SetActive(true);
            }

            if (player.GetComponent<Upgrading>().CanUpgradeHP == true && (Input.GetKeyDown(KeyCode.E)) && dashTutorialDone && !upgradeTutorial1Done)
            {
                StartCoroutine(ChangeTutorialToUpgrade2());
            }
            else if (player.GetComponent<Upgrading>().CanUpgradeHP == false && (Input.GetKeyDown(KeyCode.E)) && dashTutorialDone && !upgradeTutorial1Done && coinParent.transform.childCount < healthUpgrade.GetComponent<PaymentManager>().cost && coinParent.transform.childCount < speedUpgrade.GetComponent<PaymentManager>().cost)
            {
                coinSpawner.GetComponent<SpawnCoins>().Spawn();
            }

            if (player.GetComponent<Upgrading>().CanUpgradeSpeed == true && (Input.GetKeyDown(KeyCode.E)) && upgradeTutorial1Done && (!upgradeTutorial2Done || !turretTutorialDone))
            {
                StartCoroutine(ChangeTutorialToTurret());
            }
            else if (player.GetComponent<Upgrading>().CanUpgradeSpeed == false && (Input.GetKeyDown(KeyCode.E)) && upgradeTutorial1Done && (!upgradeTutorial2Done || !turretTutorialDone) && coinParent.transform.childCount < healthUpgrade.GetComponent<PaymentManager>().cost && coinParent.transform.childCount < speedUpgrade.GetComponent<PaymentManager>().cost)
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
                /*else if (Input.GetKeyDown(KeyCode.E) && upgradeTutorial2Done && !turretTutorialDone && player.GetComponent<CoinCollection>().coins < 10)
                {
                    coinSpawner.GetComponent<SpawnCoins>().Spawn();
                }*/
            }

            if (combatTutorialDone)
            {
                StartCoroutine(EndTutorial());
            }
        }

    }
}
