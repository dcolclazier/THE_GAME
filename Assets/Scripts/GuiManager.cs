using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GuiManager :MonoBehaviour
{
    public void exit_click()
    {
        Debug.Log("Exit button clicked!");
        Application.Quit();
    }

    public void singlep_click()
    {
        Debug.Log("Single player button clicked.");
    }

    public void multip_click()
    {
        Debug.Log("Multiplayer button clicked.");
    }

    public void options_click()
    {
        Debug.Log("Options button clicked.");
    }


}
