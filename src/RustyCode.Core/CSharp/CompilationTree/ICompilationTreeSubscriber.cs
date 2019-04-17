namespace OxidePack.CoreLib
{
    internal interface ICompilationTreeSubscriber
    {
        void OnCompilationTreeUpdate(CompilationTree compilationTree);
    }
}