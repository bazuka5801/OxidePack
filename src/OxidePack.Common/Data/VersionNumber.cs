namespace OxidePack.Data
{
    public struct VersionNumber
    {
        public int Major, Minor, Build;

        public VersionNumber(int major, int minor, int build)
        {
            this.Major = major;
            this.Minor = minor;
            this.Build = build;
        }
        
        #region Operator Overloads

        /// <summary>
        /// Compares this version for equality to another
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(VersionNumber a, VersionNumber b)
        {
            return a.Major == b.Major && a.Minor == b.Minor && a.Build == b.Build;
        }

        /// <summary>
        /// Compares this version for inequality to another
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(VersionNumber a, VersionNumber b)
        {
            return a.Major != b.Major || a.Minor != b.Minor || a.Build != b.Build;
        }

        public static bool operator >(VersionNumber a, VersionNumber b)
        {
            if (a.Major < b.Major)
            {
                return false;
            }

            if (a.Major > b.Major)
            {
                return true;
            }

            if (a.Minor < b.Minor)
            {
                return false;
            }

            if (a.Minor > b.Minor)
            {
                return true;
            }

            return a.Build > b.Build;
        }

        public static bool operator >=(VersionNumber a, VersionNumber b)
        {
            if (a.Major < b.Major)
            {
                return false;
            }

            if (a.Major > b.Major)
            {
                return true;
            }

            if (a.Minor < b.Minor)
            {
                return false;
            }

            if (a.Minor > b.Minor)
            {
                return true;
            }

            return a.Build >= b.Build;
        }

        public static bool operator <(VersionNumber a, VersionNumber b)
        {
            if (a.Major > b.Major)
            {
                return false;
            }

            if (a.Major < b.Major)
            {
                return true;
            }

            if (a.Minor > b.Minor)
            {
                return false;
            }

            if (a.Minor < b.Minor)
            {
                return true;
            }

            return a.Build < b.Build;
        }

        public static bool operator <=(VersionNumber a, VersionNumber b)
        {
            if (a.Major > b.Major)
            {
                return false;
            }

            if (a.Major < b.Major)
            {
                return true;
            }

            if (a.Minor > b.Minor)
            {
                return false;
            }

            if (a.Minor < b.Minor)
            {
                return true;
            }

            return a.Build <= b.Build;
        }

        #endregion Operator Overloads
        
        /// <summary>
        /// Compares this version for equality to the specified object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is VersionNumber other)
            {
                return this == other;
            }

            return false;
        }

        /// <summary>
        /// Gets a hash code for this version
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Major.GetHashCode();
                hash = hash * 23 + Minor.GetHashCode();
                hash = hash * 23 + Build.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Build}";
        }
    }
}