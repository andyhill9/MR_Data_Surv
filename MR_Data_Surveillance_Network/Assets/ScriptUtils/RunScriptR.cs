using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class RunScriptR : MonoBehaviour
{
    System.Diagnostics.Process process;
    string RConfigPath = Application.streamingAssetsPath + @"/configR.txt";
    ConfigData configData;

    [System.Serializable]
    class ConfigData {
        public string RScriptExexutablePath = "";
        public string ScriptPath = "";
        public string DataSavePath = "";
        public char DataDelimiter = ',';
    }
    void Awake()
    {
        if (File.Exists(RConfigPath))
        {
            configData = ConfigJSON.LoadFromJSON<ConfigData>(RConfigPath); 
        }
        else
        {   // Provide a form of Config recreation either an individual template file or just create an empty when no config is available
            ConfigJSON.SaveToJSON<ConfigData>(RConfigPath, new ConfigData());
            Debug.LogError("No config data available in " + RConfigPath + " a new empty config was created please fill out the empty parameter");
            System.Console.WriteLine("No config data available in " + RConfigPath + " a new empty config was created please fill out the empty parameter");
            Application.Quit();
        }

    }
    private void Start()
    {
        process = GetProcess(configData.RScriptExexutablePath, configData.ScriptPath, Application.dataPath);
        process.Start();
        process.WaitForExit();
        GenericParsing.GenericParserAdapter parser = new GenericParsing.GenericParserAdapter(configData.DataSavePath) { ColumnDelimiter = configData.DataDelimiter };
        System.Data.DataTable dsResult = parser.GetDataTable();
        Debug.Log(dsResult.Rows[0][0]);

    }
    /// <summary>
    /// Returns a process to the respective command in order to be run. In our case it will be mostly used to run an R script on the server
    /// </summary>
    /// <param name="fileName">Path to the program which should be started e.g. Rscript</param>
    /// <param name="args">Arguments presented to the program in case of Rscript that would be the path to the script itself</param>
    /// <param name="workingDir">Working directory of the program in most cases not really important as long as absolute paths will be used relative paths will start from here</param>
    /// <returns>Returns a process which can be started by Process.Start() </returns>
    private System.Diagnostics.Process GetProcess(string fileName, string args, string workingDir)
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.FileName = fileName;// @"C:\Program Files\R\R-4.0.2\bin\x64\Rscript.exe";
        process.StartInfo.Arguments = args;//"test.R";
        process.StartInfo.WorkingDirectory = workingDir;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        return process;
    }
}
