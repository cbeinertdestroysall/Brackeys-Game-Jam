using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashMeter : MonoBehaviour
{
    [SerializeField] Slider meter;
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
        transform.position = IndicatorPos + new Vector3(0, OffsetY, 0);
        meter.value = PC.dashMeter;
    }
    public void ShowMeter()
    {
        shown = true;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void HideMeter()
    {
        shown = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }
}
