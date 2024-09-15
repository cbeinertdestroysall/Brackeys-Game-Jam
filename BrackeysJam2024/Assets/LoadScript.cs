using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScript : MonoBehaviour
{
    bool loaded;
    public GameObject SkipText;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        if(loaded)
        {
            SkipText.SetActive(true);
        }
        loaded = true;
    }
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
