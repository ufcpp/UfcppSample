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
    }

    [Serializable]
    public struct KeyEvent
    {
        public EventType Type;
        public int Code;
        public long Elapsed;
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
                select new INPUT
                {
                    type = type.INPUT_KEYBOARD,
                    ki = new KEYBDINPUT
                    {
                        time = (int)x.Elapsed,
                        wVk = (wVk)x.Code,
                        dwFlags = x.Type == EventType.KeyDown ? dwFlags.KEYEVENTF_KEYDOWN : dwFlags.KEYEVENTF_KEYUP,
                    },
                };

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
            this.Replay();
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
            this.listHistory.ItemsSource = keys.Select(x => string.Format("{0}: {1} ({2})", x.Type, ((Key)x.Code), x.Elapsed));
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

        #endregion
    }
}
