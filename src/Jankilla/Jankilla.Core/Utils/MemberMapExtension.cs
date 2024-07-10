using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Utils
{
    public static class MemberMapExtension
    {
        public static MemberMap<TClass, TMember> ConstantFixed<TClass, TMember>(this MemberMap<TClass, TMember> mm, TMember constantValue)
        {
            mm.Constant(constantValue);

            // Hacky stuff here
            var setter = mm.Data.GetType().GetProperty(nameof(mm.Data.Member))?.GetSetMethod(true);
            setter?.Invoke(mm.Data, new object[] { new CustomPropInfo(constantValue) });

            return mm;
        }

        private class CustomPropInfo : PropertyInfo
        {
            private readonly object constantValue;

            public CustomPropInfo(object constantValue) => this.constantValue = constantValue;

            public object Getter() => this.constantValue;

            public override object[] GetCustomAttributes(bool inherit) => Array.Empty<object>();
            public override object[] GetCustomAttributes(Type attributeType, bool inherit) => Array.Empty<object>();
            public override bool IsDefined(Type attributeType, bool inherit) => false;
            public override Type DeclaringType => null;
            public override string Name => "dynamic";
            public override Type ReflectedType => null;
            public override MethodInfo[] GetAccessors(bool nonPublic) => Array.Empty<MethodInfo>();
            public override MethodInfo GetGetMethod(bool nonPublic) => this.GetType().GetMethod(nameof(this.Getter));

            public override ParameterInfo[] GetIndexParameters() => Array.Empty<ParameterInfo>();
            public override MethodInfo GetSetMethod(bool nonPublic) => null;
            public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture) => null;
            public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object [] index, CultureInfo culture) { }
            public override PropertyAttributes Attributes => PropertyAttributes.None;
            public override bool CanRead => true;
            public override bool CanWrite => false;
            public override Type PropertyType => typeof(string);
        }
    }
}
