namespace Augbox
{
    public class UIPanelGameSettings : UIPanelSettings
    { 
        
        public void Back()
        {
            UIPanelManager.Instance.HidePanel<UIPanelGameSettings>(false);
        }
    }
}