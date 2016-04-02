using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace KeyLogger
{
    public class Win32
    {
        /// <summary>
        /// SendInput Win32APIをラップしたメソッド
        /// </summary>
        /// <param name="inputs"></param>
        public static void SendInput(INPUT[] inputs)
        {
            for (Int32 i = 0; i < inputs.Length; i++)
            {
                INPUT input = inputs[i];
                if (input.type == type.INPUT_KEYBOARD)
                {
                    //キーボードの時は変換が必要らしい
                    //input.ki.wScan = (Int16)input.ki.wVk;//(Int16)(MapVirtualKey((Int32)input.ki.wVk, 0));
                    input.ki.dwExtraInfo = GetMessageExtraInfo();
                    input.ki.dwFlags = ExtendedKeyFlagW(input.ki.wVk) | input.ki.dwFlags;
                }
            }
            SendInput(inputs.Length, inputs, Marshal.SizeOf(inputs[0]));
        }

        public static void SendInput(INPUT input)
        {
            input.ki.dwExtraInfo = GetMessageExtraInfo();
            input.ki.dwFlags = ExtendedKeyFlagW(input.ki.wVk) | input.ki.dwFlags;
            SendInput(1, ref input, Marshal.SizeOf(input));
        }

        private static dwFlags ExtendedKeyFlagW(wVk key)
        {
            dwFlags flag = 0;
            if (key == wVk.VK_CONTROL)
            {//とりあえずControlだけ
                flag = dwFlags.KEYEVENTF_EXTENDEDKEY;
            }
            return flag;
        }
        //SendInput Function
        //http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/keyboardinput/keyboardinputreference/keyboardinputfunctions/sendinput.asp
        [DllImport("user32.dll")]
        private extern static Int32 SendInput(Int32 nInputs, INPUT[] pInputs, Int32 cbSize);

        [DllImport("user32.dll")]
        public static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        //GetMessageExtraInfo Function
        //http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/messagesandmessagequeues/messagesandmessagequeuesreference/messagesandmessagequeuesfunctions/getmessageextrainfo.asp
        [DllImport("user32.dll")]
        private extern static IntPtr GetMessageExtraInfo();

        //MapVirtualKey Function
        //http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/keyboardinput/keyboardinputreference/keyboardinputfunctions/mapvirtualkey.asp
        [DllImport("user32.dll")]
        private extern static Int32 MapVirtualKey(Int32 uCode, Int32 uMapType);

        [DllImport("user32.dll")]
        public extern static IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public extern static int SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public extern static int GetWindowThreadProcessId(IntPtr hWnd, ref int lpdwProcessId);

        [DllImport("user32.dll")]
        public extern static bool AttachThreadInput(int idAttach, int idAttachTo, bool fAttach);
    }

    //for KEYBDINPUT /MOUSEINPUT Structure
    public enum dwFlags
    {
        KEYEVENTF_KEYDOWN = 0,
        KEYEVENTF_EXTENDEDKEY = 0x1,
        KEYEVENTF_KEYUP = 0x2,
        KEYEVENTF_SCANCODE = 0x8,
        KEYEVENTF_UNICODE = 0x4,

        MOUSEEVENTF_MOVED = 0x0001,
        MOUSEEVENTF_LEFTDOWN = 0x0002,  // 左ボタン Down
        MOUSEEVENTF_LEFTUP = 0x0004,  // 左ボタン Up
        MOUSEEVENTF_RIGHTDOWN = 0x0008,  // 右ボタン Down
        MOUSEEVENTF_RIGHTUP = 0x0010,  // 右ボタン Up
        MOUSEEVENTF_MIDDLEDOWN = 0x0020,  // 中ボタン Down
        MOUSEEVENTF_MIDDLEUP = 0x0040,  // 中ボタン Up
        MOUSEEVENTF_WHEEL = 0x0080,
        MOUSEEVENTF_XDOWN = 0x0100,
        MOUSEEVENTF_XUP = 0x0200,
        MOUSEEVENTF_ABSOLUTE = 0x8000

    }

    //for INPUT Structure
    public enum type
    {
        INPUT_MOUSE = 0,
        INPUT_KEYBOARD = 1,
        INPUT_HARDWARE = 2
    }

    //INPUT struct
    //http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/keyboardinput/keyboardinputreference/keyboardinputstructs/input.asp
    [StructLayout(LayoutKind.Explicit)]
    public struct INPUT
    {
        [FieldOffset(0)]
        public type type;
        [FieldOffset(4)]
        public MOUSEINPUT mi;
        [FieldOffset(4)]
        public KEYBDINPUT ki;
        [FieldOffset(4)]
        public HARDWAREINPUT hi;
    }

    //MOUSEINPUT struct
    //http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/keyboardinput/keyboardinputreference/keyboardinputstructs/mouseinput.asp
    [StructLayout(LayoutKind.Explicit)]
    public struct MOUSEINPUT
    {
        [FieldOffset(0)]
        public Int32 dx;
        [FieldOffset(4)]
        public Int32 dy;
        [FieldOffset(8)]
        public Int32 mouseData;
        [FieldOffset(12)]
        public dwFlags dwFlags;
        [FieldOffset(16)]
        public Int32 time;
        [FieldOffset(20)]
        public IntPtr dwExtraInfo;
    }

    //KEYBDINPUT struct
    //http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/keyboardinput/keyboardinputreference/keyboardinputstructs/keybdinput.asp
    [StructLayout(LayoutKind.Explicit)]
    public struct KEYBDINPUT
    {
        [FieldOffset(0)]
        public wVk wVk;
        [FieldOffset(2)]
        public Int16 wScan;
        [FieldOffset(4)]
        public dwFlags dwFlags;
        [FieldOffset(8)]
        public Int32 time;
        [FieldOffset(12)]
        public IntPtr dwExtraInfo;
    }

    //HARDWAREINPUT struct
    //http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/keyboardinput/keyboardinputreference/keyboardinputstructs/hardwareinput.asp
    [StructLayout(LayoutKind.Explicit)]
    public struct HARDWAREINPUT
    {
        [FieldOffset(0)]
        public Int32 uMsg;
        [FieldOffset(4)]
        public Int16 wParamL;
        [FieldOffset(6)]
        public Int16 wParamH;
    }

    public enum wVk : short
    {
        VK_CANCEL = 0x3, //BREAK(Control+Pause) key(ExtendedKey)
        VK_BACK = 0x8, //BACKSPACE key
        VK_TAB = 0x9, //TAB key
        VK_CLEAR = 0xC,
        VK_RETURN = 0xD, //ENTER key
        VK_SHIFT = 0x10, //SHIFT key
        VK_CONTROL = 0x11, //CTRL key
        VK_MENU = 0x12, //ALT key
        VK_PAUSE = 0x13, //PAUSE key
        VK_CAPITAL = 0x14, //CAPS LOCK key
        VK_KANA = 0x15, //IME かな mode
        VK_JUNJA = 0x17,
        VK_FINAL = 0x18,
        VK_KANJI = 0x19, //IME 漢字 mode
        VK_ESCAPE = 0x1B, //ESC key
        VK_CONVERT = 0x1C, //IME 変換 key
        VK_NONCONVERT = 0x1D, //IME 無変換 key
        VK_ACCEPT = 0x1E,
        VK_MODECHANGE = 0x1F,
        VK_SPACE = 0x20,//SPACEBAR
        VK_PRIOR = 0x21, //PAGE UP key(ExtendedKey)
        VK_NEXT = 0x22, //PAGE DOWN key(ExtendedKey)
        VK_END = 0x23,//END key(ExtendedKey)
        VK_HOME = 0x24, //HOME key(ExtendedKey)
        VK_LEFT = 0x25, //← key(ExtendedKey)
        VK_UP = 0x26, //↑ key(ExtendedKey)
        VK_RIGHT = 0x27, //→ key(ExtendedKey)
        VK_DOWN = 0x28, //↓ key(ExtendedKey)
        VK_SELECT = 0x29,
        VK_PRINT = 0x2A,
        VK_EXECUTE = 0x2B,
        VK_SNAPSHOT = 0x2C, //PRINT SCREEN key(ExtendedKey)
        VK_INSERT = 0x2D, //INS key(ExtendedKey)
        VK_DELETE = 0x2E, //DEL key(ExtendedKey)
        VK_HELP = 0x2F,
        VK_0 = 0x30,//0 key
        VK_1 = 0x31, //1 key
        VK_2 = 0x32, //2 key
        VK_3 = 0x33, //3 key
        VK_4 = 0x34, //4 key
        VK_5 = 0x35, //5 key
        VK_6 = 0x36, //6 key
        VK_7 = 0x37, //7 key
        VK_8 = 0x38, //8 key
        VK_9 = 0x39, //9 key
        VK_A = 0x41, //A key
        VK_B = 0x42, //B key
        VK_C = 0x43, //C key
        VK_D = 0x44, //D key
        VK_E = 0x45, //E key
        VK_F = 0x46, //F key
        VK_G = 0x47, //G key
        VK_H = 0x48, //H key
        VK_I = 0x49, //I key
        VK_J = 0x4A, //J key
        VK_K = 0x4B, //K key
        VK_L = 0x4C, //L key
        VK_M = 0x4D, //M key
        VK_N = 0x4E, //N key
        VK_O = 0x4F, //O key
        VK_P = 0x50, //P key
        VK_Q = 0x51, //Q key
        VK_R = 0x52, //R key
        VK_S = 0x53, //S key
        VK_T = 0x54, //T key
        VK_U = 0x55, //U key
        VK_V = 0x56, //V key
        VK_W = 0x57, //W key
        VK_X = 0x58, //X key
        VK_Y = 0x59, //Y key
        VK_Z = 0x5A, //Z key
        VK_LWIN = 0x5B, //Left Windows key
        VK_RWIN = 0x5C, //Right Windows key
        VK_APPS = 0x5D, // Applications key
        VK_NUMPAD0 = 0x60, //Numeric keypad 0 key
        VK_NUMPAD1 = 0x61, //Numeric keypad 1 key
        VK_NUMPAD2 = 0x62, //Numeric keypad 2 key
        VK_NUMPAD3 = 0x63, //Numeric keypad 3 key
        VK_NUMPAD4 = 0x64, //Numeric keypad 4 key
        VK_NUMPAD5 = 0x65, //Numeric keypad 5 key
        VK_NUMPAD6 = 0x66, //Numeric keypad 6 key
        VK_NUMPAD7 = 0x67, //Numeric keypad 7 key
        VK_NUMPAD8 = 0x68, //Numeric keypad 8 key
        VK_NUMPAD9 = 0x69, //Numeric keypad 9 key
        VK_MULTIPLY = 0x6A, //* key
        VK_ADD = 0x6B, //+ key
        VK_SEPERATOR = 0x6C, //
        VK_SUBTRACT = 0x6D, //- key
        VK_DECIMAL = 0x6E, //テンキーの . key
        VK_DEVIDE = 0x6F, /// key(ExtendedKey)
        VK_F1 = 0x70, //F1 key
        VK_F2 = 0x71, //F2 key
        VK_F3 = 0x72, //F3 key
        VK_F4 = 0x73, //F4 key
        VK_F5 = 0x74,//F5 key
        VK_F6 = 0x75, //F6 key
        VK_F7 = 0x76, //F7 key
        VK_F8 = 0x77, //F8 key
        VK_F9 = 0x78, //F9 key
        VK_F10 = 0x79, //F10 key
        VK_F11 = 0x7A, //F11 key
        VK_F12 = 0x7B, //F12 key
        VK_F13 = 0x7C, //F13 key
        VK_F14 = 0x7D, //F14 key
        VK_F15 = 0x7E, //F15 key
        VK_F16 = 0x7F, //F16 key
        VK_F17 = 0x80, //F17 key
        VK_F18 = 0x81, //F18 key
        VK_F19 = 0x82, //F19 key
        VK_F20 = 0x83, //F20 key
        VK_F21 = 0x84, //F21 key
        VK_F22 = 0x85, //F22 key
        VK_F23 = 0x86, //F23 key
        VK_F24 = 0x87, //F24 key
        VK_NUMLOCK = 0x90, //NUM LOCK key(ExtendedKey)
        VK_SCROLL = 0x91, //SCROLL LOCK key
        VK_LSHIFT = 0xA0, //Left SHIFT key
        VK_RSHIFT = 0xA1, // Right SHIFT key(ExtendedKey)
        VK_LCONTROL = 0xA2, //Left CONTROL key
        VK_RCONTROL = 0xA3, //Right CONTROL key(ExtendedKey)
        VK_LMENU = 0xA4, //Left MENU key
        VK_RMENU = 0xA5, //Right MENU key(ExtendedKey)
        VK_BROWSER_BACK = 0xA6, //Browser Back key
        VK_BROWSER_FORWARD = 0xA7, //Browser Forward key
        VK_BROWSER_REFRESH = 0xA8, //Browser Refresh key
        VK_BROWSER_STOP = 0xA9, //Browser Stop key
        VK_BROWSER_SEARCH = 0xAA, //Browser Search key
        VK_BROWSER_FAVORITES = 0xAB, //Browser Favorites key
        VK_BROWSER_HOME = 0xA6, //Browser Start and Home key
        VK_VOLUME_MUTE = 0xAD, //Volume Mute key
        VK_VOLUME_DOWN = 0xAE, //Volume Down key
        VK_VOLUME_UP = 0xAF, //Volume Up key
        VK_MEDIA_NEXT_TRACK = 0xB0, //Next Track key
        VK_MEDIA_PREV_TRACK = 0xB1, //Previous Track key
        VK_MEDIA_STOP = 0xB2, //Stop Media key
        VK_MEDIA_PLAY_PAUSE = 0xB3, //Play/Pause Media key
        VK_LAUNCH_MAIL = 0xB4, //Start Mail key
        VK_LAUNCH_MEDIA_SELECT = 0xB5, //Select Media Key
        VK_LAUNCH_APP1 = 0xB6, //Start Application 1 key
        VK_LAUNCH_APP2 = 0xB7, //Start Application 2 key
        VK_OEM_1 = 0xBA, //: *  key
        VK_OEM_PLUS = 0xBB, //; + key
        VK_OEM_COMMA = 0xBC, //, < key
        VK_OEM_MINUS = 0xBD, //- = key
        VK_OEM_PERIOD = 0xBE, //. > key
        VK_OEM_2 = 0xBF, /// ? key
        VK_OEM_3 = 0xC0, //@ ` key
        VK_OEM_4 = 0xDB, //[ { key
        VK_OEM_5 = 0xDC, //\ | key
        VK_OEM_6 = 0xDD, //] } key
        VK_OEM_7 = 0xDE, //^ ~ key
        VK_OEM_8 = 0xDF,
        VK_PROCESSKEY = 0xE5,
        VK_OEM_ATTN = 0xF0, //英数
        VK_OEM_102 = 0xE2, //\ _ key
        VK_OEM_COPY = 0xF2, //カタカナひらがな
        VK_OEM_AUTO = 0xF3, //全角/半角
        VK_OEM_ENLW = 0xF4,//全角/半角
        VK_OEM_BACKTAB = 0xF5, //ローマ字
        VK_PACKET = 0xE7,
        VK_ATTN = 0xF6,
        VK_CRSEL = 0xF7,
        VK_EXSEL = 0xF8,
        VK_EREOF = 0xF9,
        VK_PLAY = 0xFA,
        VK_ZOOM = 0xFB,
        VK_NONAME = 0xFC,
        VK_PA1 = 0xFD,
        VK_OEM_CLEAR = 0xFE
    }
}
