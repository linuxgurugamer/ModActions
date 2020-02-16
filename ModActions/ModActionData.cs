using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModActions
{
    public class ModActionData
    {

        public int Identifier; //unique identifier, used for SWITCH statement
        public string ModuleName; //partModule name for filter, show this action if selected part has this module
        public string Description; //action group description, used for BaseAction.guiName, first column (is editable), resets when ActionValue type changes to default action value
        public string Name; //Mod name, second column (is selectable)
        public string ActionGroup; //Action group, third column (is selectable), break up large mods such as mechjeb into regions
        public string ActionActual; //our actual action, fourth column
        public string ActionValue; //default value, if non-editable display as lable, otherwise as text box, fifth column
        public string ActionDataType; //type of last column?

        public override string ToString() //give this class a useful ToString() function
        {
            return Identifier.ToString() + " " + ModuleName + " " + Description + " " + Name + " " + ActionGroup + " " + ActionActual + " " + ActionValue + " " + ActionDataType.ToString();
        }

        public ModActionData() //blank constructor
        {

        }

        public ModActionData(ModActionData orig) //copy constructor
        {
            //ModActionData copy = new ModActionData();
            Identifier = orig.Identifier;
            ModuleName = string.Copy(orig.ModuleName);
            Description = string.Copy(orig.Description);
            Name = string.Copy(orig.Name);
            ActionGroup = string.Copy(orig.ActionGroup);
            ActionActual = string.Copy(orig.ActionActual);
            ActionValue = string.Copy(orig.ActionValue);
            ActionDataType = string.Copy(orig.ActionDataType);

        }

    }

}
