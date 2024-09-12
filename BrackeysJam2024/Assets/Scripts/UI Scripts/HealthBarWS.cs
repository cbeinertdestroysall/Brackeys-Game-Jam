using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarWS : MonoBehaviour
{
    [SerializeField] public Slider meter;
    Vector3 IndicatorPos;
    public Vector3 WorldSpaceTarget;
    public bool shown = false;
    public Transform WorldSpaceTransform;
    [SerializeField] Camera Cam;
    [SerializeField] int OffsetY;
    PlayerController PC;
    // Start is called before the first frame update
    void Start()
    {
        PC = GameObject.Find("Player").GetComponent<PlayerController>();
        Cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //WorldSpaceTarget = Transform.position;
    }
    void LateUpdate()
    {
        UpdateIndicator();
    }
    public void SetBarValue(int value)
    {
        meter.value = value;
    }

    private void UpdateIndicator()
    {
        IndicatorPos = Cam.WorldToScreenPoint(WorldSpaceTarget);
        transform.position = IndicatorPos + new Vector3(0, OffsetY, 0);
        
    }
    public void ShowBar()
    {
        shown = true;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void HideBar()
    {
        shown = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
