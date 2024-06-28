using System.Runtime.CompilerServices;

namespace Foundation.WorkBox.Applications.Workflows
{
    // Token: 0x020001EF RID: 495
    [CompilerGenerated]
    internal sealed class PrivateImplementationDetails
    {
        // Token: 0x06001216 RID: 4630 RVA: 0x00076BAC File Offset: 0x00074DAC
        internal static uint ComputeStringHash(string s)
        {
            uint num = 1;
            if (s != null)
            {
                num = 2166136261U;
                for (int i = 0; i < s.Length; i++)
                {
                    num = ((uint)s[i] ^ num) * 16777619U;
                }
            }
            return num;
        }
    }
}