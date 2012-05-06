# Metric Tensor Calculation
# Written by Ahmed Sadek as Illustration for Quantity System
# 2012-05-06  First
# -----------------------------------------------------------


c = { ${r*cos(theta)} ${r*sin(theta)} $z}

@fc = c       # Function from vector with symbolic quantities

#p1 = fc(3,4,5)
#p2 = fc(theta=2, r=10, z=3)
#p3 = fc(theta = $angle, r=$radius, z=$height)

g1 = c|$r;
g2 = c|$theta;
g3 = c|$z;

"Covariant metric tensor"
g_covariant = [g1.g1 g1.g2 g1.g3; g2.g1 g2.g2 g2.g3; g3.g1 g3.g2 g3.g3]
""			
""
"Convtravariant metric tensor"
g_contravariant = g_covariant ^ -1

@gcov = g_covariant;
@gcont = g_contravariant;

vcov = {3 2 10};
"Starting with Covariant Vector: " + vcov

calced_vcont = gcov(r = vcov[0], theta = vcov[1]) * vcov;
"Calculated Contravariant Vector: " + calced_vcont

calced_vcov = gcont(r = calced_vcont[0], theta = calced_vcont[1]) * calced_vcont;
"Calculated Covariant Vector: " + calced_vcov

"Test Sucessful: "+calced_vcov when calced_vcov == vcov otherwise "Test Failed!"



