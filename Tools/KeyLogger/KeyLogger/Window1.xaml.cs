using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Threading;

using Key = System.Windows.Forms.Keys;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace KeyLogger
{
    public enum EventType
    {
        KeyDown,
        KeyUp,
        MouseDown,
        MouseUp,
    }

    [Serializable]
    public struct KeyEvent
    {
        public EventType Type;
        public int Code;
        public int X;
        public int Y;
        public long Elapsed;
    }

    public static class KeyEventExtensions
    {
        public static string ToHistoryString(this KeyEvent x)
        {
            switch (x.Type)
            {
                case EventType.KeyDown:
                    return $"KeyDown, {(Key)x.Code}, {x.Elapsed}";
                case EventType.KeyUp:
                    return $"KeyUp  , {(Key)x.Code}, {x.Elapsed}";
                case EventType.MouseDown:
                    return $"MouseDown, {(System.Windows.Forms.MouseButtons)x.Code}, ({x.X}, {x.Y}), {x.Elapsed}";
                case EventType.MouseUp:
                    return $"MouseUp  , {(System.Windows.Forms.MouseButtons)x.Code}, ({x.X}, {x.Y}), {x.Elapsed}";
                default:
                    return "";
            }
        }

        public static INPUT ToInput(this KeyEvent x)
        {
            switch (x.Type)
            {
                default:
                case EventType.KeyDown:
                case EventType.KeyUp:
                    return new INPUT
                    {
                        type = type.INPUT_KEYBOARD,
                        ki = new KEYBDINPUT
                        {
                            time = (int)x.Elapsed,
                            wVk = (wVk)x.Code,
                            dwFlags = x.Type == EventType.KeyDown ? dwFlags.KEYEVENTF_KEYDOWN : dwFlags.KEYEVENTF_KEYUP,
                        },
                    };
                case EventType.MouseDown:
                case EventType.MouseUp:
                    dwFlags f = 0;

                    switch ((System.Windows.Forms.MouseButtons)x.Code)
                    {
                        case System.Windows.Forms.MouseButtons.Left:
                            f = x.Type == EventType.MouseDown ? dwFlags.MOUSEEVENTF_LEFTDOWN : dwFlags.MOUSEEVENTF_LEFTUP;
                            break;
                        case System.Windows.Forms.MouseButtons.Right:
                            f = x.Type == EventType.MouseDown ? dwFlags.MOUSEEVENTF_RIGHTDOWN : dwFlags.MOUSEEVENTF_RIGHTUP;
                            break;
                        case System.Windows.Forms.MouseButtons.Middle:
                            f = x.Type == EventType.MouseDown ? dwFlags.MOUSEEVENTF_MIDDLEDOWN : dwFlags.MOUSEEVENTF_MIDDLEUP;
                            break;
                        default:
                            break;
                    }

                    return new INPUT
                    {
                        type = type.INPUT_MOUSE,
                        mi = new MOUSEINPUT
                        {
                            time = (int)x.Elapsed,
                            dx = x.X,
                            dy = x.Y,
                            dwFlags = f,
                        },
                    };
            }
        }
    }

    /// <summary>
    /// Window1.xaml の相互作用ロジック
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            Gma.UserActivityMonitor.HookManager.KeyDown += new System.Windows.Forms.KeyEventHandler(HookManager_KeyDown);
            //Gma.UserActivityMonitor.HookManager.KeyPress += new System.Windows.Forms.KeyPressEventHandler(HookManager_KeyPress);
            Gma.UserActivityMonitor.HookManager.KeyUp += new System.Windows.Forms.KeyEventHandler(HookManager_KeyUp);
            Gma.UserActivityMonitor.HookManager.MouseDown += HookManager_MouseDown;
            Gma.UserActivityMonitor.HookManager.MouseUp += HookManager_MouseUp; ;

            this.buttonRecord.Click += new RoutedEventHandler(buttonRecord_Click);
            this.buttonClear.Click += new RoutedEventHandler(buttonClear_Click);
            this.buttonReduce.Click += new RoutedEventHandler(buttonReduce_Click);
            this.buttonSave.Click += new RoutedEventHandler(buttonSave_Click);
            this.buttonLoad.Click += new RoutedEventHandler(buttonLoad_Click);
            this.buttonReplay.Click += new RoutedEventHandler(buttonReplay_Click);
        }

        void buttonReduce_Click(object sender, RoutedEventArgs e)
        {
            var reduced = new List<KeyEvent>();

            KeyEvent prev = new KeyEvent { Type = (EventType)(-1) };
            foreach (var k in keys)
            {
                if (prev.Type == k.Type
                    && k.Type == EventType.KeyDown
                    && prev.Code == k.Code)
                {
                    prev = k;
                    continue;
                }

                reduced.Add(k);
                prev = k;
            }

            this.keys = reduced;

            this.ShowHistory();
        }

        #region 再生

        Thread replay = null;
        bool isReplayAlive;

        void Dispatch(Action a)
        {
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, a);
        }

        void ReplayThread()
        {
            var inputs =
                from x in this.keys
                select x.ToInput();

            Stopwatch sw = new Stopwatch();

            sw.Start();

            foreach (var input in inputs)
            {
                while (input.ki.time > sw.ElapsedMilliseconds)
                {
                    Thread.Sleep(1);
                    //this.Dispatch(() => { this.labelDebug.Content = string.Format("{0} : {1}", input.ki.time, sw.ElapsedMilliseconds); });
                }

                Win32.SendInput(input);

                if (!this.isReplayAlive)
                    break;
            }

            sw.Stop();

            this.Dispatch(() =>
            {
                this.buttonReplay.IsEnabled = true;
                this.replay = null;
                this.isReplayAlive = false;
            });
        }

        void Replay()
        {
            if (this.isReplayAlive)
            {
                this.isReplayAlive = false;
                return;
            }

            this.isReplayAlive = true;
            this.replay = new Thread(this.ReplayThread)
            {
                Priority = ThreadPriority.AboveNormal,
            };

            this.buttonReplay.IsEnabled = false;
            this.replay.Start();
        }

        void buttonReplay_Click(object sender, RoutedEventArgs e)
        {
            //Keyboard.Focus(this.textResult);
            int repeat;
            if (!int.TryParse(textRepeat.Text, out repeat)) repeat = 1;

            for (int i = 0; i < repeat; i++)
            {
                this.Replay();
            }
        }

        #endregion
        #region シリアライズ

        XmlSerializer serializer = null;
        XmlSerializer Serializer
        {
            get
            {
                if (this.serializer == null)
                {
                    this.serializer = new XmlSerializer(typeof(List<KeyEvent>));
                }
                return this.serializer;
            }
        }

        void Serialize(Stream stream)
        {
            this.Serializer.Serialize(stream, this.keys);
        }

        void Deserialize(Stream stream)
        {
            this.keys = (List<KeyEvent>)this.Serializer.Deserialize(stream);

            this.ShowHistory();
        }

        void Serialize()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = false,
                Filter = "XML|*.xml",
                AddExtension = true,
                CheckFileExists = false,
            };

            if (dlg.ShowDialog() ?? false)
            {
                using (var stream = File.Open(dlg.FileName, FileMode.Create))
                {
                    this.Serialize(stream);
                }
            }
        }

        void Deserialize()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = false,
                Filter = "XML|*.xml",
                AddExtension = true,
                CheckFileExists = true,
            };

            if (dlg.ShowDialog() ?? false)
            {
                using (var stream = File.OpenRead(dlg.FileName))
                {
                    this.Deserialize(stream);
                }
            }
        }

        void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            this.Deserialize();
        }

        void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            this.Serialize();
        }

        void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            this.keys.Clear();
            this.ShowHistory();
        }

        #endregion
        #region 記録開始/停止

        bool isRecording = false;
        Stopwatch stopwatch = new Stopwatch();
        List<KeyEvent> keys = new List<KeyEvent>();

        void SwitchRecording()
        {
            if (this.isRecording)
            {
                this.isRecording = false;
                this.buttonRecord.Content = "記録開始（右Ctrl）";
            }
            else
            {
                this.isRecording = true;
                this.stopwatch.Reset();
                this.stopwatch.Start();
                this.buttonRecord.Content = "記録停止（右Ctrl）";
            }
        }

        void buttonRecord_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchRecording();
        }

        #endregion
        #region ログの表示

        void ShowHistory()
        {
            this.listHistory.ItemsSource = keys.Select(x => x.ToHistoryString());
        }

        #endregion
        #region フック

        void HookManager_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.RControlKey
                || e.KeyCode == System.Windows.Forms.Keys.RMenu
                || e.KeyCode == System.Windows.Forms.Keys.F12)
            {
                e.Handled = true;
                return;
            }

            if (this.replay != null && this.replay.IsAlive)
            {
                //this.labelDebug.Content = string.Format("{0}", e.KeyCode);
                return;
            }

            if (!this.isRecording)
                return;

            this.keys.Add(new KeyEvent
            {
                Type = EventType.KeyDown,
                Code = (int)e.KeyCode,
                Elapsed = this.stopwatch.ElapsedMilliseconds,
            });

            //this.ShowHistory();
        }

        void HookManager_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.RMenu
                || e.KeyCode == System.Windows.Forms.Keys.F12)
            {
                this.Replay();
                e.Handled = true;
                return;
            }

            if (this.replay != null && this.replay.IsAlive)
            {
                e.Handled = false;
                return;
            }

            if (e.KeyCode == System.Windows.Forms.Keys.RControlKey)
            {
                this.SwitchRecording();
                this.ShowHistory();
                e.Handled = true;
                return;
            }

            if (!this.isRecording)
                return;

            this.keys.Add(new KeyEvent
            {
                Type = EventType.KeyUp,
                Code = (int)e.KeyCode,
                Elapsed = this.stopwatch.ElapsedMilliseconds,
            });

            //this.ShowHistory();
        }

        private void HookManager_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.replay != null && this.replay.IsAlive)
            {
                return;
            }

            if (!this.isRecording)
                return;

            this.keys.Add(new KeyEvent
            {
                Type = EventType.MouseDown,
                Code = (int)e.Button,
                X = e.X,
                Y = e.Y,
                Elapsed = this.stopwatch.ElapsedMilliseconds,
            });
        }

        private void HookManager_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.replay != null && this.replay.IsAlive)
            {
                return;
            }

            if (!this.isRecording)
                return;

            this.keys.Add(new KeyEvent
            {
                Type = EventType.MouseUp,
                Code = (int)e.Button,
                X = e.X,
                Y = e.Y,
                Elapsed = this.stopwatch.ElapsedMilliseconds,
            });
        }

        #endregion
    }
}
