package com.example.proj;

import org.postgresql.pljava.annotation.Function;

public class TestChar {

    @Function
    public static String retVarCharJava(String fname) {
        return fname + " Cabral";
    }

    @Function
    public static String retConcatVarCharJava(String fname, String lname) {
        return fname + lname;
    }

    @Function
    public static String retConcatTextJava(String fname, String lname) {
        return "Hello " + fname + lname + "!";
    }

    @Function
    public static String retVarCharTextJava(String fname, String lname) {
        return "Hello " + fname + lname + "!";
    }

    @Function
    public static String retCharJava(String argchar) {
        return argchar;
    }

    @Function
    public static String retConcatLettersJava(String a, String b) {
        return  a + b;
    }

    @Function
    public static String retConcatCharsJava(String a, String b) {
        return a + b;
    }

    @Function
    public static String retConcatVarCharsJava(String a, String b) {
        return a + b;
    }

    @Function
    public static String retNonRegularEncodingJava(String a) {
        return a;
    }  
}
