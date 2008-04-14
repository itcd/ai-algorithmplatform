
using System;
using Troschuetz.Random;

namespace Troschuetz.Random
{
  
    ///<summary>
    ///</summary>
    public class LevyDistribution : Distribution
    {
        #region instance fields
        /// <summary>
        /// Gets or sets the parameter alpha of cauchy distributed random numbers which is used for their generation.
        /// </summary>
        /// <remarks>Call <see cref="IsValidAlpha"/> to determine whether a value is valid and therefor assignable.</remarks>
        public double Alpha
        {
            get
            {
                return this.alpha;
            }
            set
            {
                if (this.IsValidAlpha(value))
                {
                    this.alpha = value;
                }
            }
        }

        /// <summary>
        /// Stores the parametera alpha of cauchy distributed random numbers which is used for their generation.
        /// </summary>
        private double alpha;

        ///<summary>
        ///</summary>
        public double SCALE
        {
            get
            {
                return this.Scale;
            }
            set
            {
                if (this.IsValidGamma(value))
                {
                    this.Scale = value;
                }
            }
        }

        /// <summary>
        /// Stores the parameter gamma which is used for generation of cauchy distributed random numbers.
        /// </summary>
        private double Scale;
        #endregion

        #region construction
        /// <summary>
        /// Initializes a new instance of the <see cref="CauchyDistribution"/> class, using a 
        ///   <see cref="StandardGenerator"/> as underlying random number generator. 
        /// </summary>
        public LevyDistribution()
            : this(new StandardGenerator())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CauchyDistribution"/> class, using the specified 
        ///   <see cref="Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">A <see cref="Generator"/> object.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="generator"/> is NULL (<see langword="Nothing"/> in Visual Basic).
        /// </exception>
        public LevyDistribution(Generator generator)
            : base(generator)
        {
            this.alpha = 1.0;
            this.Scale = 1.0;
        }
        #endregion

        #region instance methods
        /// <summary>
        /// Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/>.</returns>
        public bool IsValidAlpha(double value)
        {
            return true;
        }

        /// <summary>
        /// Determines whether the specified value is valid for parameter <!--<see cref="Gamma"/>.-->
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        /// <see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidGamma(double value)
        {
            return value > 0.0;
        }
        #endregion

        #region overridden Distribution members
        /// <summary>
        /// Gets the minimum possible value of cauchy distributed random numbers.
        /// </summary>
        public override double Minimum
        {
            get
            {
                return double.MinValue;
            }
        }

        /// <summary>
        /// Gets the maximum possible value of cauchy distributed random numbers.
        /// </summary>
        public override double Maximum
        {
            get
            {
                return double.MaxValue;
            }
        }

        /// <summary>
        /// Gets the mean value of cauchy distributed random numbers.
        /// </summary>
        public override double Mean
        {
            get
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// Gets the median of cauchy distributed random numbers.
        /// </summary>
        public override double Median
        {
            get
            {
                return this.alpha;
            }
        }

        /// <summary>
        /// Gets the variance of cauchy distributed random numbers.
        /// </summary>
        public override double Variance
        {
            get
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// Gets the mode of cauchy distributed random numbers.
        /// </summary>
        public override double[] Mode
        {
            get
            {
                return new double[] { this.alpha };
            }
        }

        /// <summary>
        /// Returns a cauchy distributed floating point random number.
        /// </summary>
        /// <returns>A cauchy distributed double-precision floating point number.</returns>
        public override double NextDouble()
        {
            double u, v, t, s;
            

            u = Math.PI * (this.Generator.NextDouble() - 0.5);

            if (alpha == 1)		/* cauchy case */
            {
                t = Math.Tan(u);
                return Scale * t;
            }

            do
            {
                ExponentialDistribution edis = new ExponentialDistribution();
                v = edis.NextDouble();
            }
            while (v == 0);

            if (alpha == 2)             /* gaussian case */
            {
                t = 2 *Math.Sin(u) *Math.Sqrt(v);
                return Scale * t;
            }

            /* general case */

            t =Math.Sin(alpha * u) /Math.Pow(Math.Cos(u), 1 / alpha);
            s = Math.Pow(Math.Cos((1 - alpha) * u) / v, (1 - alpha) / alpha);

            return Scale * t * s;

        }
        #endregion
    }
}