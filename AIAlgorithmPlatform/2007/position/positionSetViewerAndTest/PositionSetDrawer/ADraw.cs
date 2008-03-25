using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Position_Interface;

namespace PositionSetDrawer
{
    public delegate void dPropertyChanged();

    public abstract class ADraw
    {
        protected Graphics graphics = null;

        protected ADraw()
        {
        }

        public event dPropertyChanged DrawerPropertyChanged;

        protected void SpringDrawerPropertyChangedEvent()
        {
            if (DrawerPropertyChanged != null)
            {
                DrawerPropertyChanged();
            }
        }

        public void SetGraphics(Graphics graphics)
        {
            this.graphics = graphics;
        }

        private bool visible = true;
    
        public bool Visible
        {
            get { return visible; }
            set 
            {
                if (visible != value)
                {
                    visible = value;
                    SpringDrawerPropertyChangedEvent();
                }
            }
        }
    }
}
