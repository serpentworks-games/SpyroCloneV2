using ScalePact.Utils;
using UnityEngine;

public class PrintDebugMessage : MonoBehaviour
{
    [SerializeField] string messageToPrint;

    public void PrintMessage()
    {
        Debug.Log(messageToPrint);
    }
}