// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("4+fNU1SNAgWqtGeIXYKZuOSSfPWE145nSklhG0d3o0lhLZgr3dkuEG+mOr8/SB12vf8bSfkYvi+lPul6CVd842KB0CVfHPknCeNfej9nvANfX4VUYRyHU3SuR7uwtiecjS6wp+dkamVV52RvZ+dkZGX26CPmdIji+ts6FuPDB7tLuxWlRUT+v5mgyPsz3PqIaK55IbRA4Fvqur20UBIo/glVTT43XlNdsZCaAi+u0U6ABfOrdILUtlSI8ESCSeuFVP/znzTjWdnhBGZ1blZg0buuWl4r5gIpxv6XmkJuwQCtLtRhv2wlcc31s9oRqj81VedkR1VoY2xP4y3jkmhkZGRgZWbpg9QfYgUfK4XtSIl/F2pi9r2pjzC1qPFg4YpNrGdmZGVk");
        private static int[] order = new int[] { 2,9,12,10,8,9,7,7,13,9,13,11,12,13,14 };
        private static int key = 101;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
