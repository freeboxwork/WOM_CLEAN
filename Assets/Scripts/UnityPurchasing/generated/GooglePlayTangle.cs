// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("stiPRDleRHDethPSJEwxOa3m8tShgGFNuJhc4BDgTv4eH6XkwvuToFIMJ7g52ot+BEeifFK4BCFkPOdYvD8xPg68PzQ8vD8/Pq2zeL0v07kOvD8cDjM4NxS4drjJMz8/Pzs+PQQE3g86R9wIL/Uc4OvtfMfWdev8aIeh0zP1InrvG7sAseHm7wtJc6UZNZpb9nWPOuQ3fiqWruiBSvFkbt+M1TwREjpAHCz4Ejp2w3CGgnVLNP1h5GQTRi3mpEASokPldP5lsiFSDhZlbAUIBurLwVl09YoV216o8C/Zj+0P06sf2RKw3g+kqMRvuAKCul89LjUNO4rg9QEFcL1Zcp2lzMG4vJYID9ZZXvHvPNMG2cLjv8knrmvu86o7utEW9zw9Pz4/");
        private static int[] order = new int[] { 3,7,10,7,7,8,6,11,9,12,13,12,12,13,14 };
        private static int key = 62;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
