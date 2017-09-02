# Sequence


Sequence [http://en.wikipedia.org/wiki/Sequence](http://en.wikipedia.org/wiki/Sequence) is a mathematical expression that define a list of successive elements.

The most famous sequences in the mathematics is Fibonacci Sequence [http://en.wikipedia.org/wiki/Fibonacci_number](http://en.wikipedia.org/wiki/Fibonacci_number)

When adding the sequence elements from specific element to specific element or infinity we obtain the Series  [http://en.wikipedia.org/wiki/Series_(mathematics)](http://en.wikipedia.org/wiki/Series_(mathematics))

Also we can multiply the elements of the sequence

for expressing the sequence a new operator has been added to support sequence declaration

Sequence in Qs is having two modes  Parameterless or with parameters

the most easy Sequence declaration can be like the following

**{"S[]() ..> 10; 30; 40; 20; 10"}**

these elements are stored at indices from 0 to 4

you can access the sequence by index number

**{"S[3](3)"}**

but what if you accessed an index outside the defined range ??
**{"S[1000](1000)"}**   #will return 10 as a value

Fibonacci sequence:
**{"fib[n](n) ..> 0; 1; fib[n-1](n-1) + fib[n-2](n-2)			#fibonaccy sequence  "}**

as you can see Sequence elements can be called recursively
Sequence elements are also cached for better performance  {only parameterless ones}


## Sequence is infinite
Sequence is INFINITE  and accessing any element in the sequence must return a value.

you  can set individual sequence elements 
**{"S[10](10) = 20"}**
**{"S[20](20) = 300"}**
and as you can imagine any empty elements will get its values from the nearest lowest element for it.

This is a valid form also
**{"S[h:30](h_30) = (30/h^2)+300"}**

no matter what the index of declaration differ from the original, the calculator is happily understand what you mean.

## Access the sequence
* Access by index
	* **{"mil[10](10)"}**
	* **{"Series access mil[10++30](10++30)     # to get the series value"}**
	* **{"mil[10****40](10____40)                            # ok its a multiplications for the elements."}**
	* **{"mil[0!!50](0!!50)                              #guess what :D  yes it is for Mean Value,  and soon I'll add the standard deviation."}**

## Sequence with Parameters
For many serieses like the exponential series Exp(), SIN(), COS(),  we need to pass arguments

for example to define the COS(x) function from scratch we can write

**{"cos[n](n)(x) ..> ((-1)^n**x^(2**n))/(2**n)!			#cos sequence "}**

and to encapsulate into a function
**{"cos(x) = cos[0++40](0++40)(x)							#cos function"}**

Note: I have found that calling to the 40 element was very satisfactory for the double size I use in the implementation 



