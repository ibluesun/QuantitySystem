# Qs Types illustration

# Scalars
u = 4      #Numerical Quantity Scalar
v = $x     #Symbolic Quantity Scalar
w = $y     #Symbolic Quantity Scalar
wv = (v-w)
wvu = wv^u



# Vectors
a = {4 3 2}
b = {3 2 7}
c = {5 3 1}

# Matrices
m1 = [a; b; c]
m2 = [b; a; c]

m3 = [a b; b c; c a]

ms = [$x 0 0; 0 $y 0; 0 0 $z]    #matrix contains symbolic quantities


# Tensors from zero rank to 3rd rank
T0 = <| 3 |>
T1 = <| a b |>
T2 = <| a ;b ; c|>
T3 = <| a;b;c | b;c;a | c;a;b |>



 
