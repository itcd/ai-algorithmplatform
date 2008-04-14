using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Position_Interface;
using PositionSetDrawer;
using System.ComponentModel;
using M2M;
using DrawingLib;
using System.Reflection;

namespace PositionSetViewer
{
    public delegate void hander();
    public delegate void dLayerChanged(Layer layer);

    /// <summary>
    /// 图层
    /// </summary>
    public abstract class Layer
    {
        public Layer()
        {
            Type layerType = this.GetType();
            PropertyInfo[] layerPropertyInfos = layerType.GetProperties();

            foreach (PropertyInfo layerPropertyInfo in layerPropertyInfos)
            {
                Type baseType = layerPropertyInfo.PropertyType.BaseType;

                if (baseType == typeof(ADraw))
                {
                    object o = layerPropertyInfo.GetValue(this, null);
                    ((ADraw)o).DrawerPropertyChanged += delegate { SpringLayerRepresentationChangedEvent(this); };                    
                }
            }
        }

        protected void SetDrawerGraphic(Graphics graphics)
        {
            Type layerType = this.GetType();
            PropertyInfo[] layerPropertyInfos = layerType.GetProperties();

            foreach (PropertyInfo layerPropertyInfo in layerPropertyInfos)
            {
                Type baseType = layerPropertyInfo.PropertyType.BaseType;

                if (baseType == typeof(ADraw))
                {
                    object o = layerPropertyInfo.GetValue(this, null);
                    ((ADraw)o).SetGraphics(graphics);
                }
            }
        }

        //当层的显示区域发生变化（一般是变大）的时候调用
        public event dLayerChanged LayerMaxRectChanged;
        public virtual void SpringLayerMaxRectChangedEvent()
        {
            if (LayerMaxRectChanged != null)
                LayerMaxRectChanged(this);
            SpringLayerRepresentationChangedEvent(this);
        }

        //当层的显示内容发生变化的时候调用（但如果显示内容超出原来显示区域，需要调用SpringLayerMaxRectChangedEvent函数）
        public event dLayerChanged LayerRepresentationChanged;
        public virtual void SpringLayerRepresentationChangedEvent(Layer layer)
        {
            if (LayerRepresentationChanged != null)
                LayerRepresentationChanged(layer);
        }

        public event hander ActiveChanged;

        public abstract void Draw(Graphics graphics);

        protected bool transformIsChanged = true;

        public int ConvertRealXToScreenX(float realX)
        {
            return (int)(realX * ScreenScaleX + ScreenTranslationX);
        }

        public int ConvertRealYToScreenY(float realY)
        {
            return (int)(realY * ScreenScaleY + ScreenTranslationY);
        }

        protected Rectangle GetPartScreenRectangleInSpecificLevel(ILevel level, int sequenceX, int sequenceY)
        {
            return new Rectangle((int)ConvertRealXToScreenX(level.ConvertPartSequenceXToRealValue(sequenceX)), 
                (int)ConvertRealYToScreenY(level.ConvertPartSequenceYToRealValue(sequenceY + 1)), 
                (int)Math.Abs(level.GetGridWidth() * ScreenScaleX), (int)Math.Abs(level.GetGridHeight() * ScreenScaleY));
        }

        protected Rectangle GetScreenRectangleFormRealBound(float upperBound, float lowerBound, float leftBound, float rightBound)
        {
            return new Rectangle(ConvertRealXToScreenX(leftBound),
                ConvertRealYToScreenY(upperBound),
                (int)Math.Abs((rightBound - leftBound) * ScreenScaleX), (int)Math.Abs((upperBound - lowerBound) * ScreenScaleY));
        }

        private string name = "default";
        [CategoryAttribute("Layer")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Active属性设为true的层总是覆盖在Active属性设为false的层之上
        /// Active属性默认为fasle，应为变动频繁，绘制时间较短的层的Active属性设为true
        /// </summary>
        private bool active = false;
        [CategoryAttribute("Layer")]
        public bool Active
        {
            get { return active; }
            set
            {
                if (value != active)
                {
                    if (ActiveChanged != null)
                    {
                        ActiveChanged();
                    }
                    active = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置层的可见性
        /// </summary>
        private bool visible = true;
        [CategoryAttribute("Layer")]
        public bool Visible
        {
            get { return visible; }
            set
            {
                if (visible != value)
                {
                    visible = value;
                    SpringLayerRepresentationChangedEvent(this);
                }
            }
        }

        private bool visibleInOptDlg = true;
        [BrowsableAttribute(false)]
        public bool VisibleInOptDlg
        {
            get { return visibleInOptDlg; }
            set { visibleInOptDlg = value; }
        }

        /// <summary>
        /// 层的主要颜色，如果没有则设为null
        /// </summary>
        [BrowsableAttribute(false)]
        public abstract Color MainColor
        {
            get;
            set;
        }
        
        /// <summary>
        /// 返回包围整个画图区域的最小矩形
        /// </summary>
        protected RectangleF maxRect = new RectangleF();
        [BrowsableAttribute(false)]
        public virtual RectangleF MaxRect
        {
            get { return maxRect; }
        }        

        private float screenScaleX = 1;
        [BrowsableAttribute(false)]
        public float ScreenScaleX
        {
            get { return screenScaleX; }
            set 
            {
                if (value != screenScaleX)
                {
                    screenScaleX = value;
                    transformIsChanged = true;
                    SpringLayerRepresentationChangedEvent(this);
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
                if (value != screenScaleY)
                {
                    screenScaleY = value;
                    transformIsChanged = true;
                    SpringLayerRepresentationChangedEvent(this);
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
                if (value != screenTranslationX)
                {
                    screenTranslationX = value;
                    transformIsChanged = true;
                    SpringLayerRepresentationChangedEvent(this);
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
                if (value != screenTranslationY)
                {
                    screenTranslationY = value;
                    transformIsChanged = true;
                    SpringLayerRepresentationChangedEvent(this);
                }
            }
        }
    }

    /// <summary>
    /// Add new layer style below
    /// </summary>
    #region custom layers

    public class Layer_Grid : Layer
    {
        int unitNumInWidth;
        int unitNumInHeight;
        float gridWidth;
        float gridHeight;
        float upperLeftX;
        float upperLeftY;

        public Layer_Grid(int unitNumInWidth, int unitNumInHeight, 
           float gridWidth, float gridHeight, float upperLeftX, float upperLeftY)
        {
            this.unitNumInWidth = unitNumInWidth;
            this.unitNumInHeight = unitNumInHeight;
            this.gridHeight = gridHeight;
            this.gridWidth = gridWidth;
            this.upperLeftX = upperLeftX;
            this.upperLeftY = upperLeftY;

            maxRect = GetMaxRect();

            MainColor = Color.FromArgb(192, 192, 192);
        }

        public Layer_Grid(ILevel M2MLevel)
        {
            this.unitNumInWidth = M2MLevel.GetUnitNumInWidth();
            this.unitNumInHeight = M2MLevel.GetUnitNumInHeight();
            this.gridHeight = M2MLevel.GetGridHeight();
            this.gridWidth = M2MLevel.GetGridWidth();
            this.upperLeftX = M2MLevel.ConvertPartSequenceXToRealValue(0);
            this.upperLeftY = M2MLevel.ConvertPartSequenceYToRealValue(0);

            maxRect = GetMaxRect();

            MainColor = Color.FromArgb(192, 192, 192);
        }
        
        public override Color MainColor
        {
            get { return gridLineDrawer.LineColor; }
            set { gridLineDrawer.LineColor = value; SpringLayerRepresentationChangedEvent(this); }
        }

        OpenLineDrawer gridLineDrawer = new OpenLineDrawer();
        public OpenLineDrawer GridLine
        {
            get { return gridLineDrawer; }
            set { gridLineDrawer = value; SpringLayerRepresentationChangedEvent(this); }
        }
        
        private RectangleF GetMaxRect()
        {
            return new RectangleF(upperLeftX, upperLeftY, unitNumInWidth * gridWidth, unitNumInHeight * gridHeight);
        }

        public override void Draw(Graphics graphics)
        {
            if (Visible)
            {
                GridLine.SetGraphics(graphics);

                float screenLeftMostX = ConvertRealXToScreenX(upperLeftX);
                float screenRightMostX = ConvertRealXToScreenX(upperLeftX + unitNumInWidth * gridWidth);
                for(int i = 0;i <= unitNumInHeight; i++)
                {
                    float screenLineY = ConvertRealYToScreenY(upperLeftY + i * gridHeight);

                    GridLine.DrawFirstPoint(screenLeftMostX, screenLineY);
                    GridLine.Draw(screenRightMostX, screenLineY);
                }

                float screenTopY = ConvertRealYToScreenY(upperLeftY);
                float screenBottomY = ConvertRealYToScreenY(upperLeftY + unitNumInHeight * gridHeight);
                for (int j = 0; j <= unitNumInWidth; j++)
                {
                    float screenLineX = ConvertRealXToScreenX(upperLeftX + j * gridWidth);

                    GridLine.DrawFirstPoint(screenLineX, screenTopY);
                    GridLine.Draw(screenLineX, screenBottomY);
                }
            }
        }
    }

    public class Layer_M2MStructureGrid : Layer
    {
        IM2MStructure m2mStructure;

        Layer_Grid layer_Grid;

        public Layer_M2MStructureGrid(IM2MStructure m2mStructure)
        {
            this.m2mStructure = m2mStructure;
            maxRect = GetMaxRect();
        }

        private RectangleF GetMaxRect()
        {
            layer_Grid = new Layer_Grid(m2mStructure.GetLevel(0));
            return layer_Grid.MaxRect;
        }

        public override Color MainColor
        {
            get { return gridColor; }
            set { gridColor = value; }
        }

        Color gridColor = Color.FromArgb(192,192,192);
        [CategoryAttribute("Drawer")]
        public Color GridColor
        {
            get { return gridColor; }
            set { gridColor = value; SpringLayerRepresentationChangedEvent(this); }
        }

        int alpha = 100;
        [CategoryAttribute("Drawer")]
        public int Alpha
        {
            get { return alpha; }
            set 
            {
                if (value > 255)
                {
                    alpha = 255;
                }
                else if (value < 0)
                {
                    alpha = 0;
                }
                else
                {
                    alpha = value;
                }

                SpringLayerRepresentationChangedEvent(this);
            }
        }

        public override void Draw(Graphics graphics)
        {
            if (Visible)
            {
                Color tempColor = GridColor;

                for (int i = m2mStructure.GetLevelNum() - 1; i > 0; i--)
                {
                    layer_Grid = new Layer_Grid(m2mStructure.GetLevel(i));

                    layer_Grid.ScreenScaleX = this.ScreenScaleX;
                    layer_Grid.ScreenScaleY = this.ScreenScaleY;
                    layer_Grid.ScreenTranslationX = this.ScreenTranslationX;
                    layer_Grid.ScreenTranslationY = this.ScreenTranslationY;

                    layer_Grid.Visible = true;
                    tempColor = GenerateColor.GetSimilarColor(tempColor);
                    layer_Grid.GridLine.LineColor = Color.FromArgb(alpha,tempColor);
                    layer_Grid.GridLine.LineWidth = (float)((m2mStructure.GetLevelNum() - i));
                    layer_Grid.Draw(graphics);
                }
            }
        }
    }

    public class Layer_M2MLevel : Layer
    {
        ILevel level;

        Layer_Grid layer_Grid;
        
        public Layer_M2MLevel(ILevel M2MLevel)
        {
            this.level = M2MLevel;
            layer_Grid = new Layer_Grid(level);
            UpdateLayerGridTransform();
            maxRect = GetMaxRect();
        }

        private RectangleF GetMaxRect()
        {
            return layer_Grid.MaxRect;
        }

        public override Color MainColor
        {
            get { return partColor; }
            set { partColor = value; SpringLayerRepresentationChangedEvent(this); }
        }

        bool levelPartVisible = true;
        [CategoryAttribute("Layer")]
        public bool LevelPartVisible
        {
            get { return levelPartVisible; }
            set { levelPartVisible = value; SpringLayerRepresentationChangedEvent(this); }
        }

        Color partColor = GenerateColor.GetNextDifferentColor();
        [CategoryAttribute("Drawer")]
        public Color PartColor
        {
            get { return partColor; }
            set { partColor = value; SpringLayerRepresentationChangedEvent(this); }
        }        

        bool levelGridVisible = true;
        [CategoryAttribute("Layer")]
        public bool LevelGridVisible
        {
            get { return levelGridVisible; }
            set { levelGridVisible = value; SpringLayerRepresentationChangedEvent(this); }
        }

        Color gridColor = Color.FromArgb(192, 192, 192);
        [CategoryAttribute("Drawer")]
        public Color GridColor
        {
            get { return gridColor; }
            set { gridColor = value; SpringLayerRepresentationChangedEvent(this); }
        }

        int gridLineWidth = 1;
        [CategoryAttribute("Drawer")]
        public int GridLineWidth
        {
            get { return gridLineWidth; }
            set { gridLineWidth = value; SpringLayerRepresentationChangedEvent(this); }
        }

        int alpha = 30;
        [CategoryAttribute("Drawer")]
        public int Alpha
        {
            get { return alpha; }
            set
            {
                if (value > 255)
                {
                    alpha = 255;
                }
                else if (value < 0)
                {
                    alpha = 0;
                }
                else
                {
                    alpha = value;
                }

                SpringLayerRepresentationChangedEvent(this);
            }
        }

        public override void Draw(Graphics graphics)
        {
            if (Visible)
            {
                if (LevelGridVisible)
                {
                    //画格子
                    if (transformIsChanged)
                    {
                        UpdateLayerGridTransform();
                    }
                    layer_Grid.Visible = true;
                    layer_Grid.GridLine.LineColor = Color.FromArgb(Alpha + (255 - Alpha) / 6, gridColor);
                    layer_Grid.GridLine.LineWidth = GridLineWidth;
                    layer_Grid.Draw(graphics);
                }

                if (LevelPartVisible)
                {
                    Brush brush = new SolidBrush(Color.FromArgb(alpha, partColor));
                    //画有效分块
                    for (int i = 0; i < level.GetUnitNumInHeight(); i++)
                    {
                        for (int j = 0; j < level.GetUnitNumInWidth(); j++)
                        {
                            if (level.GetPartRefByPartIndex(j, i) != null)
                            {
                                graphics.FillRectangle(brush, GetPartScreenRectangleInSpecificLevel(level, j, i));
                            }
                        }
                    }
                }
            }
        }

        private void UpdateLayerGridTransform()
        {
            layer_Grid.ScreenScaleX = this.ScreenScaleX;
            layer_Grid.ScreenScaleY = this.ScreenScaleY;
            layer_Grid.ScreenTranslationX = this.ScreenTranslationX;
            layer_Grid.ScreenTranslationY = this.ScreenTranslationY;
        }
    }

    public class Layer_M2MStructure : Layer
    {
        IM2MStructure m2mStructure;

        Layer_M2MLevel[] layerList;

        public Layer_M2MStructure(IM2MStructure m2mStructure)
        {
            this.m2mStructure = m2mStructure;
            layerList = new Layer_M2MLevel[m2mStructure.GetLevelNum()];

            for (int i = 0; i < layerList.Length;i++)
            {
                layerList[i] = new Layer_M2MLevel(m2mStructure.GetLevel(i));
            }

            UpdateLayerListTransform();

            maxRect = layerList[0].MaxRect;
        }

        public override Color MainColor
        {
            get { return partColor; }
            set { partColor = value; SpringLayerRepresentationChangedEvent(this); }
        }

        bool levelPartVisible = true;
        [CategoryAttribute("Layer")]
        public bool LevelPartVisible
        {
            get { return levelPartVisible; }
            set
            {
                if (levelPartVisible != value)
                {
                    levelPartVisible = value;
                    foreach (Layer_M2MLevel layer in layerList)
                    {
                        layer.LevelPartVisible = value;
                    }
                    SpringLayerRepresentationChangedEvent(this);
                }
            }
        }

        Color partColor = Color.FromArgb(128, 128, 128);
        [CategoryAttribute("Drawer")]
        public Color PartColor
        {
            get { return partColor; }
            set { partColor = value; SpringLayerRepresentationChangedEvent(this); }
        }

        bool levelGridVisible = true;
        [CategoryAttribute("Layer")]
        public bool LevelGridVisible
        {
            get { return levelGridVisible; }
            set
            {
                if (levelGridVisible != value)
                {
                    levelGridVisible = value;
                    foreach (Layer_M2MLevel layer in layerList)
                    {
                        layer.LevelGridVisible = value;
                    }
                    SpringLayerRepresentationChangedEvent(this);
                }
            }
        }

        Color gridColor = Color.FromArgb(192, 192, 192);
        [CategoryAttribute("Drawer")]
        public Color GridColor
        {
            get { return gridColor; }
            set { gridColor = value; SpringLayerRepresentationChangedEvent(this); }
        }

        int alpha = 30;
        [CategoryAttribute("Drawer")]
        public int Alpha
        {
            get { return alpha; }
            set
            {
                if (value > 255)
                {
                    alpha = 255;
                }
                else if (value < 0)
                {
                    alpha = 0;
                }
                else
                {
                    alpha = value;
                }

                SpringLayerRepresentationChangedEvent(this);
            }
        }

        int showLayerSequence = -1;
        [CategoryAttribute("Layer")]
        public int ShowLayerSequence
        {
            get { return showLayerSequence; }
            set 
            {
                if (value > m2mStructure.GetLevelNum() - 1)
                {
                    showLayerSequence = m2mStructure.GetLevelNum() - 1;
                }
                else if (value < 0)
                {
                    showLayerSequence = -1;
                }
                else
                {
                    showLayerSequence = value;
                }

                SpringLayerRepresentationChangedEvent(this);
            }
        }

        public override void Draw(Graphics graphics)
        {
            if (Visible)
            {
                for (int i = 0; i < layerList.Length; i++)
                {
                    if (showLayerSequence < 0 || i == showLayerSequence)
                    {
                        layerList[i].Alpha = this.Alpha;
                        layerList[i].PartColor = this.PartColor;
                        layerList[i].GridColor = this.GridColor;
                        layerList[i].GridLineWidth = (m2mStructure.GetLevelNum() - i);
                        if (transformIsChanged)
                        {
                            UpdateLayerListTransform();
                        }
                        layerList[i].Draw(graphics);
                    }
                }
            }
        }

        private void UpdateLayerListTransform()
        {
            for (int i = 0; i < layerList.Length; i++)
            {
                layerList[i].ScreenScaleX = this.ScreenScaleX;
                layerList[i].ScreenScaleY = this.ScreenScaleY;
                layerList[i].ScreenTranslationX = this.ScreenTranslationX;
                layerList[i].ScreenTranslationY = this.ScreenTranslationY;
            }
        }
    }

    //public class Layer_M2MPart : Layer
    //{
    //    public Layer_M2MPart(ILevel level, int sequenceX, int sequenceY)
    //    {
    //        this.level = level;
    //        this.SequenceX = sequenceX;
    //        this.SequenceY = sequenceY;
    //    }

    //    public Layer_M2MPart(ILevel level, IPosition part)
    //    {
    //        this.level = level;
    //        this.SequenceX = (int)part.GetX();
    //        this.SequenceY = (int)part.GetY();
    //    }

    //    Color partColor = Color.Blue;
    //    public override Color MainColor
    //    {
    //        get { return partColor; }
    //        set { partColor = value; SpringLayerRepresentationChangedEvent(this); }
    //    }

    //    private ILevel level;
    //    [BrowsableAttribute(false)]
    //    public ILevel Level
    //    {
    //        get { return level; }
    //        set { level = value; SpringLayerRepresentationChangedEvent(this); }
    //    }

    //    private int sequenceX;
    //    [BrowsableAttribute(false)]
    //    public int SequenceX
    //    {
    //        get { return sequenceX; }
    //        set { sequenceX = value; SpringLayerRepresentationChangedEvent(this); }
    //    }

    //    private int sequenceY;
    //    [BrowsableAttribute(false)]
    //    public int SequenceY
    //    {
    //        get { return sequenceY; }
    //        set { sequenceY = value; SpringLayerRepresentationChangedEvent(this); }
    //    }

    //    public override void Draw(Graphics graphics)
    //    {
    //        if (Visible)
    //        {
    //            Brush brush = new SolidBrush(Color.FromArgb(80, partColor));
    //            graphics.FillRectangle(brush, GetPartScreenRectangleInSpecificLevel(level, sequenceX, sequenceY));
    //        }
    //    }
    //}    

    public class Layer_Rectangle : Layer
    {
        public Layer_Rectangle(float upperBound, float lowerBound, float leftBound, float rightBound)
        {
            this.upperBound = upperBound;
            this.lowerBound = lowerBound;
            this.leftBound = leftBound;
            this.rightBound = rightBound;

            maxRect = GetMaxRect();
        }

        float upperBound;
        [BrowsableAttribute(false)]
        public float UpperBound
        {
            get { return upperBound; }
            set { upperBound = value; SpringLayerRepresentationChangedEvent(this); }
        }

        float lowerBound;
        [BrowsableAttribute(false)]
        public float LowerBound
        {
            get { return lowerBound; }
            set { lowerBound = value; SpringLayerRepresentationChangedEvent(this); }
        }

        float leftBound;
        [BrowsableAttribute(false)]
        public float LeftBound
        {
            get { return leftBound; }
            set { leftBound = value; SpringLayerRepresentationChangedEvent(this); }
        }

        float rightBound;
        [BrowsableAttribute(false)]
        public float RightBound
        {
            get { return rightBound; }
            set { rightBound = value; SpringLayerRepresentationChangedEvent(this); }
        }

        Pen penLine = new Pen(Color.Blue);

        [CategoryAttribute("Drawer")]
        public float PenWidth
        {
            get { return penLine.Width; }
            set { penLine.Width = value; SpringLayerRepresentationChangedEvent(this); }
        }

        [CategoryAttribute("Drawer")]
        public System.Drawing.Drawing2D.DashStyle PenStyle
        {
            get { return penLine.DashStyle; }
            set { penLine.DashStyle = value; SpringLayerRepresentationChangedEvent(this); }
        }        

        Color lineColor = Color.Blue;
        public override Color MainColor
        {
            get { return penLine.Color; }
            set { penLine.Color = value; SpringLayerRepresentationChangedEvent(this); }
        }

        private RectangleF GetMaxRect()
        {
            return new RectangleF(leftBound, lowerBound, upperBound - lowerBound, rightBound - leftBound);
        }

        public override void Draw(Graphics graphics)
        {
            if (Visible)
            {
                graphics.DrawRectangle(penLine, GetScreenRectangleFormRealBound(
                    upperBound, lowerBound, leftBound, rightBound));
            }
        }
    }

    #endregion    
}