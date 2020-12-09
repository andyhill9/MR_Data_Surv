using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

public class BuildScript : EditorWindow
{
    [MenuItem("Window/BuildScript")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(BuildScript));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Build ALL"))
        {
            BuildAll();
        }
        if (GUILayout.Button("Build Windows!"))
        {
            BuildWin();
        }
        if (GUILayout.Button("Build Windows Server!"))
        {
            BuildWinServer();
        }
        if (GUILayout.Button("Build HoloLens!"))
        {
            BuildHoloLens();
        }
        if (GUILayout.Button("Build Linux Server!"))
        {
            BuildLinuxServer();
        }
    }
    public static void BuildAll() {
        BuildWin();
        BuildWinServer();
        BuildLinux();
        BuildLinuxServer();
        BuildHoloLens();
    }
    public static void BuildWin() {
        string name = "Build Windows";
        string path = "Build/Win/" + Application.productName + ".exe";
        Debug.Log(name);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            //scenes = new[] { "Assets/Scene1.unity", "Assets/Scene2.unity" };
            locationPathName = path,
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log(name + " succeeded");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log(name + " failed");
        }

    }
    public static void BuildWinServer()
    {
        string name = "Build Windows Server";
        string path = "Build/WinServer/" + Application.productName + ".exe";
        Debug.Log(name);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            //scenes = new[] { "Assets/Scene1.unity", "Assets/Scene2.unity" };
            locationPathName = path,
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.EnableHeadlessMode
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log(name + " succeeded");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log(name + " failed");
        }
    }
    public static void BuildLinux()
    {
        string name = "Build Linux";
        string path = "Build/Linux/" + Application.productName + ".64_86";
        Debug.Log(name);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            //scenes = new[] { "Assets/Scene1.unity", "Assets/Scene2.unity" };
            locationPathName = path,
            target = BuildTarget.StandaloneLinux64,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log(name + " succeeded");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log(name + " failed");
        }
    }
    public static void BuildLinuxServer()
    {
        string name = "Build Linux Server";
        string path = "Build/LinuxServer/" + Application.productName + ".64_86";
        Debug.Log(name);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            //scenes = new[] { "Assets/Scene1.unity", "Assets/Scene2.unity" };
            locationPathName = path,
            target = BuildTarget.StandaloneLinux64,
            options = BuildOptions.EnableHeadlessMode
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log(name + " succeeded");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log(name + " failed");
        }
    }
    public static void BuildHoloLens()//TODO set options for hololens
    {
        string name = "Build HoloLens";
        string path = "Build/HoloLens/" + Application.productName;
        Debug.Log(name);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            //scenes = new[] { "Assets/Scene1.unity", "Assets/Scene2.unity" };
            locationPathName = path,
            target = BuildTarget.WSAPlayer,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log(name + " succeeded");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log(name + " failed");
        }
    }
}
