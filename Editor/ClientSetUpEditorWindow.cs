using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using Sirenix.OdinInspector;
using FMETP;
using static FMETP.FMNetworkManager;

public class ClientSetUpEditorWindow : OdinEditorWindow
{

    [SceneObjectsOnly]
    [HorizontalGroup("SplitH1")]
    public Camera playerCamera;

    [SceneObjectsOnly]
    [HorizontalGroup("SplitH2")]
    public GameObject playerParentObject;


    [MenuItem("Tools/VR-Guide Tablet/Set-Up/Client")]
    private static void OpenWindow()
    {

        GetWindow<ClientSetUpEditorWindow>().Show();

    }

    [HorizontalGroup("SplitH1")]
    [Button("Get Main Camera")]
    public void GetMainCam()
    {

        playerCamera = Camera.main;

    }

    [Button()]
    public void SetUpClient()
    {

        if (playerCamera == null || playerParentObject == null)
            return;

        FMNetworkManager fMNetworkManager = playerParentObject.AddComponent<FMNetworkManager>();
        GameViewEncoder encoder = playerParentObject.AddComponent<GameViewEncoder>();
        GuideTabletClient client = playerParentObject.AddComponent<GuideTabletClient>();

        fMNetworkManager.AutoInit = true;
        fMNetworkManager.NetworkType = FMNetworkType.Client;

        FMClientSettings newSettings = new FMClientSettings();

        newSettings.ClientListenPort = 3334;
        newSettings.UseMainThreadSender = false;
        newSettings.AutoNetworkDiscovery = true;

        fMNetworkManager.ClientSettings = newSettings;

        encoder.CaptureMode = GameViewCaptureMode.RenderCam;
        encoder.RenderCam = playerCamera;
        encoder.Resolution = new Vector2(512, 256);
        encoder.MatchScreenAspect = true;
        encoder.Quality = 25;
        encoder.StreamFPS = 24;
        encoder.GZipMode = true;
        encoder.OutputFormat = GameViewOutputFormat.FMMJPEG;
        encoder.OutputAsChunks = true;
        encoder.OnDataByteReadyEvent.AddListener(fMNetworkManager.SendToServer);
        encoder.FastMode = true;
        encoder.AsyncMode = true;
        encoder.EnableAsyncGPUReadback = true;
        encoder.ChromaSubsampling = FMChromaSubsamplingOption.Subsampling420;

    }

}