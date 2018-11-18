using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;
using System.Threading;
using UnityEngine.UI;

public  class ShellManager : MonoBehaviour {

    public static string inUrl;
    public static string outUrl;
    public static string pngUrl;
    public static string ffmpegUrl;
    public Text inText;
    public Text outText;
    public Text pngText;

    public void Awake()
    {
        pngUrl = Application.streamingAssetsPath + "/png/";
        ffmpegUrl = Application.streamingAssetsPath + "/ffmpeg/ffmpeg.exe";
    }
    public void Update()
    {
        inText.text = inUrl;
        outText.text = outUrl;
        pngText.text = pngUrl;
        
    }

    public void btn()
    {
        ThreadStart threadStart = new ThreadStart(Transform);
        Thread thread = new Thread(threadStart);
        thread.Start();
        
    }


    public void Transform()
        {
            ExcuteProcess(ffmpegUrl, " -y -i "+inUrl+" -f image2 "+ pngUrl + "%03d.png", (s, e) => UnityEngine.Debug.Log(e.Data));
            ExcuteProcess(ffmpegUrl, " -y -f image2 -i "+ pngUrl+"%03d.png -auto-alt-ref 0 -c:v libvpx -b:v 20000k "+ outUrl, (s, e) => UnityEngine.Debug.Log(e.Data));
             UnityEngine.Debug.Log("OK");
        }

        private static void ExcuteProcess(string exe, string arg, DataReceivedEventHandler output)
        {
            using (var p = new Process())
            {
                p.StartInfo.FileName = exe;
                p.StartInfo.Arguments = arg;

                p.StartInfo.UseShellExecute = false;    //输出信息重定向
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;

                p.OutputDataReceived += output;
                p.ErrorDataReceived += output;

                p.Start();                    //启动进程
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                Thread.Sleep(2000);
                p.WaitForExit();            //等待进程结束
                
                p.Close();
                UnityEngine.Debug.Log("TransformSucceed");
            }
        }
    

}
