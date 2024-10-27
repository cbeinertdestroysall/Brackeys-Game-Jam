using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowCost : MonoBehaviour
{
    [SerializeField] TMP_Text costText;

    public GameObject upgradeBox;

    public Vector3 WorldSpaceTarget;
    public Transform WorldSpaceTransform;

    Vector3 IndicatorPos;

    [SerializeField] Camera Cam;
    [SerializeField] int OffsetY;
    [SerializeField] int OffsetX;
    [SerializeField] bool Speed,HP;

    private void Update()
    {
        if (upgradeBox.GetComponent<PaymentManager>().canShow == true)
        {
            costText.enabled = true;
        }
        else
        {
            costText.enabled = false;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateIndicator();
    }

    private void UpdateIndicator()
    {
        WorldSpaceTarget = WorldSpaceTransform.position;
        IndicatorPos = Cam.WorldToScreenPoint(WorldSpaceTarget);
        transform.position = IndicatorPos + new Vector3(OffsetX, OffsetY, 0);

        if(HP)
        {
            costText.text = "Health Upgrade<br>(E) Cost: " + upgradeBox.GetComponent<PaymentManager>().cost;
        }
        else if(Speed)
        {
            costText.text = "Dash Upgrade<br>(E) Cost: " + upgradeBox.GetComponent<PaymentManager>().cost;
        }
    }
}
