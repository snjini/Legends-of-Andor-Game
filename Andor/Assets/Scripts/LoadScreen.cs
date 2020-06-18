using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScreen : MonoBehaviour
{

    public GameObject screen1;
    public GameObject screen2;

    bool on;

    // Start is called before the first frame update
    public void Start()
{
   on = true;
    
}

public void onMouseDown()
{
    //Hide or show input field
    
    screen1.SetActive(on);
    screen2.SetActive(!on);
}
}
