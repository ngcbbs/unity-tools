using System;

namespace Tools.Runtime.DependencyInjection {
    private const AttributeTargets kInjectTarget = AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property;
    private const AttributeTargets kProvideTarget = AttributeTargets.Method;

    [AttributeUsage(kInjectTarget)]
    public sealed class InjectAttribute : Attribute { }

    [AttributeUsage(kProvideTarget)]
    public sealed class ProvideAttribute : Attribute { }
}