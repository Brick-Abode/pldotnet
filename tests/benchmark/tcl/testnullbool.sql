CREATE OR REPLACE FUNCTION returnNullBoolTcl() RETURNS boolean AS $$
return_null;
$$ LANGUAGE pltcl;
SELECT returnNullBoolTcl() is NULL;

CREATE OR REPLACE FUNCTION BooleanNullAndTcl(a boolean, b boolean) RETURNS boolean AS $$
if {[argisnull 1] || [argisnull 2]} return_null;
return [expr $1 && $2];
$$ LANGUAGE pltcl;
SELECT BooleanNullAndTcl(true, null) is NULL;
SELECT BooleanNullAndTcl(null, true) is NULL;
SELECT BooleanNullAndTcl(false, null) is NULL;
SELECT BooleanNullAndTcl(null, false) is NULL;
SELECT BooleanNullAndTcl(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullOrTcl(a boolean, b boolean) RETURNS boolean AS $$
if {[argisnull 1] || [argisnull 2]} return_null;
return [expr $1 || $2];
$$ LANGUAGE pltcl;
SELECT BooleanNullOrTcl(true, null) is NULL;
SELECT BooleanNullOrTcl(null, true) is NULL;
SELECT BooleanNullOrTcl(false, null) is NULL;
SELECT BooleanNullOrTcl(null, false) is NULL;
SELECT BooleanNullOrTcl(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullXorTcl(a boolean, b boolean) RETURNS boolean AS $$
if {[argisnull 1] || [argisnull 2]} return_null;
return [expr (!$1 && $2) || ($1 && !$2) ];
$$ LANGUAGE pltcl;
SELECT BooleanNullXorTcl(true, null) is NULL;
SELECT BooleanNullXorTcl(null, true) is NULL;
SELECT BooleanNullXorTcl(false, null) is NULL;
SELECT BooleanNullXorTcl(null, false) is NULL;
SELECT BooleanNullXorTcl(null, null) is NULL;
