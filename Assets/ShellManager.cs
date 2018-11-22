using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;
using System.Threading;
using UnityEngine.UI;
using System;

public class ShellManager : MonoBehaviour
{
    public static VideoType.TypeOfVideo InVideoType;
    public static VideoType.TypeOfVideo OutVideoType;
    public static string inUrl;
    public static string outUrl;
    public static string pngUrl;
    public static string ffmpegUrl;
    public Text inText;
    public Text outText;
    public Text pngText;
    public Text status;
    private string Sstatus;
    private string OutArgument;
    public bool isChooseFolder;
     


    public void Awake()
    {
        pngUrl = Application.streamingAssetsPath + "/png/";
        ffmpegUrl = Application.streamingAssetsPath + "/ffmpeg/ffmpeg.exe";
        OutArgument = "-auto-alt-ref 0 -c:v libvpx -b:v 20000k ";
    }

    public void ChooseFolderAgument(bool ChooseFolder)
    {
        isChooseFolder = ChooseFolder;
    }

    public void ChooseInType(Text txt)
    {
        InVideoType = (VideoType.TypeOfVideo)Enum.Parse(typeof(VideoType.TypeOfVideo), txt.text, false);
        //选择输出png序列帧语句
    }

    public void ChooseOutType(Text txt)
    {
        OutVideoType = (VideoType.TypeOfVideo)Enum.Parse(typeof(VideoType.TypeOfVideo), txt.text, false); 
        UnityEngine.Debug.Log(OutVideoType.ToString());
        //选择输出video语句
        switch(OutVideoType)
        {
            case VideoType.TypeOfVideo.webm:
                OutArgument = "-auto-alt-ref 0 -c:v libvpx -b:v 20000k ";
                //OutArgument = "-c：v libvpx-vp8 -pix_fmt yuva420p -metadata：s：v：0 alpha_mode =\"1\" ";
                break;
            case VideoType.TypeOfVideo.avi:
                OutArgument = "-c:v libx264 -b:v 20000k ";
                break;
            case VideoType.TypeOfVideo.mov:
                OutArgument = "-c:v libx264 -b:v 20000k ";
                break;
            case VideoType.TypeOfVideo.mp4:
                OutArgument = "-c:v libx264 -b:v 20000k ";
                break;
            default:
                break;
        }
    }
    public void Update()
    {
        inText.text = inUrl;
        outText.text = outUrl;
        pngText.text = pngUrl;
        status.text = Sstatus; 
    }

    public void btn()
    {
        ThreadStart threadStart = new ThreadStart(Transform);
        Thread thread = new Thread(threadStart);
        thread.Start(); 
    }


    private void ClearFolder(string dir)
    {
        foreach (string d in Directory.GetFileSystemEntries(dir))
        {
            if (File.Exists(d))
            {
                FileInfo fi = new FileInfo(d);
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    fi.Attributes = FileAttributes.Normal;
                File.Delete(d);//直接删除其中的文件  
            }
        }
    }

    public void Transform()
    {
        if (!isChooseFolder)
        {
            if (!Directory.Exists(pngUrl))
            {
                Directory.CreateDirectory(pngUrl);
            }
            Sstatus = "正在转换，请稍后...";
            ExcuteProcess(ffmpegUrl, " -y -i " + inUrl + " -f image2 " + pngUrl + "%03d.png", (s, e) => UnityEngine.Debug.Log(e.Data));
            ExcuteProcess(ffmpegUrl, " -y -f image2 -i " + pngUrl + "%03d.png "+ OutArgument + outUrl + "." + OutVideoType.ToString(), (s, e) => UnityEngine.Debug.Log(e.Data));
            //清空图片缓存文件夹
            ClearFolder(pngUrl);
            UnityEngine.Debug.Log("OK");
            Sstatus = "转换成功！";
        }
        else
        {    
            //遍历指定路径下的所有指定类型的文件
            ForeachFile(inUrl);
            int count = videoPaths.Count;
            UnityEngine.Debug.Log(videoPaths.Count);
            for (int i = 0; i < count; i++)
            {
                if (!Directory.Exists(pngUrl))
                {
                    Directory.CreateDirectory(pngUrl);
                }
                Sstatus = "正在转换 " + i.ToString() + "/" + count.ToString() + " ，请稍后...";
                ExcuteProcess(ffmpegUrl, " -y -i " + videoPaths[i] + " -f image2 " + pngUrl + "%03d.png", (s, e) => UnityEngine.Debug.Log(e.Data));
                ExcuteProcess(ffmpegUrl, " -y -f image2 -i " + pngUrl + "%03d.png "+ OutArgument + outUrl + "\\" + videoName[i] + "." + OutVideoType.ToString(), (s, e) => UnityEngine.Debug.Log(e.Data));
                //UnityEngine.Debug.LogError(" -y  -i " + videoPaths[i] + " -f image2 " + pngUrl + "%03d.png。\n" + " -y -i " + pngUrl + "%03d.png " + OutArgument + outUrl + "\\" + videoName[i] + "." + OutVideoType.ToString());
                ClearFolder(pngUrl);
            }
            Sstatus = "转换成功!";
        }
    }
    private List<string> videoPaths;
    private List<string> videoName;
    private void ForeachFile(string path)
    {
        videoPaths = new List<string>();
        videoName = new List<string>();
        DirectoryInfo dir = new DirectoryInfo(path);//查找某个文件夹下的所有文件
        FileInfo[] inf = dir.GetFiles();
        foreach (FileInfo info in inf)
        {
            if (info.Extension.Equals("." + InVideoType.ToString()))
            {
                videoPaths.Add(info.FullName);
                string name = info.Name.Replace("." + InVideoType.ToString(), "");
                videoName.Add(name);
                UnityEngine.Debug.Log(name);
                UnityEngine.Debug.Log(info.FullName);
            }
        }  
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
