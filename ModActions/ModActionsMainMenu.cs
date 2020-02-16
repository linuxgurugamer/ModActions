using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace ModActions
{
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class ModActionsMainMenu : PartModule
    {
        public void Start()
        {
            Debug.Log("ModActions Ver. 1.6 Starting.....");
            if (!StaticMethods.ListPopulated) //populate our list if this is first load
            {
                StaticMethods.AllActionsList = new List<ModActionData>();
                string[] configFiles = Directory.GetFiles(new DirectoryInfo(KSPUtil.ApplicationRootPath).FullName + "GameData/ModActions"); //full path of all files in save dir
                foreach (string str in configFiles)
                {
                    if (str.EndsWith(".actions"))
                    {
                        ConfigNode loadingNode = ConfigNode.Load(str);
                        foreach (AssemblyLoader.LoadedAssembly Asm in AssemblyLoader.loadedAssemblies)
                        {
                            if (Asm.dllName == loadingNode.GetValue("assemblyname") || "Stock" == loadingNode.GetValue("assemblyname"))
                            {
                                string modName = loadingNode.GetValue("modname");
                                string pmName = loadingNode.GetValue("pmname");
                                foreach (ConfigNode actNode in loadingNode.nodes)
                                {
                                    string actgroup = actNode.GetValue("name");
                                    foreach (ConfigNode typeNode in actNode.nodes)
                                    {
                                        StaticMethods.AllActionsList.Add(new ModActionData() { Identifier = int.Parse(typeNode.GetValue("ident")), ModuleName = pmName, Description = "", Name = modName, ActionGroup = actgroup, ActionActual = typeNode.GetValue("name"), ActionValue = typeNode.GetValue("data"), ActionDataType = typeNode.GetValue("ActionData") });
                                    }
                                }
                                loadingNode.SetValue("assemblyname", "gibberishToPreventThisFileFromLoadingTwice");
                            }
                        }

                    }
                }
                StaticMethods.pmTypes = new Dictionary<string, Type>();
                foreach (AssemblyLoader.LoadedAssembly Asm in AssemblyLoader.loadedAssemblies)
                {
                    Type[] typeList = Asm.assembly.GetTypes();
                    foreach (Type t in typeList)
                    {
                        if (t.IsSubclassOf(typeof(PartModule)) && !StaticMethods.pmTypes.ContainsKey(t.Name))
                        {
                            StaticMethods.pmTypes.Add(t.Name, t);
                        }
                    }
                }
                StaticMethods.ListPopulated = true;
                //Debug.Log("ModActs Type Count is " + StaticMethods.pmTypes.Count);

                //foreach (ModActionData md in StaticMethods.AllActionsList) //for debugging, lists all actions
                //{
                //    Debug.Log("ModAction " + md.ToString());
                //}
            }
        }
    }

}
