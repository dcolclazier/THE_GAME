using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using Assets.Code.Abstract;
using Assets.Code.States;

// ReSharper disable once CheckNamespace (only because Monobehaviour can't be in a custom namespace)

public class GameManager : MonoBehaviour
{
    //PARAMS: old state, new state
    public event Action<IEngineState, IEngineState> StateChanged; 

    private IEngineState _currentState;

    public void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        
    }
	//Begin pre-loading phase
	public void Start () {
        Debug.Log("Got here! GameManager loading prestart state.");
        ChangeState(new PreStartState(this));
	}
	
	// Update is called once per frame
	public void Update () {
	    _currentState.Update();
	}

    public void ChangeState(IEngineState newState)
    {
        var oldState = _currentState;
        if(oldState != null) oldState.Deactivate();

        _currentState = newState;
        _currentState.Activate();

        //trigger state-changed event - old state necessary? trigger necessary?
        if (StateChanged != null) StateChanged(oldState, newState);
    }
}