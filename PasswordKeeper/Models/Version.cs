using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordKeeper
{
    public class Version
    {
        private int _MajorVersionNumber = 0;
        private int _MinorVersionNumber = 0;
        private int _RevisionNumber = 0;
        private int _BuildNumber = 0;
        public int MajorVersionNumber
        {
            get
            {
                return _MajorVersionNumber;
            }
            set
            {
                _MajorVersionNumber = value;
            }
        }
        public int MinorVersionNumber
        {
            get
            {
                return _MinorVersionNumber;
            }
            set
            {
                _MinorVersionNumber = value;
            }
        }
        public int RevisionNumber
        {
            get
            {
                return _RevisionNumber;
            }
            set
            {
                _RevisionNumber = value;
            }
        }
        public int BuildNumber
        {
            get
            {
                return _BuildNumber;
            }
            set
            {
                _BuildNumber = value;
            }
        }
        public Version(string strVersion)
        {
            string[] listVersion = strVersion.Split('.');
            int.TryParse(listVersion[0], out _MajorVersionNumber);
            int.TryParse(listVersion[1], out _MinorVersionNumber);
            int.TryParse(listVersion[2], out _RevisionNumber);
            int.TryParse(listVersion[3], out _BuildNumber);
        }

        public override string ToString()
        {
            return this._MajorVersionNumber.ToString() + "." + this._MinorVersionNumber.ToString() + "." + this._RevisionNumber.ToString() + "." + this._BuildNumber.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Version)
            {
                var version = obj as Version;
                if (this._MajorVersionNumber == version._MajorVersionNumber && this._MinorVersionNumber == version._MinorVersionNumber && this._RevisionNumber == version._RevisionNumber && this._BuildNumber == version._BuildNumber)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

        }

        public override int GetHashCode()
        {
            return this._MajorVersionNumber.GetHashCode() ^ this._MinorVersionNumber.GetHashCode() ^ this._RevisionNumber.GetHashCode() ^ this._BuildNumber.GetHashCode();
        }

        public static bool operator > (Version version1,Version version2)
        {
            if (version1._MajorVersionNumber > version2._MajorVersionNumber && version1._MinorVersionNumber > version2._MinorVersionNumber && version1._RevisionNumber > version2._RevisionNumber && version1._BuildNumber > version2._BuildNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator < (Version version1, Version version2)
        {
            if (version1._MajorVersionNumber < version2._MajorVersionNumber && version1._MinorVersionNumber < version2._MinorVersionNumber && version1._RevisionNumber < version2._RevisionNumber && version1._BuildNumber < version2._BuildNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator == (Version version1, Version version2)
        {
            if (version1._MajorVersionNumber == version2._MajorVersionNumber && version1._MinorVersionNumber == version2._MinorVersionNumber && version1._RevisionNumber == version2._RevisionNumber && version1._BuildNumber == version2._BuildNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(Version version1, Version version2)
        {
            if (version1._MajorVersionNumber == version2._MajorVersionNumber && version1._MinorVersionNumber == version2._MinorVersionNumber && version1._RevisionNumber == version2._RevisionNumber && version1._BuildNumber == version2._BuildNumber)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
