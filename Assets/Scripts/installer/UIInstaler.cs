using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reflex.Scripts;
using Reflex;

public class UIInstaler : Installer
{
    [SerializeField]
    private WarningUI warningUI;

    public override void InstallBindings(Container container)
    {
        Debug.Assert(this.warningUI != null);
        container.BindInstanceAs(this.warningUI);
    }
}
