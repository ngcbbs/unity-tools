using System;

namespace UnityTools.DependencyInjection {
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class InjectAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProvideAttribute : Attribute { }
}