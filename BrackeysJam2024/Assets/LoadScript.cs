using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScript : MonoBehaviour
{
    bool loaded;
    public GameObject SkipText; 
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if(loaded)
        {
            SkipText.SetActive(true);
        }
        loaded = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
