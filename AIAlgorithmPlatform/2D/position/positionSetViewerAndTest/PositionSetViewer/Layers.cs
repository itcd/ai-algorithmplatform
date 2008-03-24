using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Position_Interface;
using System.Collections;
using System.ComponentModel;

namespace PositionSetViewer
{
    /// <summary>
    /// Layers
    /// </summary>
    public class Layers : List<Layer>
    {
        public event hander LayersCountChanged;
        public event hander UnActiveBitmapChanged;
        public event hander Clearing;

        public bool Up(Layer item)
        {
            lock (this)
            {
                int sequence = this.IndexOf(item);
                if (sequence > 0)
                {
                    Layer temp = this[sequence - 1];

                    this[sequence - 1] = item;
                    this[sequence] = temp;

                    if (item.Active == false && temp.Active == false)
                    { Invalidate(); }
                    return true;
                }
                return false;
            }
        }

        public bool Down(Layer item)
        {
            lock (this)
            {
                int sequence = this.IndexOf(item);
                if (sequence < this.Count - 1)
                {
                    Layer temp = this[sequence + 1];

                    this[sequence + 1] = item;
                    this[sequence] = temp;

                    if (item.Active == false && temp.Active == false)
                    { Invalidate(); }
                    return true;
                }
                return false;
            }
        }

        public new void Add(Layer item)
        {
            lock (this)
            {
                base.Add(item);
                item.ActiveChanged += delegate { Invalidate(); };
                item.LayerRepresentationChanged += InvalidateSpecificLayer;
                item.LayerMaxRectChanged += delegate { ReCountRect(); };

                ReCountRect();

                item.ScreenScaleX = this.ScreenScaleX;
                item.ScreenScaleY = this.ScreenScaleY;
                item.ScreenTranslationX = this.ScreenTranslationX;
                item.ScreenTranslationY = this.ScreenTranslationY;

                InvalidateSpecificLayer(item);
                if (LayersCountChanged != null)
                {
                    LayersCountChanged();
                }
            }
        }

        public new void Remove(Layer item)
        {
            lock (this)
            {
                base.Remove(item);
                if (this.Count <= 0)
                {
                    if (Clearing != null)
                    {
                        Clearing();
                    }
                }
                item.ActiveChanged -= delegate { Invalidate(); };
                item.LayerMaxRectChanged -= delegate { ReCountRect(); };
                item.LayerRepresentationChanged -= InvalidateSpecificLayer;
                ReCountRect();
                InvalidateSpecificLayer(item);
                if (LayersCountChanged != null)
                {
                    LayersCountChanged();
                }
            }
        }

        public new void Clear()
        {
            lock (this)
            {
                base.Clear();
                ReCountRect();
                unActiveBitmap = null;
                if (LayersCountChanged != null)
                {
                    LayersCountChanged();
                }
                if (Clearing != null)
                {
                    Clearing();
                }
            }
        }

        public Layers()
        {
            initMaxRectToDraw = new Rectangle(0, 0, 800, 600);
            maxRectToDraw = initMaxRectToDraw;
            realRectToDraw = initMaxRectToDraw;
            activeBitmap = new Bitmap(1, 1);
        }
        private RectangleF maxRectInLayers = new RectangleF();

        private float screenScaleX = 1;
        [BrowsableAttribute(false)]
        public float ScreenScaleX
        {
            get { return screenScaleX; }
            set
            {
                screenScaleX = value;
                foreach (Layer lay in this)
                {
                    lay.ScreenScaleX = this.ScreenScaleX;
                }
            }
        }

        private float screenScaleY = 1;
        [BrowsableAttribute(false)]
        public float ScreenScaleY
        {
            get { return screenScaleY; }
            set
            {
                screenScaleY = value;
                foreach (Layer lay in this)
                {
                    lay.ScreenScaleY = this.ScreenScaleY;
                }
            }
        }

        private float screenTranslationX = 0;
        [BrowsableAttribute(false)]
        public float ScreenTranslationX
        {
            get { return screenTranslationX; }
            set
            {
                screenTranslationX = value;
                foreach (Layer lay in this)
                {
                    lay.ScreenTranslationX = this.ScreenTranslationX;
                }
            }
        }

        private float screenTranslationY = 0;
        [BrowsableAttribute(false)]
        public float ScreenTranslationY
        {
            get { return screenTranslationY; }
            set
            {
                screenTranslationY = value;
                foreach (Layer lay in this)
                {
                    lay.ScreenTranslationY = this.ScreenTranslationY;
                }
            }
        }

        public float ConvertScreenXToRealX(float screenX)
        {
            return (screenX - ScreenTranslationX) / ScreenScaleX;
        }

        public float ConvertScreenYToRealY(float screenY)
        {
            return (screenY - ScreenTranslationY) / ScreenScaleY;
        }

        /// <summary>
        /// 在添加新图层或改变画图区大小时重新计算比例与画图坐标
        /// </summary>
        private void ReCountRect()
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            for (int i = 0; i < this.Count; i++)
            {
                RectangleF maxRectInLayer = this[i].MaxRect;

                if (maxRectInLayer.Width < float.Epsilon || maxRectInLayer.Height < float.Epsilon)
                {
                    continue;
                }

                if (i == 0)
                {
                    minX = maxRectInLayer.Left;
                    maxX = maxRectInLayer.Right;
                    minY = maxRectInLayer.Top;
                    maxY = maxRectInLayer.Bottom;
                }
                else
                {
                    if (maxRectInLayer.Left < minX)
                    {
                        minX = maxRectInLayer.Left;
                    }
                    else if (maxRectInLayer.Right > maxX)
                    {
                        maxX = maxRectInLayer.Right;
                    }

                    if (maxRectInLayer.Top < minY)
                    {
                        minY = maxRectInLayer.Top;
                    }
                    else if (maxRectInLayer.Bottom > maxY)
                    {
                        maxY = maxRectInLayer.Bottom;
                    }
                }
            }

            //如果maxRectInLayers没有改变则返回
            if ((maxRectInLayers.X.CompareTo(minX) == 0) && (maxRectInLayers.Y.CompareTo(minY) == 0) && (maxRectInLayers.Width.CompareTo(maxX - minX) == 0) && (maxRectInLayers.Height.CompareTo(maxY - minY) == 0))
            {
                return;
            }
            else
            {
                maxRectInLayers = new RectangleF(minX, minY, maxX - minX, maxY - minY);
                ReCountScreenTransform();
            }
        }

        private void ReCountScreenTransform()
        {
            //如果没有东西显示
            if (maxRectInLayers.Width <= float.Epsilon || maxRectInLayers.Height <= float.Epsilon)
            {
                maxRectInLayers = maxRectToDraw;
            }

            float set_distanceBetweenFrameAndImage = 10;

            float scaleX = (maxRectToDraw.Width - set_distanceBetweenFrameAndImage * 2) / maxRectInLayers.Width;
            float scaleY = (maxRectToDraw.Height - set_distanceBetweenFrameAndImage * 2) / maxRectInLayers.Height;

            float scale = Math.Min(scaleX, scaleY);

            this.ScreenScaleX = scale;
            this.ScreenScaleY = -scale;
            this.ScreenTranslationX = -(maxRectInLayers.X * scale) + set_distanceBetweenFrameAndImage;
            this.ScreenTranslationY = maxRectInLayers.Bottom * scale + set_distanceBetweenFrameAndImage;

            realRectToDraw = new Rectangle(0, 0, (int)(maxRectInLayers.Width * scale + set_distanceBetweenFrameAndImage * 2),
                (int)(maxRectInLayers.Height * scale + set_distanceBetweenFrameAndImage * 2));
        }

        private Bitmap unActiveBitmap = null;
        public Bitmap UnActiveBitmap
        {
            get { return unActiveBitmap; }
        }
        private Bitmap activeBitmap = null;
        public void Draw(Graphics graphic, int x,int y, int width, int height)
        {
            try
            {
                lock (this)
                {
                    if (isUnActiveLayerChanged)
                    {
                        unActiveBitmap = new Bitmap(RealRectToDraw.Width, RealRectToDraw.Height);

                        Graphics graphicsFromUnActiveBitmap = Graphics.FromImage(unActiveBitmap);
                        graphicsFromUnActiveBitmap.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        graphicsFromUnActiveBitmap.Clear(Color.White);
                        foreach (Layer lay in this)
                        {
                            if (!lay.Active)
                            {
                                lay.Draw(graphicsFromUnActiveBitmap);
                            }
                        }

                        if (UnActiveBitmapChanged != null)
                        {
                            UnActiveBitmapChanged();
                        }
                        isUnActiveLayerChanged = false;
                    }

                    bool isActiveBitmapExit = false;
                    bool isUnActiveBitmapExit = false;
                    foreach (Layer lay in this)
                    {
                        if (lay.Active)
                        {
                            isActiveBitmapExit = true;
                        }
                        else
                        {
                            isUnActiveBitmapExit = true;
                        }
                    }

                    //if (isActiveBitmapExit)
                    //{
                    //    if (isActiveLayerChanged)
                    //    {
                    //        Graphics graphicsFromActiveBitmap;

                    //        if (width != activeBitmap.Width || height != activeBitmap.Height)
                    //        {
                    //            activeBitmap = new Bitmap(width, height);
                    //            graphicsFromActiveBitmap = Graphics.FromImage(activeBitmap);
                    //        }
                    //        else
                    //        { 
                    //            graphicsFromActiveBitmap = Graphics.FromImage(activeBitmap);
                    //            graphicsFromActiveBitmap.Clear(Color.Transparent);
                    //        }                    

                    //        graphicsFromActiveBitmap.TranslateTransform(x, y);

                    //        foreach (Layer lay in this)
                    //        {
                    //            if (lay.Active)
                    //            {
                    //                lay.Draw(graphicsFromActiveBitmap);
                    //            }
                    //        }
                    //    }
                    //}

                    if (isUnActiveBitmapExit)
                    {
                        if (unActiveBitmap.Width < width || unActiveBitmap.Height < height)
                        {
                            graphic.Clear(Color.White);
                        }
                        graphic.DrawImageUnscaled(unActiveBitmap, x, y, width, height);
                    }
                    else
                    {
                        graphic.Clear(Color.White);
                    }

                    graphic.TranslateTransform(x, y);
                    foreach (Layer lay in this)
                    {
                        if (lay.Active)
                        {
                            lay.Draw(graphic);
                        }
                    }

                    //if (isActiveBitmapExit)
                    //{
                    //    graphic.DrawImageUnscaledAndClipped(activeBitmap, new Rectangle(0, 0, width, height));
                    //}

                    //bitmap.Save(@"ScreenImage" + DateTime.Now.Ticks);

                    //    imageIsChanged = false;
                    //}            
                }
            }
            catch (System.InvalidOperationException e)
            {
            }
        }

        //private void UnActiveLayerChanged()
        //{
            
        //}

        public void Invalidate()
        {
            isActiveLayerChanged = true;
            isUnActiveLayerChanged = true;
        }

        bool isActiveLayerChanged = true;
        bool isUnActiveLayerChanged = true;
        public void InvalidateSpecificLayer(Layer layer)
        {
            if (layer.Active)
            {                
                isActiveLayerChanged = true;
            }
            else 
            {
                isUnActiveLayerChanged = true;
            }
        }

        private Rectangle realRectToDraw;
        public Rectangle RealRectToDraw
        {
            get { return realRectToDraw; }
        }

        private Rectangle maxRectToDraw;
        public Rectangle MaxRectToDraw
        {
            get { return maxRectToDraw; }
            set { maxRectToDraw = value; ReCountScreenTransform(); }
        }

        private Rectangle initMaxRectToDraw;
        public Rectangle InitMaxRectToDraw
        {
            get { return initMaxRectToDraw; }            
        }
    }
}