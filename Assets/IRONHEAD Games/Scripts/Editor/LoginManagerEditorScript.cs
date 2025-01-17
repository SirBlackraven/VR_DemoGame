﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoginManager))]
public class LoginManagerEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This script is for connecting to Photon sever", MessageType.Info);

        LoginManager loginManager = (LoginManager)target;
        if(GUILayout.Button("Connect Anonymously"))
        {
            loginManager.ConnectToPhotonServer();
        }
    }
}
