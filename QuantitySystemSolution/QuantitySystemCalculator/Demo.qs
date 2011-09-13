root:loadlibrary("modules/qsgraphics.dll")
s = new ::QsGraphics:Screen(500, 200, 5);

u(t) = 0<m>+1<m/s>*t+2<m/s^2>*t^2;

v1(t) = 3<m>+2<m>*math:sin(2<rad/s>*t);

v2(t) = 9<m>+2<m>*math:sin(4<rad/s>*t);

s -> Circle(2<m>,3<m>,1<in>);
s-> Circle(@u,4<m>,2<m>);
s-> Circle(4<m>,@v1,1<m>);
s-> Circle(@u,@v2,0.5<m/s>);

s->Circle(@{(t)=40<m>+9<m>*Math:Sin(200<rpm>*t)},15<m>,1.5<m>);

s->Circle(@{(t)=40<m>+9<m>*Math:Sin(100<rpm>*t)},@{(t)=20<m>+9<m>*Math:Cos(100<rpm>*t)},1.5<m>);

s->updateforever();



