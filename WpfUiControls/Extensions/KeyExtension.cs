using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace WpfUiControls
{
    public static class KeyExtension
    {
        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint virtualKeyCode, uint scanCode,
            byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)]
            StringBuilder receivingBuffer,
            int bufferSize, uint flags);

        public static bool IsDigit(this Key key)
        {
            if (char.IsDigit((char)key))
                return true;

            if ((int)key >= (int)Key.NumPad0
                && (int)key <= (int)Key.NumPad9)
                return true;

            return false;
        }

        public static string GetUnicodeFromVirtualKey(this Key key)
        {
            var virtKey = KeyInterop.VirtualKeyFromKey(key);
            var buf = new StringBuilder(256);
            ToUnicode((uint)virtKey, 0, new byte[256], buf, 256, 0);
            return buf.ToString();
        }
    }
}
