using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutoFixture.Xunit2;

using Xunit;
using Xunit.Sdk;

namespace Nfl.Rushing.FrontEnd.Tests.Unit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MemberAutoFakeItEasyDataAttribute : CompositeDataAttribute
    {
        public MemberAutoFakeItEasyDataAttribute(string member)
            : base(new MemberDataAttribute(member), new InfiniteDataAttribute(new AutoFakeItEasyDataAttribute()))
        {
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var result = base.GetData(testMethod);
            return result;
        }

        // This attribute provide all the data to match the member data test case. It will not loop for ever. 
        private class InfiniteDataAttribute : DataAttribute
        {
            private readonly DataAttribute _decorated;

            public InfiniteDataAttribute(DataAttribute decorated)
            {
                this._decorated = decorated;
            }

            public override IEnumerable<object[]> GetData(MethodInfo testMethod)
            {
                while (true)
                {
                    foreach (var dataSet in this._decorated.GetData(testMethod))
                    {
                        yield return dataSet;
                    }
                }
            }
        }
    }
}