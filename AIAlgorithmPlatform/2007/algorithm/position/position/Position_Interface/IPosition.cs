using System;
using System.Collections.Generic;

namespace Position_Interface
{
    public interface IPosition
    {
        float GetX();
        float GetY();
    }

    [Serializable]
    public class Position_Point : IPosition_Edit
    {
        public Position_Point()
        { }

        public Position_Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        float x;
        float y;

        public float GetX()
        {
            return x;
        }

        public float GetY()
        {
            return y;
        }

        public void SetX(float x)
        {
            this.x = x;
        }

        public void SetY(float y)
        {
            this.y = y;
        }
    }

    public class Transform
    {
        private float scaleX = 1;
        public float ScaleX
        {
            get { return scaleX; }
            set
            {
                scaleX = value;
            }
        }

        private float scaleY = 1;
        public float ScaleY
        {
            get { return scaleY; }
            set
            {
                scaleY = value;
            }
        }

        private float translationX = 0;
        public float TranslationX
        {
            get { return translationX; }
            set
            {
                translationX = value;
            }
        }

        private float translationY = 0;
        public float TranslationY
        {
            get { return translationY; }
            set
            {
                translationY = value;
            }
        }
    }

    public class Position_Transformed : IPosition
    {
        protected IPosition position;
        protected Transform transform;
        public Position_Transformed(IPosition position, Transform transform)
        {
            this.position = position;
            this.transform = transform;
        }

        public float GetX()
        {
            return position.GetX() * transform.ScaleX + transform.TranslationX;
        }

        public float GetY()
        {
            return position.GetY() * transform.ScaleY + transform.TranslationY;
        }
    }

    public class Position_Edit_Transformed : Position_Transformed, IPosition_Edit
    {
        public Position_Edit_Transformed(IPosition_Edit position, Transform transform)
            : base(position, transform)
        {
        }

        public void SetX(float x)
        {
            ((IPosition_Edit)(this.position)).SetX((x - transform.TranslationX) / transform.ScaleX);
        }

        public void SetY(float y)
        {
            ((IPosition_Edit)(this.position)).SetY((y - transform.TranslationY) / transform.ScaleY);
        }
    }
}