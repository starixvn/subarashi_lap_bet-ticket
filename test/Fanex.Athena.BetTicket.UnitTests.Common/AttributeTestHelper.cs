using System;

namespace Fanex.Athena.BetTicket.UnitTests.Common
{
    public static class AttributeTestHelper
    {
        public static bool IsDecoratedWithAttribute(Type controllerType, Type attributeType, string methodName)
        {
            var methodInfo = controllerType.GetMethod(methodName);
            if (methodInfo is null)
            {
                throw new ArgumentException("Invalid method");
            }

            var methodAttributes = methodInfo.GetCustomAttributes(false);

            return Array.Exists(methodAttributes, x => x.GetType() == attributeType);
        }
    }
}
