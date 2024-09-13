using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [System.Serializable]
    public class TutorialText
    {
        public List<string> Lines;
    }

    [SerializeField] List<Dialogue> TextInput;

    [SerializeField] GameObject InteractUIPrefab;
    [SerializeField] Transform WorldTarget;

    Canvas TextPromptCanvas;
    public GameObject Prompt;
    InteractionUI TextScript;

    // Start is called before the first frame update
    void Start()
    {
        TextPromptCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        Prompt = Instantiate(InteractUIPrefab, TextPromptCanvas.transform);
        TextScript = Prompt.GetComponent<InteractionUI>();
        TextScript.WorldSpaceTransform = WorldTarget.transform;

        
    }

    // Update is called once per frame
    void Update()
    {
        TextScript.InArea = true;
        TextScript.ToggleShown();
        if (Input.GetKeyDown(KeyCode.M))
        {
            TextScript.AdvanceState();
        }
    }
}
