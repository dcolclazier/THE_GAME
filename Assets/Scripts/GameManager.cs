using System.CodeDom;
using UnityEngine;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Abstract;
using States;

// ReSharper disable once CheckNamespace (only because Monobehaviour can't be in a custom namespace)
public class GameManager : MonoBehaviour
{

    private IEngineState _currentState;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        
    }
	// Use this for initialization
	void Start () {
        _currentState = new SandboxState(this);
	}
	
	// Update is called once per frame
	void Update () {
	    _currentState.Update();
	}

    
}
