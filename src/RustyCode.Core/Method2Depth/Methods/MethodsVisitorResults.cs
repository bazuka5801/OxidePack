using System.Collections.Generic;

namespace OxidePack.CoreLib.Method2Depth
{
    public class MethodsVisitorResults
    {
        /// <summary>
        ///     Method - Parent Class
        /// </summary>
        public Dictionary<string, MethodClassData> Methods;

        public MethodsVisitorResults()
        {
            Methods = new Dictionary<string, MethodClassData>();
        }

        public void Merge(MethodsVisitorResults methodsVisitorResults)
        {
            foreach (var method in methodsVisitorResults.Methods)
            {
                Methods[method.Key] = method.Value;
            }

            methodsVisitorResults.Methods.Clear();
        }
    }
}