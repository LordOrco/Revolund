using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public InputActionReference MoveRef, JumpRef, FireRef;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        MoveRef.action.Disable();
        JumpRef.action.Disable();
        FireRef.action.Disable();
    }
    private void Diseable()
    {
        MoveRef.action.Enable();
        JumpRef.action.Enable();
        FireRef.action.Enable();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
