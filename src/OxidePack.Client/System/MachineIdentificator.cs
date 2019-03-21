using System.Collections.Generic;
using System.Management;

namespace OxidePack.Client
{
    public class MachineIdentificator
    {
        private static string key = null;
        public static string Value()
        {
            if (key == null)
            {
                key = (
                    "CPU >> "  + cpuId()  + "\n" +
                    "BIOS >> " + biosId() + "\n" +
                    "BASE >> " + baseId() + "\n" +
                    "MAC >> "  + macId()  + "\n" +
                    "DISK >> " + diskId() + "\n" // +
//                    "VIDEO >> " + videoId()
                );
                key = key.ToSHA512();
            }
            return key;
        }

        #region Original Device ID Getting Code

        private static readonly Dictionary<string, ManagementObjectCollection> mCollections = new Dictionary<string, ManagementObjectCollection>();

        private static ManagementObjectCollection GetMOCollection(string wmiClass)
        {
            if (mCollections.TryGetValue(wmiClass, out var moc) == false)
            {
                mCollections[wmiClass] = moc = new ManagementClass(wmiClass).GetInstances();
            }

            return moc;
        }

        //Return a hardware identifier
        private static string identifier
            (string wmiClass, string wmiProperty, string wmiMustBeTrue)
        {
            string result = "";
            ManagementObjectCollection moc = GetMOCollection(wmiClass);
            foreach (ManagementObject mo in moc)
            {
                if (mo[wmiMustBeTrue].ToString() == "True")
                {
                    //Only get the first one
                    if (result == "")
                    {
                        try
                        {
                            result = mo[wmiProperty].ToString();
                            break;
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return result;
        }

        //Return a hardware identifier
        private static string identifier(string wmiClass, string wmiProperty)
        {
            string result = "";
            ManagementObjectCollection moc = GetMOCollection(wmiClass);
            foreach (ManagementObject mo in moc)
            {
                //Only get the first one
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
            }

            return result;
        }

        private static string cpuId()
        {
            //Uses first CPU identifier available in order of preference
            //Don't get all identifiers, as it is very time consuming
            string retVal = "";
            retVal = identifier("Win32_Processor", "ProcessorId");
            if (retVal == "") //If no ProcessorId, use Name
            {
                retVal = identifier("Win32_Processor", "Name");
                if (retVal == "") //If no Name, use Manufacturer
                {
                    retVal = identifier("Win32_Processor", "Manufacturer");
                }

                //Add clock speed for extra security
                retVal += identifier("Win32_Processor", "MaxClockSpeed");
            }

            return retVal;
        }
        //BIOS Identifier
        private static string biosId()
        {
            return identifier("Win32_BIOS", "Manufacturer")
            + identifier("Win32_BIOS", "SMBIOSBIOSVersion")
            + identifier("Win32_BIOS", "IdentificationCode")
            + identifier("Win32_BIOS", "SerialNumber")
            + identifier("Win32_BIOS", "ReleaseDate")
            + identifier("Win32_BIOS", "Version")
                ;
        }
        //Main physical hard drive ID
        private static string diskId()
        {
            return identifier("Win32_DiskDrive", "Model")
            + identifier("Win32_DiskDrive", "Manufacturer")
            + identifier("Win32_DiskDrive", "Signature")
            + identifier("Win32_DiskDrive", "TotalHeads");
        }
        //Motherboard ID
        private static string baseId()
        {
            return identifier("Win32_BaseBoard", "Model")
            + identifier("Win32_BaseBoard", "Manufacturer")
            + identifier("Win32_BaseBoard", "Name")
            + identifier("Win32_BaseBoard", "SerialNumber");
        }
        //Primary video controller ID
        private static string videoId()
        {
            return identifier("Win32_VideoController", "DriverVersion")
            + identifier("Win32_VideoController", "Name");
        }
        //First enabled network card ID
        private static string macId()
        {
            return identifier("Win32_NetworkAdapterConfiguration",
                "MACAddress", "IPEnabled");
        }
        #endregion
    }
}