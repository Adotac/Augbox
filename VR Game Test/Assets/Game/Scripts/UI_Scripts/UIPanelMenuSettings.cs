
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