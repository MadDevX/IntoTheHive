using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;

public class CmdServer : IDisposable
{
    private Settings _settings;
    private Process _serverProcess = null;
    private bool _initialized = false;

    public CmdServer(Settings settings)
    {
        _settings = settings;
        InitializeServerProcessSettings();;
    }

    public bool Create()
    {
        if (PortAvailable())
        {
            if (_initialized == false)
            {
                _serverProcess.Start();
                _initialized = true;
            }
            else if (_serverProcess.HasExited)
            {
                _serverProcess.Start();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Close()
    {
        if (_initialized && _serverProcess.HasExited == false)
        {
            _serverProcess.CloseMainWindow();
        }
    }
    
    private void InitializeServerProcessSettings()
    {
        _serverProcess = new Process();
        _serverProcess.StartInfo.FileName = GetServerPath();
        _serverProcess.StartInfo.WorkingDirectory = GetServerWorkingDirectory();
        _serverProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    }

    private string GetServerWorkingDirectory()
    {
        var path = Application.dataPath; // returns /Assets or /ProjectName_data
        path = path.Substring(0, path.LastIndexOf('/') + 1) + "Server/"; //returns RootFolder/Server (in which server exe should exist)
        return path;
    }

    private string GetServerPath()
    {
        return GetServerWorkingDirectory() + _settings.serverFileName;
    }

    private bool PortAvailable()
    {
        int port = 4296;
        bool isAvailable = true;
        IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

        foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
        {
            if (tcpi.LocalEndPoint.Port == port)
            {
                isAvailable = false;
                break;
            }
        }
        return isAvailable;
    }

    public void Dispose()
    {
        Close();
    }

    [System.Serializable]
    public class Settings
    {
        public string serverFileName;
    }
}
