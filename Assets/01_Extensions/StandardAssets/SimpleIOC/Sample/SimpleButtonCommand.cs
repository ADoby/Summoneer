using System.Collections;
using UnityEngine;

public class SimpleButtonCommand : SimpleCommand
{
    public override void Execute()
    {
        base.Execute();
        Debug.Log("ButtonCommand");
    }
}