using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    private static MenuController _menuController;

    public Canvas QuitMenu;
    public Button SingleP;
    public Button MultiP;
    public Button Options;
    public Button Cancel;
    public Button Exit;
    public Button Confirm;

	// Use this for initialization
	protected void Awake () {
	    _menuController = this;

	    QuitMenu = QuitMenu.GetComponent<Canvas>();
	    SingleP = SingleP.GetComponent<Button>();
	    MultiP = MultiP.GetComponent<Button>();
	    Exit = Exit.GetComponent<Button>();
	    QuitMenu.enabled = false;

	}

    protected void OnDestroy() {
        _menuController = null;
    }

    public void ExitPress() {
        
        SingleP.enabled = MultiP.enabled = Options.enabled = Exit.enabled = false;
        QuitMenu.enabled = true;

    }

    public void ConfirmPress() {
        Application.Quit();
    }

    public void CancelPress() {
        SingleP.enabled = MultiP.enabled = Options.enabled = Exit.enabled = true;
        QuitMenu.enabled = false;
    }

    public void SinglepPress() {

        MainController.SwitchScene("Test Scene");
    }

    public void MultipPress() {
        //load network lobby
    }

    public void OptionsPress() {
        //load options menu
    }
  
	// Update is called once per frame
	void Update () {
	
	}
}
