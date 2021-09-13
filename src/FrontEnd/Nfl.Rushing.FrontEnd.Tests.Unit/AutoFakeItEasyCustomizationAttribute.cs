using System;
using System.Collections.Generic;
using System.Linq;

namespace Nfl.Rushing.FrontEnd.Tests.Unit
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AutoFakeItEasyCustomizationAttribute : Attribute
    {
        public AutoFakeItEasyCustomizationAttribute(params Type[] customizationTypes)
        {
            this.CustomizationTypes = customizationTypes;
        }

        public Type[] CustomizationTypes { get; }
    }
}