using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {

    public ShellManager sm;
    public Text InNote;
    public GameObject inin;
    public Text Start;
    public Text Save;
    public GameObject isPNG;

    public void OutPut()
    {
       if(sm.isChooseFolder)
        {
            ChooseFolder(false);
        }
        else
        {
            SaveFile();
        }
    }

	public void SaveFile()
    {
        OpenFile openFile = new OpenFile();
        openFile.structSize = Marshal.SizeOf(openFile);
        openFile.file = new string(new char[256]);
        openFile.maxFile = openFile.file.Length;
        openFile.filter = "All Files\0*.*\0\0";
        openFile.fileTitle = new string(new char[64]);
        openFile.maxFileTitle = openFile.fileTitle.Length;
        openFile.initialDir = Application.streamingAssetsPath.Replace('/', '\\');
        openFile.title = "选择视频";
        openFile.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
        if(LocalDialog.GetSaveFileName(openFile))
        {
            Debug.Log(openFile.file);
            ShellManager.outUrl = openFile.file;
        }
    }

    public void ChooseFile()
    {   
        OpenFile openFile = new OpenFile();
        openFile.structSize = Marshal.SizeOf(openFile);
        openFile.file = new string(new char[256]);
        openFile.maxFile = openFile.file.Length;
        openFile.filter = "All Files\0*.*\0\0";
        openFile.fileTitle = new string(new char[64]);
        openFile.maxFileTitle = openFile.fileTitle.Length;
        openFile.initialDir = Application.streamingAssetsPath.Replace('/', '\\');
        openFile.title = "选择视频";
        openFile.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
        if (LocalDialog.GetOpenFileName(openFile))
        {
            Debug.Log(openFile.file);
            ShellManager.inUrl = openFile.file;
            sm.ChooseFolderAgument(false);
            isPNG.SetActive(true);
            InNote.text = "选择视频为：";
            Start.text = "开始转换";
            Save.text = "保存为";
            inin.transform.localPosition = new Vector3(120, inin.transform.localPosition.y, inin.transform.localPosition.z);
        }
    }


    public void ChooseFolder(bool IN)
    {   
        //选择文件路径
        OpenDialogDir openFile = new OpenDialogDir();
        openFile.pszDisplayName = new string(new char[2000]);
        openFile.lpszTitle = "选择视频文件夹"; 
        IntPtr pidIPtr = LocalDialog.SHBrowseForFolder(openFile);
        char[] charArray = new char[2000];
        for(int i=0;i<2000;i++)
        {
            charArray[i] = '\0';
        }
        LocalDialog.SHGetPathFromIDList(pidIPtr, charArray);
        string fullDirPath = new string(charArray);
        fullDirPath = fullDirPath.Substring(0, fullDirPath.IndexOf('\0'));
        Debug.Log(fullDirPath);
        InNote.text = "视频格式为：";
        Start.text = "开始批量转换";
        Save.text = "输出至";
        isPNG.SetActive(false);
        inin.transform.localPosition = new Vector3(231, inin.transform.localPosition.y, inin.transform.localPosition.z);
        if (IN)
        {
            ShellManager.inUrl = fullDirPath;
            sm.ChooseFolderAgument(true);
            
        }
        else
        {
            ShellManager.outUrl = fullDirPath;
        }  
    }  
}
