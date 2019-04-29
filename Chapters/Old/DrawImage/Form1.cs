using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace DrawImage
{
  delegate void WorkerThread();

  public partial class Form1 : Form
  {
    #region 定数定義

    private const int WEIGHT = 10;

    #endregion
    #region ここの中身を更新してください。

    // 例として、赤い点がパネル中を動き回るプログラム。

    int x = 0;
    int y = 0;
    int dx = 3;
    int dy = 2;

    /// <summary>
    /// this.panel1 に表示される内容を更新する。
    /// WEIGHT ミリ秒に1回呼ばれる。
    /// </summary>
    /// <param name="g">この g に対する描画が this.panel1 に反映される。</param>
    /// <param name="width">this.panel1 の幅。</param>
    /// <param name="height">this.panel1 の高さ。</param>
    private void UpdatePanel(Graphics g, int width, int height)
    {
      x += dx;
      if (x > width) { x = width; dx = -dx; }
      else if(x < 0) { x = 0; dx = -dx; }
      y += dy;
      if (y > height) { y = height; dy = -dy; }
      else if(y < 0) { y = 0; dy = -dy; }

      g.Clear(Color.White);
      g.FillEllipse(new SolidBrush(Color.Red), x, y, 5, 5);
    }

    #endregion
    #region 以下、雛形。書き換える必要なし。

    public Form1()
    {
      InitializeComponent();

      this.isAlive = true;
      this.wthread = this.MainThread;
      this.ar = this.wthread.BeginInvoke(null, null);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      this.isAlive = false;
      this.wthread.EndInvoke(this.ar);
      base.OnClosing(e);
    }

    private bool isAlive;
    private Bitmap backBuffer;
    private WorkerThread wthread;
    IAsyncResult ar;

    private void MainThread()
    {
      while (this.isAlive)
      {
        lock (this)
        {
          if (this.backBuffer == null)
          {
            this.backBuffer = new Bitmap(this.panel1.Width, this.panel1.Height);
          }
          Graphics g = Graphics.FromImage(this.backBuffer);
          this.UpdatePanel(g, this.panel1.Width, this.panel1.Height);
          g.Dispose();
        }
        this.panel1.Invalidate();
        Thread.Sleep(WEIGHT);
      }
    }

    private void panel1_Resize(object sender, EventArgs e)
    {
      lock (this)
      {
        if (this.backBuffer != null)
        {
          this.backBuffer.Dispose();
          this.backBuffer = null;
        }
      }
    }

    private void panel1_Paint(object sender, PaintEventArgs e)
    {
      lock (this)
      {
        if (this.backBuffer != null)
        {
          e.Graphics.DrawImageUnscaled(this.backBuffer, 0, 0);
        }
      }
    }

    #endregion
  }
}