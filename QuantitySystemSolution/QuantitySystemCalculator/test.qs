Cr(n,k) = n!/(k!*(n-k)!)			#Combinications
Pr(n,r) = n!/(n-r)!					#Permutations

# Radius length based functions
#r must be radius length add ! to the length units i.e 2<mm!> or 4<in!>
CF(r) = 0<m> + 2 * 3.14159265<rad> * r						#Circumference 
CA(r) = 0<m^2> + 3.14159265<rad^2> * r^2					#Circle Area
SA(r) = 0<m^2> + 4 * 3.14159265<rad^2> * r^2				#Sphere Area
SV(r) = 0<m^3> + ((4/3) * 3.14159265<rad^3> * r^3)			#Sphere Volume

e[n](x) ..> x^n/n!				#Exponential sequence
e(x) = e[0++50](x)				#Exponential function

sin[n](x) ..> ((-1)^n*x^(2*n+1))/(2*n+1)!		#Sin sequence
sin(x) = sin[0++50](x)							#Sin function

cos[n](x) ..> ((-1)^n*x^(2*n))/(2*n)!			#cos sequence
cos(x) = cos[0++50](x)							#cos function

fib[n] ..> 0; 1; fib[n-1] + fib[n-2]			#fibonaccy sequence

# Pi calculation http://en.wikipedia.org/wiki/Chudnovsky_algorithm
pi_Ch[k] ..> ((-1)^k * (6*k)! * (13591409 + 545140134*k))/((3*k)! * (k!)^3 * 640320^(3*k+(3/2)))
pi() = 1/(12*pi_Ch[0++20])


#Normal sequences 
fun:a[] ..> 2; 3; 5; 7; 9; 11
fun:b[] ..> 4; 6; 8; 10; 12; 14
fun:c[] ..> fun:b[] / fun:a[]

# Function as argument into another function
fun:g(x) = x^2
fun:g(y) = fun:g(x:=y) + y^3
fun:g(z) = fun:g(y:=z) + z^4

fun:c(x,y) = x(y/2)
fun:d(x,y) = x+y
fun:v(l1,l2,h) = l1(l2,h)

# v called with function d and 3,4

fun:a = fun:v(fun:d,3,4);

# v called with c as a function in first parameter, however c also needs function parameter
# that was passed to v in the next parameter.

fun:b = fun:v(fun:c,sin,8);

fun:a + fun:b


#test the condition statement 

wave:tri(t) = 1-|t| when |t|<1 otherwise 0						#http://en.wikipedia.org/wiki/Triangular_function 
																# remember that |x| is absolute when x is scalar and determinant if x is n x n matrix
										
wave:saw(t) = t - math:floor(t)									#http://en.wikipedia.org/wiki/Sawtooth_wave

math:sgn(x) = -1 when x<0 otherwise 0 when x==0 otherwise 1 	#http://en.wikipedia.org/wiki/Sign_function

wave:Square(x) = math:sgn(math:sin(x))  						#http://en.wikipedia.org/wiki/Square_wave

MsgBox(text) = Windows:MessageBox(text)

MsgBox(Loading Completed.);


#Please Accept my deepest regards 
#	Ahmed Sadek




