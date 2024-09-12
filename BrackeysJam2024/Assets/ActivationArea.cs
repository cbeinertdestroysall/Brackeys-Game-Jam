using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public List<string> Lines;
}

public class ActivationArea : MonoBehaviour
{

    [Tooltip("The text that will be dislayed, Each entry is displayed on a new line")]
    [SerializeField]List<Dialogue> TextInput;

    [SerializeField]GameObject InteractUIPrefab;
    [SerializeField] Transform WorldTarget;

    Canvas TextPromptCanvas;

    public GameObject Prompt;
    InteractionUI TextScript;
    public bool playerInArea;

    [SerializeField] Material T_Highlighted,T_Generic;
    
    // Start is called before the first frame update
    void Start()
    {
        InteractUIPrefab.GetComponent<InteractionUI>().WorldSpaceTransform = WorldTarget;
        InteractUIPrefab.GetComponent<InteractionUI>().DisplayText = TextInput;
        TextPromptCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        Prompt = Instantiate(InteractUIPrefab,TextPromptCanvas.transform);
        TextScript = Prompt.GetComponent<InteractionUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInArea =true;
            TextScript.InArea = true;
            TextScript.ToggleShown();
            gameObject.GetComponent<Renderer>().material = T_Highlighted;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInArea = false;
            TextScript.InArea = false;
            TextScript.ToggleShown();
            gameObject.GetComponent<Renderer>().material = T_Generic;
        }
    }
}
