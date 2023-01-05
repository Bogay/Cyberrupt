using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reflex;
using Reflex.Scripts;

public class BattleInstaller : Installer
{
    [SerializeField]
    private Player player;

    public override void InstallBindings(Container container)
    {
        Debug.Assert(this.player != null);
        container.BindInstanceAs(this.player);
        Debug.Log("Player installed");
    }
}
