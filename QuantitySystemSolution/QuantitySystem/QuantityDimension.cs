using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities;

using QuantitySystem.DimensionDescriptors;
using QuantitySystem.Quantities.DimensionlessQuantities;
using System.Text.RegularExpressions;

namespace QuantitySystem
{
    /// <summary>
    /// Quantity Dimension.
    /// dim Q = Lα Mβ Tγ Iδ Θε Nζ Jη
    /// </summary>
    public class QuantityDimension
    {

        #region Dimension Physical Properties
        public MassDescriptor Mass { get; set; }
        public LengthDescriptor Length { get; set; }
        public TimeDescriptor Time { get; set; }
        public ElectricCurrentDescriptor ElectricCurrent { get; set; }
        public TemperatureDescriptor Temperature { get; set; }
        public AmountOfSubstanceDescriptor AmountOfSubstance { get; set; }
        public LuminousIntensityDescriptor LuminousIntensity { get; set; }
        #endregion

        #region Dimension Extra Properties
        public CurrencyDescriptor Currency { get; set; }
        #endregion

        #region Constructors

        public QuantityDimension()
        {
        }

        /// <summary>
        /// basic constructor for MLT Dimensions.
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="length"></param>
        /// <param name="time"></param>
        public QuantityDimension(float mass, float length, float time)
        {
            Mass = new MassDescriptor(mass);
            Length = new LengthDescriptor(length, 0);
            Time = new TimeDescriptor(time);

        }


        public QuantityDimension(float mass, float length, float time, float temperature)
        {
            Mass = new MassDescriptor(mass);
            Length = new LengthDescriptor(length, 0);
            Time = new TimeDescriptor(time);
            Temperature = new TemperatureDescriptor(temperature);

        }


        public QuantityDimension(float mass, float length, float time, float temperature, float electricalCurrent, float amountOfSubstance, float luminousIntensity)
        {
            Mass = new MassDescriptor(mass);
            Length = new LengthDescriptor(length, 0);
            Time = new TimeDescriptor(time);
            Temperature = new TemperatureDescriptor(temperature);

            ElectricCurrent = new ElectricCurrentDescriptor( electricalCurrent);
            AmountOfSubstance = new AmountOfSubstanceDescriptor(amountOfSubstance);
            LuminousIntensity = new LuminousIntensityDescriptor(luminousIntensity);
        }


        #endregion


        #region string representaions of important values

        /// <summary>
        /// The Quality in MLT form
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "MLT")]
        public virtual string MLT
        {
            get
            {
                string mass = "M" + Mass.Exponent.ToString(CultureInfo.InvariantCulture);
                string length = "L" + Length.Exponent.ToString(CultureInfo.InvariantCulture);
                string time = "T" + Time.Exponent.ToString(CultureInfo.InvariantCulture);

                return mass + length + time;
            }
        }

        #endregion


        #region Force Component

        /// <summary>
        /// returns the power of Force.
        /// </summary>
        public float ForceExponent
        {
            get
            {
                //M1L1T-2
                //take from MLT untill the M==0

                float TargetM = Mass.Exponent;


                float TargetF = 0;


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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "FLT")]
        public string FLT
        {
            get
            {

                //M1L1T-2
                //take from MLT untill the M==0

                float TargetM = Mass.Exponent;

                float TargetL = Length.Exponent;

                float TargetT = Time.Exponent;

                float TargetF = 0;


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

            dim += "M" + Mass.Exponent.ToString(CultureInfo.InvariantCulture);


            string lll = "";

            if (Length.PolarExponent != 0)
            {
                dim +=
                    string.Format("L{0}(RL{1}PL{2})",
                    Length.Exponent.ToString(CultureInfo.InvariantCulture),
                    Length.RegularExponent.ToString(CultureInfo.InvariantCulture),
                    Length.PolarExponent.ToString(CultureInfo.InvariantCulture)
                    );
            }
            else
            {
                dim += string.Format("L{0}",
                    Length.Exponent.ToString(CultureInfo.InvariantCulture));
            }
            
            dim += "T" + Time.Exponent.ToString(CultureInfo.InvariantCulture);
            dim += "I" + ElectricCurrent.Exponent.ToString(CultureInfo.InvariantCulture);
            dim += "O" + Temperature.Exponent.ToString(CultureInfo.InvariantCulture);
            dim += "N" + AmountOfSubstance.Exponent.ToString(CultureInfo.InvariantCulture);
            dim += "J" + LuminousIntensity.Exponent.ToString(CultureInfo.InvariantCulture);

            dim += "$" + Currency.Exponent.ToString(CultureInfo.InvariantCulture);

            return dim;
        }

        public override bool Equals(object obj)
        {
            QuantityDimension QD = obj as QuantityDimension;


            if (QD != null)
            {
                if (!this.ElectricCurrent.Equals(QD.ElectricCurrent))
                    return false;

                if (!this.Length.Equals(QD.Length))
                    return false;
                
                if (!this.LuminousIntensity.Equals( QD.LuminousIntensity))
                    return false;

                if (!this.Mass.Equals(QD.Mass))
                    return false;

                if (!this.AmountOfSubstance.Equals(QD.AmountOfSubstance))
                    return false;

                if (!this.Temperature.Equals(QD.Temperature))
                    return false;

                if (!this.Time.Equals(QD.Time))
                    return false;

                if (!this.Currency.Equals(QD.Currency))
                    return false;
                return true;
            }
            else
            {
                return false; 
            }
        }

        /// <summary>
        /// Equality here based on first level of exponent validation.
        /// Which means length explicitly is compared to the total dimension value not on radius and normal length values.
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public bool IsEqual(QuantityDimension dimension)
        {
            if (this.ElectricCurrent.Exponent != dimension.ElectricCurrent.Exponent)
                return false;

            if (this.Length.Exponent != dimension.Length.Exponent)
                return false;

            if (this.LuminousIntensity.Exponent != dimension.LuminousIntensity.Exponent)
                return false;

            if (this.Mass.Exponent != dimension.Mass.Exponent)
                return false;

            if (this.AmountOfSubstance.Exponent != dimension.AmountOfSubstance.Exponent)
                return false;

            if (this.Temperature.Exponent != dimension.Temperature.Exponent)
                return false;

            if (this.Time.Exponent != dimension.Time.Exponent)
                return false;

            if (this.Currency.Exponent != dimension.Currency.Exponent)
                return false;

            return true;
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

            QuantityDimension QD = new QuantityDimension();

            QD.Mass = firstDimension.Mass.Add(secondDimension.Mass);
            QD.Length = firstDimension.Length.Add(secondDimension.Length);
            QD.Time =  firstDimension.Time.Add( secondDimension.Time);
            QD.Temperature = firstDimension.Temperature.Add(secondDimension.Temperature);
            QD.ElectricCurrent = firstDimension.ElectricCurrent.Add(secondDimension.ElectricCurrent);
            QD.AmountOfSubstance = firstDimension.AmountOfSubstance.Add( secondDimension.AmountOfSubstance);
            QD.LuminousIntensity = firstDimension.LuminousIntensity.Add(secondDimension.LuminousIntensity);

            QD.Currency = firstDimension.Currency.Add(secondDimension.Currency);

            return QD;
        }

        public static QuantityDimension operator -(QuantityDimension firstDimension, QuantityDimension secondDimension)
        {
            return Subtract(firstDimension, secondDimension);
        }

        public static QuantityDimension Subtract(QuantityDimension firstDimension, QuantityDimension secondDimension)
        {
            QuantityDimension QD = new QuantityDimension();

            QD.Mass = firstDimension.Mass.Subtract(secondDimension.Mass);
            QD.Length = firstDimension.Length.Subtract(secondDimension.Length);
            QD.Time = firstDimension.Time.Subtract(secondDimension.Time);
            QD.Temperature = firstDimension.Temperature.Subtract(secondDimension.Temperature);
            QD.ElectricCurrent = firstDimension.ElectricCurrent.Subtract(secondDimension.ElectricCurrent);
            QD.AmountOfSubstance = firstDimension.AmountOfSubstance.Subtract(secondDimension.AmountOfSubstance);
            QD.LuminousIntensity = firstDimension.LuminousIntensity.Subtract(secondDimension.LuminousIntensity);

            QD.Currency = firstDimension.Currency.Subtract(secondDimension.Currency);

            return QD;
        }

        public static QuantityDimension operator *(QuantityDimension dimension, float exponent)
        {
            return Multiply(dimension, exponent);
        }

        public static QuantityDimension Multiply(QuantityDimension dimension, float exponent)
        {
            QuantityDimension QD = new QuantityDimension();


            QD.Mass = dimension.Mass.Multiply(exponent);
            QD.Length = dimension.Length.Multiply(exponent);
            QD.Time = dimension.Time.Multiply(exponent);
            QD.Temperature = dimension.Temperature.Multiply(exponent);
            QD.ElectricCurrent = dimension.ElectricCurrent.Multiply(exponent);
            QD.AmountOfSubstance = dimension.AmountOfSubstance.Multiply(exponent);
            QD.LuminousIntensity = dimension.LuminousIntensity.Multiply(exponent);
            QD.Currency = dimension.Currency.Multiply(exponent);

            return QD;

        }
        #endregion
    
    
    
        #region Quantities Preparation
        private static List<Type> CurrentQuantityTypes = new List<Type>();


        /// <summary>
        /// holding Dimension -> Quantity instance  to be clonned.
        /// </summary>
        static Dictionary<QuantityDimension, Type> CurrentQuantitiesDictionary = new Dictionary<QuantityDimension, Type>();

        /// <summary>
        /// Quantity Name -> Quantity Type  as all quantity names are unique
        /// </summary>
        static Dictionary<string, Type> CurrentQuantitiesNamesDictionary = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// holding Quantity -> Dimension
        /// </summary>
        static Dictionary<Type, QuantityDimension> CurrentDimensionsDictionary = new Dictionary<Type, QuantityDimension>();

        public static Type[] AllQuantitiesTypes
        {
            get
            {
                return CurrentQuantitiesNamesDictionary.Values.ToArray();
            }
        }

        public static string[] AllQuantitiesNames
        {
            get
            {
                return CurrentQuantitiesNamesDictionary.Keys.ToArray();
            }
        }

       

        /// <summary>
        /// Cache all quantities with their Dimensions.
        /// </summary>
        static QuantityDimension()
        {

            Assembly CurrentAssembly = Assembly.GetExecutingAssembly();
            Type[] types = CurrentAssembly.GetTypes();

            var QuantityTypes = from QuantityType in types
                                where QuantityType.IsSubclassOf(typeof(BaseQuantity))
                                select QuantityType;

            CurrentQuantityTypes.AddRange(QuantityTypes);

            //storing the quantity types with thier dimensions

            foreach (Type QuantityType in CurrentQuantityTypes)
            {
                //cach the quantities that is not abstract types

                if (QuantityType.IsAbstract == false && QuantityType != typeof(DerivedQuantity<>))
                {
                    //make sure not to include Dimensionless quantities due to they are F0L0T0
                    if (QuantityType.BaseType.Name != typeof(DimensionlessQuantity<>).Name)
                    {

                        //store dimension as key and Quantity Type .
                        
                        //create AnyQuantity<Object>  Object container used just for instantiation
                        AnyQuantity<Object> Quantity = (AnyQuantity<Object>)Activator.CreateInstance(QuantityType.MakeGenericType(typeof(object)));

                        //store the Dimension and the corresponding Type;
                        CurrentQuantitiesDictionary.Add(Quantity.Dimension, QuantityType);

                        //store quantity type as key and corresponding dimension as value.
                        CurrentDimensionsDictionary.Add(QuantityType, Quantity.Dimension);

                        //store the quantity name and type with insensitive names
                        CurrentQuantitiesNamesDictionary.Add(QuantityType.Name.Substring(0, QuantityType.Name.Length - 2), QuantityType);
                    
                    }
                }
            }

            
            
        }
        #endregion


        #region Quantity utilities

        /// <summary>
        /// Returns the quantity type or typeof(DerivedQuantity&lt;&gt;) without throwing exception
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public static Type GetQuantityTypeFrom(QuantityDimension dimension)
        {
            Type qType;
            if (CurrentQuantitiesDictionary.TryGetValue(dimension, out qType))
                return qType;
            else
                return typeof(DerivedQuantity<>);
        }

        /// <summary>
        /// Get the corresponding typed quantity in the framework of this dimension
        /// Throws QuantityNotFounException when there is corresponding one.
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public static Type QuantityTypeFrom(QuantityDimension dimension)
        {
            try
            {
                Type QuantityType = CurrentQuantitiesDictionary[dimension];


                return QuantityType;

            }
            catch (KeyNotFoundException ex)
            {
                QuantityNotFoundException qnfe = new QuantityNotFoundException("Couldn't Find the quantity dimension in the dimensions Hash Key", ex);

                throw qnfe;
            }
        }


        /// <summary>
        /// Gets the quantity type from the name and throws QuantityNotFounException if not found.
        /// </summary>
        /// <param name="quantityName"></param>
        /// <returns></returns>
        public static Type QuantityTypeFrom(string quantityName)
        {
            try
            {
                Type QuantityType = CurrentQuantitiesNamesDictionary[quantityName];


                return QuantityType;

            }
            catch (KeyNotFoundException ex)
            {
                QuantityNotFoundException qnfe = new QuantityNotFoundException("Couldn't Find the quantity dimension in the dimensions Hash Key", ex);

                throw qnfe;
            }

        }


        static Dictionary<Type, object> QuantitiesCached = new Dictionary<Type, object>();

        /// <summary>
        /// Returns Strongly typed Any Quantity From the dimension based on the discovered quantities discovered when 
        /// framework initiated.
        /// Throws <see cref="QuantityNotFoundException"/> when quantity is not found.
        /// </summary>
        /// <typeparam name="T">The value container of the Quantity</typeparam>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public static AnyQuantity<T> QuantityFrom<T>(QuantityDimension dimension)
        {
            lock (QuantitiesCached)
            {
                Type QuantityType = QuantityTypeFrom(dimension);

                //the quantity type now is without container type we should generate it

                Type QuantityWithContainerType = QuantityType.MakeGenericType(typeof(T));

                object j;
                if (QuantitiesCached.TryGetValue(QuantityWithContainerType, out j))
                {
                    return (AnyQuantity<T>)((AnyQuantity<T>)j).Clone();
                }
                else
                {
                    j = Activator.CreateInstance(QuantityWithContainerType);
                    QuantitiesCached.Add(QuantityWithContainerType, j);
                    return (AnyQuantity<T>)((AnyQuantity<T>)j).Clone();
                }
            }

        
        }


        /// <summary>
        /// Returns the quantity dimenstion based on the quantity type.
        /// </summary>
        /// <param name="quantityType"></param>
        /// <returns></returns>
        public static QuantityDimension DimensionFrom(Type quantityType)
        {
            if (!quantityType.IsGenericTypeDefinition)
            {
                //the passed type is AnyQuantity<object> for example
                //I want to get the type without type parameters AnyQuantity<>
                quantityType = quantityType.GetGenericTypeDefinition();
            }

            try
            {
                return CurrentDimensionsDictionary[quantityType];
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                //if key not found and quantityType is really Quantity
                //then return dimensionless Quantity

                if (quantityType.BaseType.GetGenericTypeDefinition() == typeof(DimensionlessQuantity<>))
                    return QuantityDimension.Dimensionless;
                else
                    throw new DimensionNotFoundException("UnExpected Exception. TypeOf<" + quantityType.ToString() + "> have no dimension associate with it", ex);

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "mlt"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "MLT")]
        public static QuantityDimension ParseMLT(string mlt)
        {

            int m = ExponentOfMass(mlt);
            int l = ExponentOfLength(mlt);
            int t = ExponentOfTime(mlt);

            return new QuantityDimension(m, l, t);
        }

        public static QuantityDimension Parse(string dimension)
        {

            dimension = dimension.Trim();

            // M L T I O N J   C
            List<float> exps = new List<float>();
            if (char.IsNumber(dimension[0]))
            {
                // parsing started with numbers which will be most probably 
                //   on the format of numbers with spaces
                string[] dims = dimension.Split(' ');
                foreach (var dim in dims)
                {
                    var dimtrimmed = dim.Trim();

                    if (!string.IsNullOrEmpty(dimtrimmed))
                    {
                        exps.Add(float.Parse(dimtrimmed));
                    }
                }

                float M = exps.Count > 0 ? exps[0] : 0;
                float L = exps.Count > 1 ? exps[1] : 0;
                float T = exps.Count > 2 ? exps[2] : 0;
                float I = exps.Count > 3 ? exps[3] : 0;
                float O = exps.Count > 4 ? exps[4] : 0;
                float N = exps.Count > 5 ? exps[5] : 0;
                float J = exps.Count > 6 ? exps[6] : 0;
                float C = exps.Count > 7 ? exps[7] : 0;

                var qd = new QuantityDimension(M, L, T, I, O, N, J);
                qd.Currency = new CurrencyDescriptor(C);

                return qd;

            }
            else
            {
                // the format is based on letter and number

                var dumber  = dimension.ToUpperInvariant();

                var mts = Regex.Matches(dumber, @"(([MmLlTtIiOoNnJjCc])(\-*[0-9]+))+?");

                Dictionary<char,float> edps = new Dictionary<char,float>();

                foreach(Match m in mts)
                {
                    edps.Add(m.Groups[2].Value[0], float.Parse(m.Groups[3].Value));
                }

                if (!edps.ContainsKey('M')) edps['M'] = 0;
                if (!edps.ContainsKey('L')) edps['L'] = 0;
                if (!edps.ContainsKey('T')) edps['T'] = 0;
                if (!edps.ContainsKey('I')) edps['I'] = 0;
                if (!edps.ContainsKey('O')) edps['O'] = 0;
                if (!edps.ContainsKey('N')) edps['N'] = 0;
                if (!edps.ContainsKey('J')) edps['J'] = 0;
                if (!edps.ContainsKey('C')) edps['C'] = 0;

                var qd = new QuantityDimension(edps['M']
                    , edps['L'], edps['T'], edps['I'], edps['O']
                    , edps['N'], edps['J']);

                qd.Currency = new CurrencyDescriptor(edps['C']);

                return qd;
            }

        }


        #endregion


        public static QuantityDimension Dimensionless
        {
            get
            {
                return new QuantityDimension();
            }
        }

        public bool IsDimensionless
        {
            get
            {
                if (
                    Mass.Exponent == 0 && Length.Exponent == 0 && Time.Exponent == 0 &&
                    ElectricCurrent.Exponent == 0 && Temperature.Exponent == 0 && AmountOfSubstance.Exponent == 0 &&
                    LuminousIntensity.Exponent == 0  && Currency.Exponent == 0
                    )
                    return true;
                else
                    return false;
            }
        }



        public QuantityDimension Invert()
        {
            QuantityDimension qd = new QuantityDimension();

            qd.Mass = Mass.Invert();
            qd.Length = Length.Invert();
            qd.Time = Time.Invert();
            qd.ElectricCurrent = ElectricCurrent.Invert();
            qd.Temperature = Temperature.Invert();
            qd.AmountOfSubstance = AmountOfSubstance.Invert();
            qd.LuminousIntensity = LuminousIntensity.Invert();
            qd.Currency = Currency.Invert();
            return qd;

        }

        public QuantityDimension(QuantityDimension dimension)
        {
            Mass = dimension.Mass;
            Length = dimension.Length;
            Time = dimension.Time;
            ElectricCurrent = dimension.ElectricCurrent;
            Temperature = dimension.Temperature;
            AmountOfSubstance = dimension.AmountOfSubstance;
            LuminousIntensity = dimension.LuminousIntensity;

            Currency = dimension.Currency;

        }

    }
}
