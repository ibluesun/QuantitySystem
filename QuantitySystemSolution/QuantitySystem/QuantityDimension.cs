using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities;

namespace QuantitySystem
{
    /// <summary>
    /// Quantity Dimension based on SI.
    /// dim Q = Lα Mβ Tγ Iδ Θε Nζ Jη
    /// </summary>
    public class QuantityDimension
    {
        #region Exponent Fields
        private int L;  //Length
        private int M;  //Mass
        private int T;  //Time
        private int I;  //electric current
        private int O;  //thermodynamic temperature
        private int N;  //amount of substance
        private int J;  //luminous intensity
        #endregion

        #region Constructors

        public QuantityDimension()
        {
        }

        public QuantityDimension(int mass, int length, int time)
        {
            M = mass;
            L = length;
            T = time;
        }

        public QuantityDimension(int mass, int length, int time, int temperature)
        {
            M = mass;
            L = length;
            T = time;
            O = temperature;
        }


        public QuantityDimension(int mass, int length, int time, int temperature, int electricalCurrent, int amountOfSubstance, int luminousIntensity)
        {
            M = mass;
            L = length;
            T = time;
            I = electricalCurrent;
            O = temperature;
            N = amountOfSubstance;
            J = luminousIntensity;
        }

        #endregion

        #region Exponent Properties
        public int LengthExponent
        {
            get
            {
                return L;
            }
        }

        public int MassExponent
        {
            get
            {
                return M;
            }
        }

        public int TimeExponent
        {
            get
            {
                return T;
            }
        }

        public int ElectricCurrentExponent
        {
            get
            {
                return I;
            }
        }

        public int TemperatureExponent
        {
            get
            {
                return O;
            }
        }

        public int AmountOfSubstanceExponent
        {
            get
            {
                return N;
            }
        }

        public int LuminousIntensityExponent
        {
            get
            {
                return J;
            }
        }
        #endregion


        #region string representaions of important values

        /// <summary>
        /// The Quality in MLT form
        /// </summary>
        public virtual string MLT
        {
            get
            {
                string mass = "M" + MassExponent.ToString(CultureInfo.InvariantCulture);
                string length = "L" + LengthExponent.ToString(CultureInfo.InvariantCulture);
                string time = "T" + TimeExponent.ToString(CultureInfo.InvariantCulture);

                return mass + length + time;
            }
        }

        #endregion


        #region Force Component

        /// <summary>
        /// returns the power of Force.
        /// </summary>
        public int ForceExponent
        {
            get
            {
                //M1L1T-2
                //take from MLT untill the M==0

                int TargetM = MassExponent;


                int TargetF = 0;


                while (TargetM > 0)
                {
                    TargetM--;
                    TargetF++;
                }

                return TargetF;
            }
        }

        /// <summary>
        /// FLT powers based on Force Length, and Time.
        /// </summary>
        public string FLT
        {
            get
            {

                //M1L1T-2
                //take from MLT untill the M==0

                int TargetM = MassExponent;

                int TargetL = LengthExponent;

                int TargetT = TimeExponent;

                int TargetF = 0;


                while (TargetM > 0)
                {
                    TargetM--;
                    TargetF++;

                    TargetL -= 1;
                    TargetT += 2;
                }


                string force = "F" + TargetF.ToString(CultureInfo.InvariantCulture);
                string length = "L" + TargetL.ToString(CultureInfo.InvariantCulture);
                string time = "T" + TargetT.ToString(CultureInfo.InvariantCulture);

                return force + length + time;

            }
        }


        #endregion

        #region Equality

        public override string ToString()
        {
            string dim = "";

            dim += "M" + M.ToString(CultureInfo.InvariantCulture);
            dim += "L" + L.ToString(CultureInfo.InvariantCulture);
            dim += "T" + T.ToString(CultureInfo.InvariantCulture);
            dim += "I" + I.ToString(CultureInfo.InvariantCulture);
            dim += "O" + O.ToString(CultureInfo.InvariantCulture);
            dim += "N" + N.ToString(CultureInfo.InvariantCulture);
            dim += "J" + J.ToString(CultureInfo.InvariantCulture);

            return dim;
        }
        public override bool Equals(object obj)
        {
            QuantityDimension QD = obj as QuantityDimension;

            if (QD != null)
            {
                if (this.ElectricCurrentExponent != QD.ElectricCurrentExponent)
                    return false;
                if (this.LengthExponent != QD.LengthExponent)
                    return false;
                if (this.LuminousIntensityExponent != QD.LuminousIntensityExponent)
                    return false;
                if (this.MassExponent != QD.MassExponent)
                    return false;
                if (this.AmountOfSubstanceExponent != QD.AmountOfSubstanceExponent)
                    return false;
                if (this.TemperatureExponent != QD.TemperatureExponent)
                    return false;
                if (this.TimeExponent != QD.TimeExponent)
                    return false;

                return true;
            }
            else
            {
                return false; 
            }
        }

        public override int GetHashCode()
        {
            int hc = ToString().GetHashCode();
            return hc;
        }
        
        #endregion

        #region Dimension Calculations

        public static QuantityDimension operator +(QuantityDimension firstDimension, QuantityDimension secondDimension)
        {
            return Add(firstDimension, secondDimension);
        }

        public static QuantityDimension Add(QuantityDimension firstDimension, QuantityDimension secondDimension)
        {
            QuantityDimension QD = new QuantityDimension(
                firstDimension.MassExponent + secondDimension.MassExponent
                , firstDimension.LengthExponent + secondDimension.LengthExponent
                , firstDimension.TimeExponent + secondDimension.TimeExponent
                , firstDimension.TemperatureExponent + secondDimension.TemperatureExponent
                , firstDimension.ElectricCurrentExponent + secondDimension.ElectricCurrentExponent
                , firstDimension.AmountOfSubstanceExponent + secondDimension.AmountOfSubstanceExponent
                , firstDimension.LuminousIntensityExponent + secondDimension.LuminousIntensityExponent
                );

            return QD;
        }

        public static QuantityDimension operator -(QuantityDimension firstDimension, QuantityDimension secondDimension)
        {
            return Subtract(firstDimension, secondDimension);
        }

        public static QuantityDimension Subtract(QuantityDimension firstDimension, QuantityDimension secondDimension)
        {
            QuantityDimension QD = new QuantityDimension(
                firstDimension.MassExponent - secondDimension.MassExponent
                , firstDimension.LengthExponent - secondDimension.LengthExponent
                , firstDimension.TimeExponent - secondDimension.TimeExponent
                , firstDimension.TemperatureExponent - secondDimension.TemperatureExponent
                , firstDimension.ElectricCurrentExponent - secondDimension.ElectricCurrentExponent
                , firstDimension.AmountOfSubstanceExponent - secondDimension.AmountOfSubstanceExponent
                , firstDimension.LuminousIntensityExponent - secondDimension.LuminousIntensityExponent
                );

            return QD;
        }

        public static QuantityDimension operator *(QuantityDimension dimension, int exponent)
        {
            return Multiply(dimension, exponent);
        }

        public static QuantityDimension Multiply(QuantityDimension dimension, int exponent)
        {
            QuantityDimension QD = new QuantityDimension(
                dimension.MassExponent * exponent
                , dimension.LengthExponent * exponent
                , dimension.TimeExponent * exponent
                , dimension.TemperatureExponent * exponent
                , dimension.ElectricCurrentExponent * exponent
                , dimension.AmountOfSubstanceExponent * exponent
                , dimension.LuminousIntensityExponent * exponent
                );

            return QD;

        }
        #endregion
    
    
    
        #region Quantities Preparation
        private static List<Type> CurrentQuantityTypes = new List<Type>();

        /// <summary>
        /// holding Dimension -> Quantity instance  to be clonned.
        /// </summary>
        private static Dictionary<QuantityDimension, AnyQuantity> CurrentQuantitiesDictionary = new Dictionary<QuantityDimension,AnyQuantity>();

        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<Type, QuantityDimension> CurrentDimensionsDictionary = new Dictionary<Type,QuantityDimension>();


        /// <summary>
        /// Cash all quantities.
        /// </summary>
        static QuantityDimension()
        {

            Assembly CurrentAssembly = Assembly.GetExecutingAssembly();
            Type[] types = CurrentAssembly.GetTypes();

            var QuantityTypes = from QuantityType in types
                                where QuantityType.IsSubclassOf(typeof(BaseQuantity))
                                select QuantityType;

            CurrentQuantityTypes.AddRange(QuantityTypes);

            foreach (Type QuantityType in CurrentQuantityTypes)
            {
                //cach the quantities that is not abstract types

                if (QuantityType.IsAbstract == false && QuantityType.Name != "DerivedQuantity")
                {
                    //make sure not to include Dimensionless quantities due to they are F0L0T0
                    if (QuantityType.BaseType.Name != "DimensionlessQuantity")
                    {
                        //store dimension as key and Quantity instance.
                        AnyQuantity Quantity = (AnyQuantity)Activator.CreateInstance(QuantityType);
                        CurrentQuantitiesDictionary.Add(Quantity.Dimension, Quantity);

                        //store quantity type as key dimension as value.

                        CurrentDimensionsDictionary.Add(QuantityType, Quantity.Dimension);

                    }
                }
            }
            
        }
        #endregion


        #region Quantity utilities


        public static AnyQuantity QuantityFrom(QuantityDimension dimension)
        {
            //the method should return the target Quantity
            //with respect to the mlt string.

            try
            {
                return (AnyQuantity)CurrentQuantitiesDictionary[dimension].Clone();
            }
            catch
            {

                throw new QuantityNotFoundException();
            }
        }


        public static QuantityDimension DimensionFrom(Type quantityType)
        {
            try
            {
                return CurrentDimensionsDictionary[quantityType];
            }
            catch
            {
                throw new DimensionNotFoundException("UnExpected Exception. TypeOf<" + quantityType.ToString() + "> have no dimension associate with it");

            }
        }


        /// <summary>
        /// Extract the Mass power from MLT string.
        /// </summary>
        /// <param name="mlt"></param>
        /// <returns></returns>
        private static int ExponentOfMass(string mlt)
        {
            int m_index = 0;
            int l_index = mlt.IndexOf('L');
            int m = int.Parse(mlt.Substring(m_index + 1, l_index - m_index - 1), CultureInfo.InvariantCulture);

            return m;

        }


        /// <summary>
        /// Extract the Length Power from MLT string.
        /// </summary>
        /// <param name="MLT"></param>
        /// <returns></returns>
        private static int ExponentOfLength(string mlt)
        {
            int l_index = mlt.IndexOf('L');
            int t_index = mlt.IndexOf('T');

            int l = int.Parse(mlt.Substring(l_index + 1, t_index - l_index - 1), CultureInfo.InvariantCulture);

            return l;

        }

        /// <summary>
        /// Extract the Time Power from MLT string.
        /// </summary>
        /// <param name="MLT"></param>
        /// <returns></returns>
        private static int ExponentOfTime(string mlt)
        {
            int t_index = mlt.IndexOf('T');
            int t = int.Parse(mlt.Substring(t_index + 1, mlt.Length - t_index - 1), CultureInfo.InvariantCulture);

            return t;


        }

        public static QuantityDimension ParseMLT(string mlt)
        {

            int m = ExponentOfMass(mlt);
            int l = ExponentOfLength(mlt);
            int t = ExponentOfTime(mlt);

            return new QuantityDimension(m, l, t);
        }


        #endregion

    }
}
