
using UnityEngine;

using ToolbarControl_NS;

namespace ModActions
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(ModActionsEditor.MODID, ModActionsEditor.MODNAME);
        }
    }
}
