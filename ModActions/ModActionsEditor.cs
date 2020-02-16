using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP.UI.Screens;

using ToolbarControl_NS;

namespace ModActions
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class ModActionsEditor : PartModule
    {
        private bool buttonCreated = false; MainGUIWindow ourWin;
        ConfigNode settings;
        float winTop;
        float winLeft;
        Part lastSelectedPart;
        bool showWin;
        float lastUpdateTime;

        public void Start()
        {
            GameEvents.onEditorScreenChange.Add(WinChangeAction);
            settings = ConfigNode.Load(KSPUtil.ApplicationRootPath + "GameData/ModActions/ModActions.settings");
            winTop = float.Parse(settings.GetValue("EdWinTop"));
            winLeft = float.Parse(settings.GetValue("EdWinLeft"));

            AddButtons();
        }
        internal ToolbarControl toolbarControl;
        internal const string MODID = "ModActions";
        internal const string MODNAME = "Mod Actions";

        void AddButtons()
        {

            if (!buttonCreated)
            {
                buttonCreated = true;

                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(onStockToolbarClick, onStockToolbarClick,
                    ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH,
                    MODID,
                    "maButton",
                    "ModActions/PluginData/BtnStock",
                    "ModActions/PluginData/Btn",
                    MODNAME
                );

            }
        }

        public void onStockToolbarClick()
        {
            //Debug.Log("Modacts buton clik");
            showWin = !showWin;
            if (ourWin != null)
            {
                ourWin.drawWin = showWin;
            }
        }

        public void DummyVoid()
        {

        }

        public void OnGUI()
        {
            //Debug.Log("Modacts calling GUI!" + ourWin.copyMode);
            if (showWin && ourWin != null)
            {
                //Debug.Log("Modacts calling GUI!2");
                ourWin.OnGUI();
            }
        }

        public void OnDisable()
        {


            if (ourWin != null)
            {
                winTop = ourWin.MainWindowRect.y;
                winLeft = ourWin.MainWindowRect.x;
                ourWin.drawWin = false;
                //ourWin.Kill();
            }
            settings.RemoveValue("EdWinTop");
            settings.RemoveValue("EdWinLeft");
            settings.AddValue("EdWinTop", winTop);
            settings.AddValue("EdWinLeft", winLeft);
            settings.Save(KSPUtil.ApplicationRootPath + "GameData/ModActions/ModActions.settings");
            ourWin = null;

            if (toolbarControl != null)
            {
                toolbarControl.OnDestroy();
                Destroy(toolbarControl);
            }
        }

        public void WinChangeAction(EditorScreen scrn)
        {
            //Debug.Log("ModActs win change");
            if (EditorLogic.fetch.editorScreen == EditorScreen.Actions)
            {
                // Debug.Log("ModActs win change1");
                if (ourWin == null) //initialize our window if not already extant, this event triggers twice per panels change
                {
                    ourWin = new MainGUIWindow(EditorLogic.SortedShipList, winTop, winLeft, true);
                    ourWin.drawWin = showWin;
                    try //getselectedparts returns null somewhere above it in the hierchy, do it this way for simplicities sake
                    {
                        // Debug.Log("ModActs win change1a1");
                        ourWin.SetPart(EditorActionGroups.Instance.GetSelectedParts().First());
                        // Debug.Log("ModActs win change1a2");
                        lastSelectedPart = EditorActionGroups.Instance.GetSelectedParts().First();
                    }
                    catch
                    {
                        //  Debug.Log("ModActs win change1a");
                        ourWin.SetPart(null);
                        lastSelectedPart = null;
                    }
                }
            }
            else //moving away from actions panel, null our window
            {
                // Debug.Log("ModActs win chang2e");
                if (ourWin != null)
                {
                    ourWin.drawWin = false;
                    //ourWin.Kill();
                }
                ourWin = null;
            }
        }
        public void Update()
        {
            if (EditorLogic.fetch.editorScreen == EditorScreen.Actions)
            {
                try
                {
                    //Debug.Log("MA 1");
                    if (EditorActionGroups.Instance.GetSelectedParts().First() != lastSelectedPart) //check if selected part has changed
                    {
                        //Debug.Log("MA 2");
                        foreach (Part p in EditorActionGroups.Instance.GetSelectedParts())
                        {
                            //Debug.Log("MA " + p.ToString() + "|" + EditorActionGroups.Instance.GetSelectedParts().Count);
                        }
                        ourWin.SetPart(EditorActionGroups.Instance.GetSelectedParts().First());
                        //Debug.Log("MA 2a");
                        lastSelectedPart = EditorActionGroups.Instance.GetSelectedParts().First();
                        // Debug.Log("MA 2b");
                    }
                }
                catch (Exception e) //error trap if GetSelecetedParts above is null,
                {
                    //Debug.Log("MA Get sle parts null " + e);
                    //do nothing, if null nothing should happen
                }

                if (Time.time > lastUpdateTime + 5 && ourWin != null)
                {
                    ourWin.UpdateCheck();
                    lastUpdateTime = Time.time;
                }

            }
            if (ourWin != null)
            {
                ourWin.Update();
            }
            //foreach(Part p in EditorLogic.SortedShipList)
            //{
            //    Debug.Log("MA start");
            //    if(p.Modules.Contains<ModuleRCS>())
            //    {
            //        Debug.Log("MA " + p.Modules.OfType<ModuleModActions>().First().modActionsList.Count);
            //    }
            //}
            //foreach(Part p in EditorLogic.SortedShipList)
            //{
            //    foreach(PartModule pm in p.Modules)
            //    {
            //        Debug.Log("MA " + p.partName + "|" + pm.moduleName);
            //    }
            //}
        }
    }
}
