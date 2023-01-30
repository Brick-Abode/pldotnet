SELECT maxSmallIntJava() = integer '32767';

SELECT sum2SmallIntJava(CAST(100 AS smallint), CAST(101 AS smallint)) = smallint '201';

SELECT maxIntegerJava() = integer '2147483647';

SELECT returnIntJava() = integer '10';

SELECT inc2ToIntJava(8) = integer '10';

SELECT sum3IntegerJava(3,2,1) = integer '6';

SELECT sum4IntegerJava(4,3,2,1) = integer '10';

SELECT sum2IntegerJava(32770, 100) = bigint '32870';

SELECT maxBigIntJava() = bigint '9223372036854775807';

SELECT sum2BigIntJava(9223372036854775707, 100) = bigint '9223372036854775807';

SELECT mixedBigIntJava(32767,  2147483647, 100) = bigint '2147516514';

SELECT mixedIntJava(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100) = integer '65634';
