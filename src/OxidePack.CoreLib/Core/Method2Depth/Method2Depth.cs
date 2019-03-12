namespace OxidePack.CoreLib.Method2Depth
{
    public class Method2Depth
    {
        public string ProcessSource(string source)
        {
            var tree = new CompilationTree(source, "references/");

            new Core(tree).Process();

            return tree.SourceText;
        }
    }
}