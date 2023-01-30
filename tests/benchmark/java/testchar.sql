SELECT retVarCharJava('Rafael') = varchar 'Rafael Cabral';

SELECT retConcatVarCharJava('João ', 'da Silva') = varchar 'João da Silva';

SELECT retConcatTextJava('João ', 'da Silva') =  varchar 'Hello João da Silva!';

SELECT retVarCharTextJava('Homer Jay ', 'Simpson') = varchar 'Hello Homer Jay Simpson!';

SELECT retCharJava('R') =  character 'R';

SELECT retConcatLettersJava('R', 'C') = varchar 'RC';

SELECT retConcatCharsJava('H.', 'Simpson') = 'H.Simpson';

SELECT retConcatVarCharsJava('H.', 'Simpson') = 'H.Simpson';
SELECT retConcatVarCharsJava('H. あ', 'Simpson') = 'H. あSimpson';

SELECT retNonRegularEncodingJava('漢字') = varchar '漢字';
SELECT retNonRegularEncodingJava('ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ') = varchar 'ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ';
SELECT retNonRegularEncodingJava('ŁĄŻĘĆŃŚŹ') = varchar 'ŁĄŻĘĆŃŚŹ';
SELECT retNonRegularEncodingJava('Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.')
    = varchar 'Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.';
