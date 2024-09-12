using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] GameObject TextLinePrefab;
    Camera Cam;
    bool Active = false;
    public bool InArea = false;
    int state = 0, maxState;
    public Transform WorldSpaceTransform;

    public List<Dialogue> DisplayText;

    [SerializeField] AudioClip TextIn, TextOut;
    [SerializeField] AudioSource SFX;
    // Start is called before the first frame update
    void Start()
    {
        Cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        maxState = DisplayText.Count - 1;

        for (int i = 0; i < DisplayText[0].Lines.Count; i++)
        {
            GameObject nextText = Instantiate(TextLinePrefab, transform);
            nextText.GetComponentInChildren<TMP_Text>().text = DisplayText[0].Lines[i];
        }

    }

    public void AdvanceState()
    {
        if (/*!isAnimating()*/ InArea)
        {
            if (state >= maxState)
            {
                state = 0;
                ExitCurentState();
                Debug.Log(state);
            }
            else
            {
                state++;
                ExitCurentState();
                Debug.Log(state);
            }
        }

        void ExitCurentState()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Image>().enabled = false;
                transform.GetChild(i).transform.GetChild(0).GetComponent<TMP_Text>().enabled = false;
                Destroy(transform.GetChild(i).gameObject, 0.4f);
            }

            Invoke("loadNextState", 0.5f);
        }
    }
    /*bool isAnimating()
    {
        Debug.Log(transform.GetComponentInChildren<Animation>().isPlaying);
        return transform.GetComponentInChildren<Animation>().isPlaying;
    }*/
    public void loadNextState()
    {
        for (int i = 0; i < DisplayText[state].Lines.Count; i++)
        {
            GameObject nextText = Instantiate(TextLinePrefab, transform);
            nextText.GetComponentInChildren<TMP_Text>().text = DisplayText[state].Lines[i];
        }
        if (/*!isAnimating()*/ InArea)
        {
            for (int i = 0; i < DisplayText[state].Lines.Count; i++)
            {
                transform.GetChild(i).GetComponent<Image>().enabled = true;
                transform.GetChild(i).transform.GetChild(0).GetComponent<TMP_Text>().enabled = true;
            }
        }
    }

    void ShowPrompt()
    {
        SFX.clip = TextIn;
        SFX.Play();
        Active = true;
        Activate();
    }

    void HidePrompt()
    {
        SFX.clip = TextOut;
        SFX.Play();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().enabled = false;
                transform.GetChild(i).transform.GetChild(0).GetComponent<TMP_Text>().enabled = false;
        }
        Deactivate();
    }
    public void Deactivate()
    {
        Active = false;
    }
    public void Activate()
    {
        if (InArea)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Image>().enabled = true;
                transform.GetChild(i).transform.GetChild(0).GetComponent<TMP_Text>().enabled = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.PageUp))
        {
            ToggleShown();
        }
        if (Input.GetKeyDown(KeyCode.E) && maxState > 0)
        {
            AdvanceState();
        }*/
        if(Active && !InArea && !GetComponentInChildren<Animation>().isPlaying)
        {
            InArea = false;
            HidePrompt();
        }
        else if(!Active && InArea && !GetComponent<Animation>().isPlaying)
        {
            InArea = true;
            ShowPrompt();
        }
    }

    public void ToggleShown()
    {
        if (Active)
        {
            HidePrompt();
        }
        else
        {
            ShowPrompt();
        }
    }

    void LateUpdate()
    {
        if (Active)
        {
            transform.position = Cam.WorldToScreenPoint(WorldSpaceTransform.position);
        }
    }
}