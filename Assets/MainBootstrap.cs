using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBootstrap : SimpleBootstrap
{
    protected override void Awake()
    {
        Instance = this;
        simpleContext = new MainContext(this);
        context = simpleContext;
    }
}