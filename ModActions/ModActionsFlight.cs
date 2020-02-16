using System;
using UnityEngine;
using KSP.UI.Screens;

using ToolbarControl_NS;

namespace ModActions
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class ModActionsFlight : PartModule
    {
        private bool buttonCreated = false;
        MainGUIWindow ourWin;
        ConfigNode settings;
        float winTop;
        float winLeft;
        Part lastSelectedPart;
        bool ShowModActs;
        float lastUpdateTime;
        public bool showKSPui = true;
        bool showBtn = true;

        public void Start()
        {

            settings = ConfigNode.Load(KSPUtil.ApplicationRootPath + "GameData/ModActions/ModActions.settings");

            winTop = float.Parse(settings.GetValue("FltWinTop"));
            winLeft = float.Parse(settings.GetValue("FltWinLeft"));
            if (int.Parse(settings.GetValue("FlightBtnVis")) == 0) //0 hide win, 1 show win, 2 (or any other value) only show if agx installed
            {
                showBtn = false;
            }
            else if (int.Parse(settings.GetValue("FlightBtnVis")) == 1)
            {
                showBtn = true;
            }
            else
            {
                showBtn = false;
                foreach (AssemblyLoader.LoadedAssembly Asm in AssemblyLoader.loadedAssemblies)
                {
                    if (Asm.dllName == "AGExt")
                    {
                        //Debug.Log("RemoteTech found");
                        //AGXRemoteTechQueue.Add(new AGXRemoteTechQueueItem(group, FlightGlobals.ActiveVessel.rootPart.flightID, Planetarium.GetUniversalTime() + 10, force, forceDir));
                        showBtn = true;
                    }
                }
            }
            if (showBtn)
            {
                AddButtons();
            }
            GameEvents.onHideUI.Add(onHideMyUI);
            GameEvents.onShowUI.Add(onShowMyUI);
        }

        void onHideMyUI()
        {
            if (ourWin != null)
            {
                ourWin.showKSPui = false;
            }
            showKSPui = false;
        }
        void onShowMyUI()
        {
            if (ourWin != null)
            {
                ourWin.showKSPui = true;
            }
            showKSPui = true;
        }

        internal ToolbarControl toolbarControl;

        void AddButtons()
        {
            if (!buttonCreated)
            {
                buttonCreated = true;

                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(onStockToolbarClick, onStockToolbarClick,
                   ApplicationLauncher.AppScenes.FLIGHT,
                    ModActionsEditor.MODID,
                    "maButton",
                    "ModActions/PluginData/BtnStock",
                    "ModActions/PluginData/Btn",
                    ModActionsEditor.MODNAME
                );
            }
        }

        public void OnGUI()
        {
            if (ShowModActs && ourWin != null)
            {
                ourWin.OnGUI();
            }
        }

        public void onStockToolbarClick()
        {
            string errLine = "1";
            try
            {
                errLine = "2";
                ShowModActs = !ShowModActs;
                errLine = "3";
                if (ShowModActs)
                {
                    errLine = "4";
                    if (ourWin == null)
                    {
                        //Debug.Log("make win");
                        errLine = "5";
                        ourWin = new MainGUIWindow(FlightGlobals.ActiveVessel.Parts, winTop, winLeft, showKSPui);
                    }
                    errLine = "6";
                    ourWin.SetPart(null);
                    errLine = "7";
                    lastSelectedPart = null;
                    errLine = "8";
                    ourWin.drawWin = ShowModActs;
                }
                else
                {
                    errLine = "9";
                    if (ourWin != null)
                    {
                        errLine = "10";
                        ourWin.drawWin = false;
                        //ourWin.Kill();
                        ourWin = null;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("ModActs StockBtnClick " + errLine + " " + e);
            }
        }

        public void DummyVoid()
        {

        }

        public void OnDisable()
        {
            if (ourWin != null)
            {
                // Debug.Log("ModActions Flight Dis b");
                winTop = ourWin.MainWindowRect.y;
                winLeft = ourWin.MainWindowRect.x;
                ourWin.drawWin = false;
                //ourWin.Kill();
            }
            settings.RemoveValue("FltWinTop");
            settings.RemoveValue("FltWinLeft");
            settings.AddValue("FltWinTop", winTop);
            settings.AddValue("FltWinLeft", winLeft);
            //Debug.Log("ModActions Flight Dis A");
            settings.Save(KSPUtil.ApplicationRootPath + "GameData/ModActions/ModActions.settings");
            //  Debug.Log("ModActions Flight Dis c");
            ourWin = null;

            if (toolbarControl != null)
            {
                toolbarControl.OnDestroy();
                Destroy(toolbarControl);
            }

            // Debug.Log("ModActions Flight Dis f");
        }


        public void Update()
        {

            if (ourWin != null)
            {
                if (ourWin.selectedPart != lastSelectedPart) //check if selected part has changed
                {
                    ourWin.SetPart(lastSelectedPart);
                }

                //if (ourWin != null)
                //{
                //    ourWin.Update();
                //}

                if (Time.time > lastUpdateTime + 5 && ourWin != null)
                {
                    ourWin.UpdateCheck();
                    lastUpdateTime = Time.time;
                }

                if (Input.GetKeyDown(KeyCode.Mouse0) && ShowModActs)
                {

                    Part selPart = new Part();
                    selPart = SelectPartUnderMouse();
                    if (selPart != null)
                    {
                        lastSelectedPart = selPart;
                    }
                }
            }
            //foreach(Part p in FlightGlobals.ActiveVessel.Parts)
            //{
            //    Debug.Log("modacts " + p.name + " " + p.HighlightActive + "|" + Mouse.HoveredPart); 
            //}
        }

        public Part SelectPartUnderMouse()
        {
            //FlightCamera CamTest = new FlightCamera();
            //CamTest = FlightCamera.fetch;
            //Ray ray = CamTest.mainCamera.ScreenPointToRay(Input.mousePosition);
            //LayerMask RayMask = new LayerMask();
            //RayMask = 1 << 0;
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit, Mathf.Infinity, RayMask))
            //{
            //    Part hitPart = (Part)UIPartActionController.GetComponentUpwards("Part", hit.collider.gameObject); //how to find small parts that are "inside" the large part they are attached to.
            //    if (FlightGlobals.ActiveVessel.parts.Contains(hitPart))
            //    {
            //        return hitPart;
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //    //return FlightGlobals.ActiveVessel.Parts.Find(p => p.gameObject == hit.transform.gameObject);
            //}
            //return null;

            return Mouse.HoveredPart;
        }
    }
}
