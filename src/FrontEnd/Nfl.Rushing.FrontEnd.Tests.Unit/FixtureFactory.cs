using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutoFixture;
using AutoFixture.AutoFakeItEasy;

namespace Nfl.Rushing.FrontEnd.Tests.Unit
{
    public static class FixtureFactory
    {
        private static readonly object SyncRoot = new();
        private static readonly HashSet<Type> CustomizationTypes = new();

        public static void AddCustomizationsFromAssembly(Assembly assembly)
        {
            var customizationAttribute = assembly.GetCustomAttribute<AutoFakeItEasyCustomizationAttribute>();
            if (customizationAttribute == null)
            {
                return;
            }

            lock (SyncRoot)
            {
                foreach (var customizationType in customizationAttribute.CustomizationTypes)
                {
                    CustomizationTypes.Add(customizationType);
                }
            }
        }

        public static IFixture CreateFixture()
        {
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());

            lock (SyncRoot)
            {
                foreach (var customizationType in CustomizationTypes)
                {
                    fixture.Customize((ICustomization)Activator.CreateInstance(customizationType));
                }
            }

            return fixture;
        }
    }
}