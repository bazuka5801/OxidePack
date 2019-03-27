using System;
using System.Collections.Generic;
using System.Linq;

namespace OxidePack.Data
{
    public partial class UserData
    {
        public bool HasPermission(string name)
        {
            for (var i = this.permissions.Count - 1; i >= 0; i--)
            {
                var permission = this.permissions[i];
                if (PermissionIsExpired(permission))
                {
                    this.permissions.Remove(permission);
                    continue;
                }

                if (permission.name == name)
                {
                    return true;
                }
            }

            return false;
        }

        private bool PermissionIsExpired(Permission perm)
        {
            return DateTime.Now > ConvertFromUnixTimestamp(perm.expired);
        }
        public string GetPermDate(Permission perm)
        {
            return ConvertFromUnixTimestamp(perm.expired).ToString("g");
        }

        public void AddPermission(string name, int seconds)
        {
            if (this.permissions == null) this.permissions = new List<Permission>();
            var permission = this.permissions.FirstOrDefault(p => p.name == name);

            if (permission != null && PermissionIsExpired(permission))
            {
                this.permissions.Remove(permission);
                permission = null;
            }

            if (permission == null)
            {
                this.permissions.Add(permission = Pool.Get<Permission>());
                permission.name = name;
                permission.expired = (ulong)ConvertToUnixTimestamp(DateTime.Now);
            }

            permission.expired += (ulong)seconds;
        }

        public bool CanEncrypt()
        {
            if (encryptionCount > 0)
            {
                return true;
            }

            return HasPermission("vip");
        }

        private static DateTime ConvertFromUnixTimestamp(ulong timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }

        private static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}