SELECT returnRealJava() = real '1.50055';

SELECT sumRealJava(1.50055, 1.50054) = real '3.00109'; -- 3.00109

SELECT returnDoubleJava() = double precision '11.0050000000005';

SELECT sumDoubleJava(10.5000000000055, 10.5000000000054) = double precision  '21.0000000000109'; -- 21.0000000000109
