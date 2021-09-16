using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutoFixture.Xunit2;

namespace Nfl.Rushing.FrontEnd.Tests.Unit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class InlineAutoFakeItEasyDataAttribute : InlineAutoDataAttribute
    {
        private readonly object[] _values;

        public InlineAutoFakeItEasyDataAttribute(params object[] values)
            : base(new AutoFakeItEasyDataAttribute(), values)
        {
            this._values = values;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var data = base.GetData(testMethod).ToList();

            for (var i = 0; i < this._values.Length; i++)
            {
                data[0][i] = this._values[i];
            }

            return data;
        }
    }
}