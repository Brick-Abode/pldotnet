SELECT returnNullIntJava() is NULL;

SELECT returnNullSmallIntJava() is NULL;

SELECT returnNullBigIntJava() is NULL;

SELECT sumNullArgIntJava(null,null) is NULL;
SELECT sumNullArgIntJava(null,3) is NULL;
SELECT sumNullArgIntJava(3,null) is NULL;
SELECT sumNullArgIntJava(3,3) = integer '6';

SELECT sumNullArgSmallIntJava(null,null) is NULL;
SELECT sumNullArgSmallIntJava(null,CAST(101 AS smallint)) is NULL;
SELECT sumNullArgSmallIntJava(CAST(101 AS smallint),null) is NULL;
SELECT sumNullArgSmallIntJava(CAST(101 AS smallint),CAST(101 AS smallint)) = smallint '202';

SELECT sumNullArgBigIntJava(null,null) is NULL;
SELECT sumNullArgBigIntJava(null,100) is NULL;
SELECT sumNullArgBigIntJava(9223372036854775707,null) is NULL;
SELECT sumNullArgBigIntJava(9223372036854775707,100) = bigint '9223372036854775807';

SELECT checkedSumNullArgIntJava(null,null) is NULL;
SELECT checkedSumNullArgIntJava(null,3) is NULL;
SELECT checkedSumNullArgIntJava(3,null) is NULL;
SELECT checkedSumNullArgIntJava(3,3) = integer '6';

SELECT checkedSumNullArgSmallIntJava(null,null) is NULL;
SELECT checkedSumNullArgSmallIntJava(null,CAST(133 AS smallint)) is NULL;
SELECT checkedSumNullArgSmallIntJava(CAST(133 AS smallint),null) is NULL;
SELECT checkedSumNullArgSmallIntJava(CAST(133 AS smallint),CAST(133 AS smallint)) = smallint '266';

SELECT checkedSumNullArgBigIntJava(null,null) is NULL;
SELECT checkedSumNullArgBigIntJava(null,100) is NULL;
SELECT checkedSumNullArgBigIntJava(9223372036854775707,null) is NULL;
SELECT checkedSumNullArgBigIntJava(9223372036854775707,100) = bigint '9223372036854775807';

SELECT checkedSumNullArgMixedJava(null,null,null) is NULL;
SELECT checkedSumNullArgMixedJava(null,CAST(1313 as smallint),null) is NULL;
SELECT checkedSumNullArgMixedJava(1313,null,null) is NULL;
SELECT checkedSumNullArgMixedJava(null,null,3) is NULL;
SELECT checkedSumNullArgMixedJava(1313,CAST(1313 as smallint), 1313) = smallint '3939';
