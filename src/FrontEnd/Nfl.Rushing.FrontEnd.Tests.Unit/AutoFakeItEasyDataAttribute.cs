using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutoFixture.Xunit2;

namespace Nfl.Rushing.FrontEnd.Tests.Unit
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AutoFakeItEasyDataAttribute : AutoDataAttribute
    {
        public AutoFakeItEasyDataAttribute()
            : base(FixtureFactory.CreateFixture)
        {
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            FixtureFactory.AddCustomizationsFromAssembly(testMethod.ReflectedType.Assembly);
            return base.GetData(testMethod);
        }
    }
}