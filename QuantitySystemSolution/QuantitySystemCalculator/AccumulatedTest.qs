
# the file contains many tests that increases with every version of quantity system to test variety of things

f(x) = x^2

R:f(x,y) = x^2+y^3

R:G:f(z) = z^4

@w = @f|$x + R:@f|$y - R:G:@f

c = w(1,2,3)


# Testing the deep properties in nested objects through properties and methods
p = new Planet;

p!properties[0]

