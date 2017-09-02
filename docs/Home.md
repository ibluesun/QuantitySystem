# Quantity System Framework and Calculator


**Project Description**

Quantity System is an attempt to make a framework to support scientific calculations through units conversions and quantities predictions.

Based on Dimensional approach not Units approach.

The first to differentiate between Torque, and Work, Angle, and Solid Angle.

## **Live demo** for the calculator can be found in [http://quantitysystem.org](http://quantitysystem.org)
## **Project Blog** [http://quantitysystem.wordpress.com](http://quantitysystem.wordpress.com)

## Requirements
    Microsoft .net Framework 4.0 [http://www.microsoft.com/downloads/en/details.aspx?displaylang=en&FamilyID=0a391abd-25c1-4fc0-919f-b21f31ab88b7](http://www.microsoft.com/downloads/en/details.aspx?displaylang=en&FamilyID=0a391abd-25c1-4fc0-919f-b21f31ab88b7)

# What's New in 1.2

## New Scalar Types
* Complex Scalar       C{3 5}
	* C{4 3}<L> +  C{5 2}<m^3> **etc.**
* Quaternion Scalar    H{3 4 2 1}
## Breaking change.
* absolute and magnitude operators  |x|  and ||x||  has been converted into {"_|x|_ and _||x||_ in favor of introducing the differential operator '|'"}
## Function Derivation
* @F|$x   to derive for x
* @|$x * @F   another way to derive the function
* @F|$x|$y^2   partial derivation for x then y two times.
* @|$x|$y^2 * @F  equivalent way of derivation.  
## Function as Quantity
* fv = {@f @g @i(z)}   is the ability to store the function into vectors, matrices, and tensors to make further calculations 
* function can be summed, subtracted, and all other basic operation which in turn are calculated based on symbolic calculations
* converting between Symbolic Quantity and Function vice versa
	* {"@s = $x+$y^2  will produce  s(x,y) = x+y^2  which in turn can be called as s(3,4)"}
* functions can be added also to symbolic variables.
	* f(x) =x     @f=@f+$x   
## New Operators
* Nable operator \/  
	* Gradient which means \/* vector field  can be achieved when multiplying \/*@F
* Range operator ..
	* To create vectors from..to   v = 1..10



## New Units
* DIP : Device Independent Pixel  (Thanks for Starnuto for the addition)  the unit is used extensively in WPF applications.


**All the new specifications will be documented in a separate spec doc.**

I hope by this release that I illustrate some of the power beyond blending symbolic calculations and numerical calculations.

**Thank you all for your continued support**


# What's New in 1.1.9.964

The appearence of symbolic quantities is the most significant event in this release.

## Breaking Changes
### Tensor Syntax
{" '<|' and '|>' with separation with vertical bar '|' "}
{"Example: T3 = <|2 3; 4 6|4 3; 1 3|>"}   this is like the hyper cube.

for more order tensors you can embed this syntax in a recursive fashion
{"Example: T4 = <| <|2 3; 4 6|4 3; 1 3|> | <|2 3; 4 6|4 3; 1 3|> |>"}    which gives you a tensor from the fourth order

**Not All tensor algebra is implemented because I am not a mathematical guy (however if you have a good tensor book that go beyound the 2nd order one, please let me know)**

## New Features
### Symbolic Quantity
{"Example: 
     a=$x+$y
     b=$x-$y
     a*b will produce x^2+y^2"}
Yes as you guessed to express a symbolic quantity you prefix the letter or letters with dollar sign '$'
so **$x, $r, $ROI, $12r, $20** are correct syntaxes are right

**Note: the symbol is also quantity and it is merged into the scalar type so you won't feel the difference in usage**

And it is available in all the mathematical types i.e. Matrix
{" m = [$x $y $z; 4 3 2; $u $v $w]($x-$y-$z;-4-3-2;-$u-$v-$w) 
   determinant can be obtained by two vertical bars aruond m
   |M| will produce DimensionlessQuantity: 3**x**w-2**x**v+2**y**u-4**y**w+4**z**v-3**z**u <1>"} 

{"and yet these symbols are quantities, so $x<ft>+40<m> is valid expression but $v<m/s> + $ws<m/s^2> is not valid because of dimensionality differnence."}

all of this calculation has been made by my beloved library [http://symbolicalgebra.codeplex.com](http://symbolicalgebra.codeplex.com)

**Expect soon to implement the derivatives also**

### Shifting Operators '<<' '>>' 
Like C family I've added shifting operators
**Vectro** {3 4 5 6 7} >> 2 will be shifted to the right 2 elements
**Matrices** also have a shifting 
**Text** is having shifting
"Ahmed" >> 1 will produce "hmedA"

### DLR update
I am now offically using the DLR version 1.0 from microsoft 

### Sequence updates
* Standard Deviation '!%' 
	* {"s[1 !% 10](1-!%-10)  produce standard deviation for the sequence elements from 1 to 10"}
* Enhanced the sequence declaration to include the starting and ending index variables during evaluation
	* {"g[k=m->n](k=m-_n)(x) ..> n - k*x  #m is the starting index, and n is the ending index which can be used in the items evaluation."}
* Getting range from parameterized sequence return vector or matrix or tensor based on what you have written in symbolic quantities
	*   {"g[n](n)(x) ..> x^n"}
	*   {"g[0..3](0..3) result in { 1 x x^2 x^3}"}

### Vector
* {"Access element from vector by indexing  V[0](0)"}

### Matrix 
* {"Access vector from Matrix by indexing M[0](0)  or scalar by M[0,0](0,0)"}
* Determinant is now expanded for more than 3x3 elements (and I borrowed the code, so it is not the fastest as usuall :) )
* Outer product for matrix added '(*)' (Kronecker product)



### Tensor 
* Elements access by indexing maximum to the order of the tensor
	* {"T4 is 4th order tensor if you specify T4[0](0) then you access the third order tensor in it"}
	* {"T4[0,0](0,0) you access the matrix"}
	* {"T4[0,0,0](0,0,0) is vector"}
	* {"finally T4[0,0,0,0](0,0,0,0)  you got your scalar there"}
* Addition and Subtraction of Tensor for any order
* Tensor Order 1 * Tensor Order 1 = Tensor Order 2 (Dyadic operation)  { looks like the outer product of two vectors ;) try the two please}

### New Quantities
* SpecificEnergy
* MolarDensity
* MolarSpecificHeat
* MolarVolume

### New Units
* <MBTU> Million British Thermal Units
* <MMBTU> Million Million British Thermal Units
* <cal>   Calorie = 4.184 <J>
* <lbcal>  Pound Calorie

## Fixes
* {"Fixed a destructive error: 5^-3 was evaluating in bad behaviour because i was translating -3 to (-1*3) then the priority of '^' precede the priority (NOW FIXED)"}
* A lot of fixes has been done **please check the check-in history from previous release**

----

# What's New in 1.1.9.93

## General
### Parsing and Tokenization enhancments in many areas.
for example {"
     a = 1<kg/s>

     a = a + (a + ( a + a + (((((((((((((((((((a + 1<mg/s>))))))))))))))))))) + 0.1<slug/h>)+ 0.2<g/s>)
"}

was taking a huge amount of time, but now it works flawesly.

### Fixed a prefix calculation error    
    <km.km.m> == hm^3   thanks for luc who pointed out the issue in issues.


## Text support 
* Qs> a = "Hello There"
* Qs> Windows:MessageBox(a)
* {" Qs> Windows:MessageBox( "3+4 = " + (3+4)); "}

## Tensor Parsing support   
{" Qs> T = <<5 3; 3 1|2 3; 2 1| 5 0 ; 2 1>>  # 3rd rank "}


## Tensor product '(*)'
now you can multiply two vectors with tensor product which result in matrix 
Qs> {3 4 2} (*) {5 6 3}
    QsMatrix:
        15<1>         18<1>          9<1>
        20<1>         24<1>         12<1>
        10<1>         12<1>          6<1>

## Functions now can be treated like values
Qs> u(x)  = x^2
Qs> v(y) = y/3
Qs> w = @u + @v  #this will return w(x,y) function
also by using '@' operator you can select a specific function with parameter  '@f(y)' for example

----


[Image:Qs SnapShot-win-1.1.9.91.png)(Image_Qs-SnapShot-win-1.1.9.91.png)

# What's New in 1.1.9.91

## Breaking Changes
### 1- Breaking Change: Named arguments now by equal sign only '='  no more ':='  it was very long :) 
* f(x) = x
* f(y) = y^2
* f(z) = z^3
* vv = f(z=f(y=f(x=math:sin(val=10))))

### 2- No referencing for global variables in functions
* 2- f(x) = 3+u  will result in QsParameterNotFoundException: u. due to u is not in the parameter list.

## New Features
### Sequence can be evaluated to a  function when called without parameters 
This will allow you to generate functions on the fly by one element or by series.

* {"Qs> g[n](n)(x) ..> x^n"}
* {"Qs> g[4](4)"}  return _(x)=x^4
* {"Qs> fg = g[0++10](0++10) # will return function fg(x) = _(x) = x^0+x^1+... "}

## Bugs fixing and organizing for code.
* Operation of Vector - Scalar  Fixed  :(  



# What's New in 1.1.9.85

### 1- Namespaces support
### 2- Calling function by argument names.
### 3- Function overloading with argument names.
### 3- Extending the core functionality with extra modules   
##### try Qs>Music:PlayMidi(RaisingFight.mid)
### 4- A new conditional statement that fit in one line for many conditions
##### f(x) = x when x<=10 otherwise x^2 when x<=20 otherwise x^3




# Quantity System Framework 1.1.8

[Image:Qs SnapShot-win-1.1.8.png)(Image_Qs-SnapShot-win-1.1.8.png)

# What's New in 1.1.8

## **Qs Specifications 1.1.8** [Qs Specifications.docx](Home_Qs Specifications.docx)

This version has gone a core change in its design  to support vectors and matrices.
In future I am intending to support Tensors also.

Don't forget to download the specifications :)

please enjoy this fine alpha release :)


QsScalar class that is wrapping the AnyQuantity<double> class
QsScalar is inherited from QsValue
Three Classes inherited from QsValue

 1) QsScalar  
 2) QsVector
 3) QsMatrix
 
 
Later the Qs will also support Tensor with rank 3 {any suggestions is appreciated}

please notice that the matrix calculations is not the best algorithm available.

## Fixes
- Metric prefixes overflow fix
  when you form a unit <ms.s> milli second * second 
  the result is <s^2>  and an overflow occur due to the milli prefix
  this milli factor goes to the value part
  so if you entered 2<ms.s> the result will be 0.002<s^2>
  
  however the framework is always try to get the best expression it can have.
  for example if you make 2<Gm.mm> giga Metre * milli Metre the result will be 2<km^2>
  
  but if there were no corresponding prefix for the overflow then the prefix is removed 
  and overflow flag in the unit is raised, and the quantity value is altered as shown 
  in the first example.


## New features:
{"
- Absolute Operator   |n|  where n is QsScalar or QsMatrix

- Vector support:
	  a= {2 3 4}  or a={2,3,5}
	  
- Dot product for vectors   New opertaor
	  {2,3,4} . { 5 6 3}
	  
- Crosss product for vectors of 3 components  use 'x' letter       New Operator
	  {3<m> 2<A> 4<N>} x {2<ft> 2<mA> 3<pdl>}
	  
- Vector Norm ||_||:
	    ||{3 5 3 2 1}|| or  ||a||

- Factorial of Vector gets all the factorial of its components

- calling a function with vector as input result in vector as a result.
  

- Matrix support 
	m1 = [2 3 4; 3 2 1; {5 4 3}](2-3-4;-3-2-1;-{5-4-3})
	
- Matrix multiplication

- Determinant for matrix using determinant operator
	| a | 
	| [ 2 3 4; 3 2 1; 5 2 3](-2-3-4;-3-2-1;-5-2-3) |
	only for 2x2 and 3x3 matrices.
	LU decomposition methods comes later.
	
- Sequence new range operator '..' i.e. 's[n..m](n..m)':
   returns vector if elements are scalars.
   returns matrix if elements are vectors.	

- Enhancments in parsing and exceptions handling.
"}

## New Quantities:
{"
- Stiffness						<N/m>
- RotationalStiffness			Torque / Angle
- Momentum						Mass * Velocity
- MassMomentOfInteria			I   <kg.m!^2>   like the regular mass
- AngularMomentum				Mass Moment Of Interia * Angular Velocity
"}

## New Units:

- Cubit <cubit>					Ancient Length unit = 45.72<cm> based on google calculator.


----

# What's New in 1.1.7
Proudly I introduce to you 1.1.7 ALPHA version of the calculator
I had fun and hard time playing with microsoft DLR to reach this status.
I am introducing a new [Sequence](Sequence) concept in my calculator for expressing the Series in math.

## New Features

### 1) Sequence Declaration

The [Sequence](Sequence) Concept is to be able to define a sequence that hold many values for you.

for example to define the exponential [http://en.wikipedia.org/wiki/Exponential_function](http://en.wikipedia.org/wiki/Exponential_function) function you can declare a sequence

   **{"Qs> e[n](n)(x)..>x^n/n!"}**

then get the value as a Series  **{"Qs> e[0++40](0++40)(1)"}**


### 2) Functions as arguments

This is known feature as a higher order function in programming.

   **{"Qs> v(i,j) = i(j)"}**

The feature is very important especially if you want to make a numerical differential function

   **{"Qs> df(f,x,h) = (f(x+h)-f(x))/h"}**


# What's New in 1.1.4
## New Features
* Factorial has been enhanced to include real positive numbers i.e. writing 3.5<m>! = 11.6317283996629 <m^3.5> 
* Heavy optimizations for calculations - now calling the same conversion from unit to unit is cached during the session.
* Enhacing Qs hosting to be used dynamically inside any .NET applications using DLR hosting APIs.	
	* ScriptRuntime sr = Qs.Qs.CreateRuntime();
	* var QsScope = sr.ExecuteFile("test.qs");            
	* var QsEngine = sr.GetEngine("Qs");
	* var a = QsEngine.Execute("a=5<m>", QsScope) as AnyQuantity<double>;



## New Quantities
* Specific Weight quantity added  it is the result of greek letter gamma = Density * Acceleration 

## New Units
* Gravitional System namespace under metric namespace added three new units  (finally I figured out what it is really mean :) )
* <hyl> for mass
* <gf> Gramforce  
* <p>  small letter for Pond unit  which equal to <gf>   Remeber <P> in capital is Poise
* <grain> renamed to <gr>  Grain unit
* <pdl> Poundal unit  added = <lbm.ft/s^2>




# What's New in 1.1
## New Features
* Differentiate between Radius Length and Normal Length on the unit entering bases so now you can easily create Torque quantity with units like 40<N.m!>. The '!' unit modifier tell the calculator that we need Radius Length instead of normal length. This is valid for all length units like ft, in, yd, etc.

* Factorial Operator '!' added for calculating factorials for integer numbers {working on real numbers later}. 
	* so this is a valid calculation 5<kg>! / 4<L>!
	* Functions also is getting used from factorials
	*   Qs> C(n,k) = n!/(k!*(n-k)!)      {"#Combinications"}
	*   Qs> P(n,r) = n!/(n-r)!             {"#Permutations"}

* Enhancing the raise to power to include fraction exponents. this means you can get the square root to the quantity.
	* {"100<kg>^0.5 = 10<kg^0.5>"}
	* more over this is a valid operation: {"20<m> + 30<ft^2>^0.5 + 10<ha>^0.5"}
	* however you can't create quantities based on fraction unit exponents  {I don't think we need this for now}

* Optimization for the whole calculator for more speed specifically for the double storage type and for the parsing code.

* Functions now can refer to global variables in the scope if they were declared. given that there is no parameter with the same name.
	* Qs> a=10
	* Qs> f(x) = x/a

* DLR engine is separated now from the calculator (this way you can call the engine and host the calculator in your .net language)

## New Quantities
* VolumeFlowRate

## New Units
* <cfm>  Cubic feet per minute
* <gpm> Gallon per minute




# What's New in 1.0.9
## New Features
* Power operator has been add  x^2   -- Cautious {you only can raise to dimensionless numbers}
* More toward DLR support: the program is entirely run under QsHost which subclass DLR ConsoleHost
* Functions appear for the first time
{
	Declaration is normal like ordinary math 
		Qs> f(x) = x^2+64
		Qs> f(x,y) = x/y
	Called
		Qs> 10 - f(f(40,20))  + 20
			DimensionlessQuantity: -38 <1>	
}
				
## Fixes
* Correct the <1> unit in parsing which affected <1/kg> and all units
* Calculating numbers that start with sign - or +   (fixed)  so this is valid  a=-2 ++324--392 +50<m/ft> 
* Adding Angle Quantity to Dimensionless Quantities is valid now
{
		-----------------------------------------
		Qs> 1 + 10 <rad>
			DimensionlessQuantity: 11 <1>
		Qs> 1 + 10 <deg>
			DimensionlessQuantity: 1.17453292519943 <1>
		Qs> 10<deg> + 1
			Angle: 67.2957795130823 deg
		Qs> 10<rad> + 1
			Angle: 11 rad   
		-------------------------------------------------
}
and with units taken into consideration also.

## New Quantities
	* Molar Mass       g/mol
	* Heat Capacity    J/K
## New units
	* lbmol  pound mol

# What's New in 1.0.8

* Colors changed :)
* SpecificVolume Quantity Added
* Calculations of operators priority fixed and Parenthesis support added.

# What's New in 1.0.7

## New Features
	* Add support for parsing units with exponent like <cm^2>
	* Add support for simple mixed units with one '/' division character <kg.m/s^2> gives Force Quantity
	* The first use the DLR Expressions instead of Linq Expressions 
		{this is a strategic transfer to support more features in the future}	
		{also because built in linq expressions make errors in mono under linux :( }
			
	
## New Units
	* psi  for pound per square inch  for pressure
	
## Many Fixes
	* Fixing of Metric Prefix multiplication and division  (although need testing).
	* conversion from metric units had an error because exponent wasn't taken in consideration.

# What's New in 1.0.6

## New Quantities 
	* Electric Charge
	* Electromotive Force
	* Capacitance
	* Electric Conductance
	* Electric Resistance
	* Magnetic Flux
	* Magnetic Flux Density
	* Inductance
	* Luminous Flux
	* Illuminance
	* Catalytic Activity  
	
## New Units
	* Coulomb 'C'
	* Farad   'F'
	* Volt    'V'
	* Siemens  'S'
	* Ohm      'ohm'
	* Weber    'Wb'
	* Tesla    'T'
	* Henry    'H'
	* Lumen    'lm'
	* Lux      'lx'
	* Katal    'kat'  => mol/s
	
	
## Astronomical Units:
	*   AstronomicalUnit symbol changed from 'AU' to 'au'
	*   Light Year Length added                      'ly'
	*   Parsec unit  length                          'pc'  with metric support
	*   Solar Mass unit                              'Mo'
	*  Julian Year unit							 'a'		
	
* Are unit of area       symnol changed from 'a' to 'are'

* u unified mass of atom  added 'u' = 'Da'
* Dalton transfered to Natural Units
	
## Shared Unit System added 
	The shared system contains units that exists in all unit systems
	* Second 's'  transfered to Shared unit system 
		
## Misc Unit System added
	the system is for units that don't have exact unit system and needs to be there untill we know its unit systems

* Angle units
	* Arc Degree unit added  'deg'  ==>this is simple the angle degrees
	* Arc Minute unit        'arcmin'
	* Arc Second unit        'arcsec'
	* Milli Arc Second       'mas'
	* Gradian  unit          'grad'
	* Revolution  unit       'r'   whole one cycle = 360<deg>
			
	* Angstrom Transfered to Misc namespace
		
* Time units
	* Minute unit            'min'
	* Hour   unit            'h'
	* Day    unit            'd'	
		
* Volume units
	* Cubit Centimetre       'cc'