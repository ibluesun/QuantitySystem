# for some calculations :)

Cr(n,k) = n!/(k!*(n-k)!)			#Combinications  for all probabilities without repetion
Pr(n,r) = n!/(n-r)!					#Permutations    for all probabilities including repetion



# This file contains test case for overloaded functions by arguments names.
# The first function declared is the default function of its number of variables.
#    any extra functions will require named arguments f(x:=val)  to be called.
# The language will try to map the best function for your call.

# This remind me that I have to make a comparison syntax so I can make unit tests.

# 4 Cr 2 = 6    possible formation without repettion.

f(x,y) = x + y

f(u,v) = u - v

f(x,u) = x * u

f(x,v) = x / v

f(u,y) = (u + y) / (u*y)

f(y,v) = (v * y) / (y+v)


#emulate the thermodynamic functions
#      T = temperature [K]
#      P = pressure [kPa]
#      D = density [mol/L]
#      E = internal energy [J/mol]
#      H = enthalpy [J/mol]
#      S = entropy [J/mol-K]

Thermo:Entropy(fluid, T, P) = Windows:MessageBox(T-P)
Thermo:Entropy(fluid, T, D) = Windows:MessageBox(T-D)
Thermo:Entropy(fluid, T, E) = Windows:MessageBox(T-E)
Thermo:Entropy(fluid, T, H) = Windows:MessageBox(T-H)
Thermo:Entropy(fluid, P, D) = Windows:MessageBox(P-D)
Thermo:Entropy(fluid, P, E) = Windows:MessageBox(P-E)
Thermo:Entropy(fluid, P, H) = Windows:MessageBox(P-H)
Thermo:Entropy(fluid, D, E) = Windows:MessageBox(D-E)
Thermo:Entropy(fluid, D, H) = Windows:MessageBox(D-H)
Thermo:Entropy(fluid, E, H) = Windows:MessageBox(E-H)



