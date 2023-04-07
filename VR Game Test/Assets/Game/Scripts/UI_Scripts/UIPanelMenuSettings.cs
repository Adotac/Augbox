using FishNet;
using FishNet.Managing.Scened;
using FishNet.Plugins.FishyEOS.Util;
using System;
namespace Augbox
{
    public class UIPanelMenuSettings : UIPanelSettings
    {
        public void Back()
        {
            UIPanelManager.Instance.HidePanel<UIPanelMenuSettings>(false);
        }
    }
}