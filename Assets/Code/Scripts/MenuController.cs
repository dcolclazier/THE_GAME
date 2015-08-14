using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{

    private static MenuController _menuController;

	// Use this for initialization
	protected void Awake ()
	{
	    _menuController = this;
	}

    protected void OnDestroy()
    {
        _menuController = null;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
