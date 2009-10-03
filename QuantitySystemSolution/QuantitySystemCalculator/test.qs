﻿C(n,k) = n!/(k!*(n-k)!)			#Combinications
P(n,r) = n!/(n-r)!				#Permutations

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
a[] ..> 2; 3; 5; 7; 9; 11
b[] ..> 4; 6; 8; 10; 12; 14
c[] ..> b[]/a[]

# Function as argument into another function
g(x) = x^2

c(x,y) = x(y/2)
d(x,y) = x+y
v(l1,l2,h) = l1(l2,h)

# v called with function d and 3,4

v(d,3,4)

# v called with c as a function in first parameter, however c also needs function parameter
# that was passed to v in the next parameter.

v(c,sin,8)



#Please Accept my deepest regards 
#	Ahmed Sadek



