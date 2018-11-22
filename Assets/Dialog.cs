using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Dialog : MonoBehaviour {

    public ShellManager sm;

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
        if(IN)
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
