using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PositionSetViewer
{
    public partial class Browser : Form
    {
        private Rectangle maxRectInScreen;

        public Rectangle MaxRectInScreen
        {
            get { return maxRectInScreen; }
            set { maxRectInScreen = value; }
        }

        LayersPainterForm layersPainterForm;
        LayersPaintedControl layersPaintedControl;

        public Browser(LayersPainterForm parentPainterForm)
        {
            this.layersPainterForm = parentPainterForm;

            maxRectInScreen = new Rectangle(0,0,200,150);

            this.ClientSize = new Size(maxRectInScreen.Width,maxRectInScreen.Height);

            InitializeComponent();
        }

        public Browser(LayersPaintedControl layersPaintedControl)
        {
            this.layersPaintedControl = layersPaintedControl;

            maxRectInScreen = new Rectangle(0, 0, 200, 150);

            this.ClientSize = new Size(maxRectInScreen.Width, maxRectInScreen.Height);

            InitializeComponent();
        }

        /// <summary>
        /// 获取已设置无法关闭窗口创建参数。
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                int CS_NOCLOSE = 0x200;
                CreateParams parameters = base.CreateParams;
                parameters.ClassStyle |= CS_NOCLOSE;

                return parameters;
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs cea)
        {
            cea.Cancel = true;
        }

        float scale;

        public void ParentImageIsChanged()
        {
            if (layersPaintedControl.EntireImage != null)
            {
                Bitmap bitmap = layersPaintedControl.EntireImage;

                float scaleX = (float)maxRectInScreen.Width / bitmap.Width;
                float scaleY = (float)maxRectInScreen.Height / bitmap.Height;

                scale = Math.Min(scaleX, scaleY);

                this.ClientSize = new Size((int)Math.Ceiling(bitmap.Width * scale), (int)Math.Ceiling(bitmap.Height * scale));
            }
        }

        Bitmap browserBackgroundBitmap;

        private void browser_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bitmap = layersPaintedControl.EntireImage;
            if (bitmap != null)
            {
                //if (parentImageIsChanged)
                //{
                //    browserBackgroundBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
                //    Graphics.FromImage(browserBackgroundBitmap).DrawImage(bitmap, 0, 0, bitmap.Width * scale, bitmap.Height * scale);

                //    parentImageIsChanged = false;
                //}

                //e.Graphics.DrawImage(browserBackgroundBitmap,0,0);
                e.Graphics.DrawImage(layersPaintedControl.EntireImage, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
                Rectangle r = new Rectangle((int)(layersPaintedControl.ScreenUpperLeftPositionX * scale),
                        (int)(layersPaintedControl.ScreenUpperLeftPositionY * scale),
                        (int)(layersPaintedControl.ClientRectangle.Width * scale),
                        (int)(layersPaintedControl.ClientRectangle.Height * scale));
                e.Graphics.DrawRectangle(new Pen(Brushes.Red), r);
            }
        }

        private void Browser_Resize(object sender, EventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            //base.OnResize(e);
        }

        private void Browser_Load(object sender, EventArgs e)
        {
        }
    }
}