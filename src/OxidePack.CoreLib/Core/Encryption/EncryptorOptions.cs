namespace OxidePack.CoreLib
{
    public class EncryptorOptions
    {
        public bool LocalVarsCompressing
        {
            get;
            set;
        }

        public bool FieldsCompressing
        {
            get;
            set;
        }

        public bool MethodsCompressing
        {
            get;
            set;
        }

        public bool TypesCompressing
        {
            get;
            set;
        }

        public bool SpacesRemoving
        {
            get;
            set;
        }

        public bool TrashRemoving
        {
            get;
            set;
        }

        public EncryptorOptions(bool maxCompression = true)
        {
            if (maxCompression)
            {
                LocalVarsCompressing = true;
                FieldsCompressing = true;
                TypesCompressing = true;
                MethodsCompressing = true;
                SpacesRemoving = true;
                TrashRemoving = true;
            }
        }
    }
}