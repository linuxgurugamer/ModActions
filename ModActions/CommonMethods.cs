using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP.UI.Screens;

using ToolbarControl_NS;

namespace ModActions
{

    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class EventTest : PartModule
    {
        public static List<EventTestListClass> testListContainer;
        public static Dictionary<Guid, Dictionary<int, bool>> testDict;

        public void Start()
        {
            testListContainer = new List<EventTestListClass>();
        }

        public static void CallEvent()
        {
            testDict = new Dictionary<Guid, Dictionary<int, bool>>();
            Debug.Log("MA Call event start");
            EventData<Action<Dictionary<Guid, Dictionary<int, bool>>>> MA2testEvent = GameEvents.FindEvent<EventData<Action<Dictionary<Guid, Dictionary<int, bool>>>>>("onTestEvent");
            List<float> tempList = new List<float>();
            MA2testEvent.Fire(linkOverEvent);
            //testListContainer.Add(new EventTestListClass(Time.realtimeSinceStartup, tempList));
            Debug.Log("MA Call event end " + testDict.Count);
        }

        public static void linkOverEvent(Dictionary<Guid, Dictionary<int, bool>> toLink)
        {
            testDict = toLink;
        }

        public static void PrintContainer()
        {
            Debug.Log("MA container print start " + Time.realtimeSinceStartup + testListContainer.Count);
            for(int i = 0;i< testDict.Count; i++)
            {
                Debug.Log("MA entry " + i + "|" + testDict.ElementAt(i).Key.ToString() + "|" + testDict.ElementAt(i).Value.ToString());
                //foreach (float fl in testListContainer[i].numList)
                //{
                //    Debug.Log("MA entry sub " + testListContainer[i].initTime + "|" + fl);
                //}
            }
            Debug.Log("MA Container print end");

        }
        

    }

    public class EventTestListClass //use for event testing
    {
        public List<float> numList;
        public float initTime;

        public EventTestListClass()
        {
            numList = new List<float>();
            initTime = Time.realtimeSinceStartup;
        }
        public EventTestListClass(float tmr, List<float> tempList)
        {
            numList = tempList;
            initTime = tmr;
        }
    }
 

    public static class StaticMethods //static data that should never change
    {
        public static bool ListPopulated = false;
        public static List<ModActionData> AllActionsList;
        public static Dictionary<string, Type> pmTypes;
    }

    //public class VslResTest
    //{
    //    Dictionary<Vessel, Dictionary<string, double>> allVesselResources;

    //    public void Update() //replace with your trigger
    //    {
    //        foreach (Vessel vsl in FlightGlobals.Vessels) //cycles all vessels in game, will probably nullref on vessels outside physics range
    //        {
    //            if(allVesselResources.ContainsKey(vsl)) //if vessel is present in our saved data remove it to avoid duplicate data
    //            {
    //                allVesselResources.Remove(vsl);
    //            }
    //            allVesselResources.Add(vsl,new Dictionary<string,double>()); //note the new, that zeros all values for the vessel
    //            Dictionary<string, double> vesselResources = allVesselResources[vsl]; //create a shortcut reference to this vessel's dictionary
    //            foreach(Part p in vsl.parts) //cycle through parts on the current vessel
    //            {
    //                foreach(PartResource pRes in p.Resources) //cycle through all resources on this part
    //                {
    //                    if(!vesselResources.ContainsKey(pRes.resourceName)) //check if resource exists already
    //                    {
    //                        vesselResources.Add(pRes.resourceName, 0f); //add resources with zero amount if it doesn't exist
    //                    }
    //                    vesselResources[pRes.resourceName] += pRes.amount;
    //                }
    //            }
    //        } //close foreach cycling through vessels
    //        Debug.Log("Electric charge on focus vessel is " + allVesselResources[FlightGlobals.ActiveVessel].["ElectricCharge"]);
    //    }
    //}


}
