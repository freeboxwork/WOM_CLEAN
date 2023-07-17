// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("zCMFd5dRht5Lvx+kFUVCS6/t1wGLfStJq3cPu322FHqrAAxgyxymJhZ8K+Cd+uDUehK3doDolZ0JQlZw9qiDHJ1+L9qg4wbY9hyghcCYQ/we+5mKkamfLkRRpaHUGf3WOQFoZXsocZi1tp7kuIhctp7SZ9QiJtHvHBgyrKty/fpVS5h3on1mRxttgwqqGJu4qpeck7Ac0hxtl5ubm5+amRiblZqqGJuQmBibm5oJF9wZi3cdvZE+/1LRK55Ak9qOMgpMJe5VwMqgoHqrnuN4rItRuERPSdhjctFPWJBZxUDAt+KJQgDktgbnQdBawRaF9qqywcihrKJOb2X90FEusX/6DFQFJMXpHDz4RLRE6lq6uwFAZl83BM9KVw6fHnWyU5iZm5qb");
        private static int[] order = new int[] { 6,5,3,10,4,9,10,10,9,11,11,12,13,13,14 };
        private static int key = 154;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
