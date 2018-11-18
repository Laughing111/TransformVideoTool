using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Dialog : MonoBehaviour {

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
        }
    }
}
