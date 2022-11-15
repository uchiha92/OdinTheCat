using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(WaitToEnd), 80);
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void WaitToEnd()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
