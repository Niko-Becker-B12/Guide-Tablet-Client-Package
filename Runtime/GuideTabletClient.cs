using FMETP;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GuideTabletClient : MonoBehaviour
{

    public int playerID;

    public string customID;

    public TMP_InputField t;

    public TextMeshProUGUI text;

    public UnityEvent<string> OnEventCallReceived;


    public void SetNewPlayerID(string newID)
    {

        int.TryParse(newID, out playerID);


        this.GetComponent<GameViewEncoder>().label = (ushort)(1000 + playerID + 1);

        SendMessageToServer($"CustomID({t.text})");

        text.text = $"PlayerID: {playerID} - {t.text}";

    }

    public void ReceiveMessageFromServer(string message)
    {

        Debug.Log(message);
        text.text += $"\n{message}";

        if(message.Contains("COMMAND_ToClient_"))
        {
            if(message.Contains("PlayerID"))
            {

                string[] s = message.Split('(');
                s[1] = s[1].Replace(")", "");
                int.TryParse(s[1], out playerID);

                SetNewPlayerID(s[1]);

            }
            if (message.Contains("Event"))
            {

                string[] s = message.Split('(');
                s[1] = s[1].Replace(")", "");

                OnEventCallReceived.Invoke(s[1]);

            }

        }

    }

    [Button]
    public void SendMessageToServer(string message)
    {
        //String Format: "COMMAND_ToServer_{playerID}_{message}"
        //Invoke this, if a Player starts/finishes a Level or the CustomID changes
        //Switch Scene call: SwitchedScene(int index) -> index==-1 should be the main scene/start room

        Debug.Log($"{message}");

        FMNetworkManager.instance.SendToServerReliable($"COMMAND_ToServer_{playerID}_{message}");

    }

}