using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;

/// <summary>
/// SceneViewWindow class.
/// </summary>
public class SceneViewWindow : EditorWindow
{
    /// <summary>
    /// Tracks scroll position.
    /// </summary>
    private Vector2 scrollPos;

    /// <summary>
    /// Initialize window state.
    /// </summary>
    [MenuItem(ProjectUtilityConstants.MAIN_MENU + "/View/Scene Management")]
    internal static void Init()
    {
        // EditorWindow.GetWindow() will return the open instance of the specified window or create a new
        // instance if it can't find one. The second parameter is a flag for creating the window as a
        // Utility window; Utility windows cannot be docked like the Scene and Game view windows.
        var window = (SceneViewWindow)GetWindow(typeof(SceneViewWindow), false, "Scene View");
        window.position = new Rect(window.position.xMin + 100f, window.position.yMin + 100f, 200f, 400f);
    }

    string[] sceneNames;
    static int startingSceneIndex;
    static int lastOpenScene;

    void OnEnable()
    {
        EditorApplication.playModeStateChanged -= StateChange;
        EditorApplication.playModeStateChanged += StateChange;

        //if (UnityToolbarExtender.ToolbarExtender.RightToolbarGUI.Contains(OnToolbarExtendGUI))
        //    UnityToolbarExtender.ToolbarExtender.RightToolbarGUI.Remove(OnToolbarExtendGUI);
        //UnityToolbarExtender.ToolbarExtender.RightToolbarGUI.Add(OnToolbarExtendGUI);
    }

    void OnDisable()
    {
        EditorApplication.playModeStateChanged -= StateChange;
        //if (UnityToolbarExtender.ToolbarExtender.RightToolbarGUI.Contains(OnToolbarExtendGUI))
        //    UnityToolbarExtender.ToolbarExtender.RightToolbarGUI.Remove(OnToolbarExtendGUI);
    }

    private const int maxWidth = 40;
    private const int maxHeight = 60;

    //void OnToolbarExtendGUI()
    //{
    //    //GUILayout.FlexibleSpace();
    //    if (isPlayButtonVisible)
    //    {
    //        if (EditorApplication.isPlaying)
    //        {
    //            GUI.color = Color.red;
    //            if (GUILayout.Button(EditorGUIUtility.IconContent("d_PlayButton"),
    //                UnityToolbarExtender.ToolbarExtender.m_commandStyleMiddle, GUILayout.MaxWidth(maxWidth),
    //                GUILayout.MinHeight(maxHeight)))
    //            {
    //                isStopByWindow = true;
    //                EditorApplication.isPlaying = false;
    //            }
    //        }
    //        else
    //        {
    //            GUI.color = Color.green;
    //            if (GUILayout.Button(EditorGUIUtility.IconContent("d_PlayButton"),
    //                UnityToolbarExtender.ToolbarExtender.m_commandStyleMiddle, GUILayout.MaxWidth(maxWidth),
    //                GUILayout.MinHeight(maxHeight)))
    //            {
    //                PlayGameInEditor();
    //            }
    //        }
    //    }

    //    GUI.color = Color.white;
    //    if (isSceneButtonVisible)
    //    {
    //        int totalScenes = EditorBuildSettings.scenes.Length;
    //        if (totalScenes > 1)
    //        {
    //            GUILayout.Space(5);

    //            for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
    //            {
    //                var scene = EditorBuildSettings.scenes[i];
    //                GUIStyle style = UnityToolbarExtender.ToolbarExtender.m_commandStyleMiddle;
    //                if (i == 0)
    //                    style = UnityToolbarExtender.ToolbarExtender.m_commandStyleLeft;
    //                else if (i == totalScenes - 1)
    //                    style = UnityToolbarExtender.ToolbarExtender.m_commandStyleRight;

    //                if (scene.enabled)
    //                {
    //                    var sceneName = Path.GetFileNameWithoutExtension(scene.path);
    //                    var styleObj = new GUIStyle("Button") { stretchWidth = true, fixedWidth = 0 };
    //                    var pressed = GUILayout.Button(i + ": " + sceneName, styleObj);
    //                    if (pressed)
    //                    {
    //                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
    //                            EditorSceneManager.OpenScene(scene.path);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    //GUILayout.FlexibleSpace();
    //}


    void StateChange(PlayModeStateChange playModeStateChange)
    {
        StopGameInEditor();
    }

    /// <summary>
    /// Called on GUI events.
    /// </summary>
    internal void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        if (EditorBuildSettings.scenes.Length == 0)
        {
            GUILayout.Label("No Scenes found  In Build", EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();
            return;
        }

        this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, false, false);
        GUILayout.Label("Choose Starting Scene", EditorStyles.boldLabel);
        CheckIsListAvailableOrNot();

        startingSceneIndex = EditorPrefs.GetInt("_StartingScene", 0);
        startingSceneIndex = EditorGUILayout.Popup(startingSceneIndex, sceneNames);
        EditorPrefs.SetInt("_StartingScene", startingSceneIndex);

        Color backUpColor = GUI.color;
        if (EditorApplication.isPlaying)
        {
            GUI.color = Color.red;
            if (GUILayout.Button("Stop"))
            {
                isStopByWindow = true;
                EditorApplication.isPlaying = false;
            }
        }
        else
        {
            GUI.color = Color.green;
            if (GUILayout.Button("Play"))
                PlayGameInEditor();
        }
        GUI.color = backUpColor;

        GUILayout.Space(5);
        isPlayButtonVisible = GUILayout.Toggle(isPlayButtonVisible, "Display play button on top menu");
        isSceneButtonVisible = GUILayout.Toggle(isSceneButtonVisible, "Display scene button on top menu");
        GUILayout.Space(5);
        GUILayout.Label("Scenes In Build", EditorStyles.boldLabel);
        for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            var scene = EditorBuildSettings.scenes[i];
            if (scene.enabled)
            {
                var sceneName = Path.GetFileNameWithoutExtension(scene.path);
                var pressed = GUILayout.Button(i + ": " + sceneName,
                    new GUIStyle(GUI.skin.GetStyle("Button")) { alignment = TextAnchor.MiddleLeft });
                if (pressed)
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scene.path);
                    }
                }
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }


    [MenuItem(ProjectUtilityConstants.MAIN_MENU + "/Editor/Play And Stop Game &p")]
    static void SetPlayStopGameInEditor()
    {
        if (EditorApplication.isPlaying)
        {
            isStopByWindow = true;
            EditorApplication.isPlaying = false;
        }
        else
        {
            PlayGameInEditor();
        }
    }

    static bool isStopByWindow
    {
        get => EditorPrefs.GetBool("_IsStopByWindow", false);
        set => EditorPrefs.SetBool("_IsStopByWindow", value);
    }

    static bool isPlayButtonVisible
    {
        get => EditorPrefs.GetBool("_isPlayButtonVisible", true);
        set => EditorPrefs.SetBool("_isPlayButtonVisible", value);
    }
    static bool isSceneButtonVisible
    {
        get => EditorPrefs.GetBool("_isSceneButtonVisible", true);
        set => EditorPrefs.SetBool("_isSceneButtonVisible", value);
    }

    static void StopGameInEditor()
    {
        if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode && isStopByWindow)
        {
            isStopByWindow = false;
            lastOpenScene = EditorPrefs.GetInt("_LastModifiedScene", 0);
            if (lastOpenScene != startingSceneIndex && lastOpenScene != -1)
            {
                var s = EditorBuildSettings.scenes[lastOpenScene];
                EditorSceneManager.OpenScene(s.path);
            }
        }
    }


    static void PlayGameInEditor()
    {
        isStopByWindow = true;
        var activeScene = EditorSceneManager.GetActiveScene();
        EditorPrefs.SetInt("_LastModifiedScene", activeScene.buildIndex);
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.SaveScene(activeScene);
        }

        var s = EditorBuildSettings.scenes[startingSceneIndex];
        if (activeScene.buildIndex != startingSceneIndex)
        {
            EditorSceneManager.OpenScene(s.path);
        }

        EditorApplication.isPlaying = true;
    }

    void CheckIsListAvailableOrNot()
    {
        if (sceneNames != null)
            return;

        sceneNames = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < sceneNames.Length; i++)
        {
            var scene = EditorBuildSettings.scenes[i];
            sceneNames[i] = Path.GetFileNameWithoutExtension(scene.path);
        }

        startingSceneIndex = EditorPrefs.GetInt("_StartingScene", 0);
        lastOpenScene = EditorPrefs.GetInt("_LastModifiedScene", 0);
    }
}
