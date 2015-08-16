using System;
using UnityEngine;
using Assets.Code;

// ReSharper disable once CheckNamespace (only because Monobehaviour can't be in a custom namespace)
public class MainController : MonoBehaviour {
    //static instance of this class
    private static MainController _mainController;

    //name of scenes
    private string _currentSceneName;
    private string _nextSceneName;

    //holds the loading/unloading tasks so execution can be paused during scene load - OMG IM SO GLAD I FOUND THIS
    private AsyncOperation _resourceUnloadTask;
    private AsyncOperation _sceneLoadTask;

    //possible states a particular Unity scene can be in
    private enum SceneState { Reset, Preload, Load, Unload, PostLoad, Ready, Run, Count };

    private SceneState _currentSceneState;

    //delegate holds required actions for each scene-state
    private delegate void UpdateDelegate();

    //an array of update delegates - one element for each scene state
    private UpdateDelegate[] _updateDelegates;


    //public method to allow other controllers to switch the current scene
    public static void SwitchScene(string nextSceneName) {
        if (_mainController == null) return;
        if (nextSceneName == _mainController._currentSceneName) return;

        _mainController._nextSceneName = nextSceneName;

    }

    //sets scene to main menu by default
    public void Awake() {
        //keep alive
        DontDestroyOnLoad(gameObject);
        _mainController = this;

        _updateDelegates = new UpdateDelegate[(int)SceneState.Count];

        _updateDelegates[(int)SceneState.Reset] = SceneStateReset;
        _updateDelegates[(int)SceneState.Preload] = SceneStatePreload;
        _updateDelegates[(int)SceneState.Load] = SceneStateLoad;
        _updateDelegates[(int)SceneState.Unload] = SceneStateUnload;
        _updateDelegates[(int)SceneState.PostLoad] = ScreenStatePostLoad;
        _updateDelegates[(int)SceneState.Ready] = ScreenStateReady;
        _updateDelegates[(int)SceneState.Run] = ScreenStateRun;

        _nextSceneName = "MainMenu";

        _currentSceneState = SceneState.Reset;

    }

    //first step - do a GC.collect!
    private void SceneStateReset(){
        System.GC.Collect();
        NodeManager.ClearEntities();
        Messenger.Cleanup();
        _currentSceneState = SceneState.Preload;
    }

    //start loading the next scene asynchrounously 
    private void SceneStatePreload()
    {
        _sceneLoadTask = Application.LoadLevelAsync(_nextSceneName);
        _currentSceneState = SceneState.Load;
    }

    //run this while the scene is still loading
    private void SceneStateLoad()
    {
        if (_sceneLoadTask.isDone)
        {
            _currentSceneState = SceneState.Unload;
        }
        else
        {
            //update loading stuff (progress bar, etc)
        }
    }

    //suggested by Unity developers to do on every scene change... not sure why, but w/e
    private void SceneStateUnload()
    {
        if (_resourceUnloadTask == null) _resourceUnloadTask = Resources.UnloadUnusedAssets();
        else
        {
            if (_resourceUnloadTask.isDone != true) return;

            _resourceUnloadTask = null;
            _currentSceneState = SceneState.PostLoad;
        }
    }

    //handle anything that needs to happen immediately after scene finishes loading
    private void ScreenStatePostLoad()
    {
        _currentSceneName = _nextSceneName;
        _currentSceneState = SceneState.Ready;
    }

    //scene is loaded - last chance to GC.
    private void ScreenStateReady()
    {
        //System.GC.Collect(); // Only include this after you're sure all assets are in use when a scene is loaded - otherwise don't
        _currentSceneState = SceneState.Run;
    }

    //scene is running - only do anything if something changes the scene name to something other than the current scene name.
    private void ScreenStateRun() {
        if (_currentSceneName != _nextSceneName)
            _currentSceneState = SceneState.Reset;
    }

    //run the current scene-state's delegate function every frame
    protected void Update() {
        if (_updateDelegates[(int)_currentSceneState] != null)
            _updateDelegates[(int)_currentSceneState]();
    }

    //for the gc
    protected void OnDestroy() {
        if (_updateDelegates != null) {
            for (var i = 0; i < (int)SceneState.Count; i++) {
                _updateDelegates[i] = null;
            }
            _updateDelegates = null;
        }
        _mainController = null;
    }


}