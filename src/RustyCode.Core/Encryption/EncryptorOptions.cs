namespace OxidePack.CoreLib
{
    public class EncryptorOptions
    {
        public bool LocalVarsCompressing { get; set; }

        public bool FieldsCompressing { get; set; }

        public bool MethodsCompressing { get; set; }

        public bool TypesCompressing { get; set; }

        public bool SpacesRemoving { get; set; }

        public bool TrashRemoving { get; set; }

        public bool Encoding { get; set; }

        public bool Spaghetti { get; set; }

        public int SpaghettiDepth { get; set; }

        public bool SpaghettiControlFlow { get; set; }
    }
}