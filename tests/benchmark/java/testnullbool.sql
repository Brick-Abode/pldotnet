SELECT returnNullBoolJava() is NULL;

SELECT BooleanNullAndJava(true, null) is NULL;
SELECT BooleanNullAndJava(null, true) is NULL;
SELECT BooleanNullAndJava(false, null) is false;
SELECT BooleanNullAndJava(null, false) is NULL;
SELECT BooleanNullAndJava(null, null) is NULL;

SELECT BooleanNullOrJava(true, null) is true;
SELECT BooleanNullOrJava(null, true) is NULL;
SELECT BooleanNullOrJava(false, null) is NULL;
SELECT BooleanNullOrJava(null, false) is NULL;
SELECT BooleanNullOrJava(null, null) is NULL;

SELECT BooleanNullXorJava(true, null) is NULL;
SELECT BooleanNullXorJava(null, true) is NULL;
SELECT BooleanNullXorJava(false, null) is NULL;
SELECT BooleanNullXorJava(null, false) is NULL;
SELECT BooleanNullXorJava(null, null) is NULL;
