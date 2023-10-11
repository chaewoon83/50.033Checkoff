using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MushroomEvent : MonoBehaviour
{

    public UnityEvent AnimationEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UseEvent()
    {
        AnimationEvent.Invoke();
    }
}
