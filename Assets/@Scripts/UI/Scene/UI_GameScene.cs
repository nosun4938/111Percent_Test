using UnityEngine;
using UnityEngine.EventSystems;

public class UI_GameScene : UI_Scene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Refresh();
        
        return true;
    }

    private void Update()
    {
        
    }
    
    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;
    }
}