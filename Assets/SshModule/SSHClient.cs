using Renci.SshNet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class SSHClient : MonoBehaviour
{

    #region private members 	
    private Thread _rebootDeviceThread;
    private Thread _restartServiceThread;
    #endregion
    public void RestartService()
    {
        try
        {
            Invoke("RestartConnection", 5);
            _restartServiceThread = new Thread(new ThreadStart(_restartService));
            _restartServiceThread.IsBackground = true;
            _restartServiceThread.Start();
        }
        catch (System.Exception e)
        {
            Debug.Log("On RestartService exception " + e);
        }
    }
    public void RebootDevice()
    {
        try
        {
            _rebootDeviceThread = new Thread(new ThreadStart(_rebootDevice));
            _rebootDeviceThread.IsBackground = true;
            _rebootDeviceThread.Start();
        }
        catch (System.Exception e)
        {
            Debug.Log("On RebootDevice exception " + e);
        }
    }

    public void _restartService()
    {
        using (var client = new SshClient("10.42.0.1", "root", "nvidia"))
        {
            client.Connect();

            using (var command = client.CreateCommand("systemctl restart mower"))
            {
                Debug.Log(command.Execute()); //Don't forget to Execute the command
            }

            client.Disconnect();
        }
    }
    public void _rebootDevice()
    {
        using (var client = new SshClient("10.42.0.1", "root", "nvidia"))
        {
            client.Connect();

            using (var command = client.CreateCommand("reboot"))
            {
                Debug.Log(command.Execute()); //Don't forget to Execute the command
            }

            client.Disconnect();
        }
    }
    public void RestartConnection()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(scene.name);
    }
}
