package com.example.proj;

import org.postgresql.pljava.annotation.Function;

public class TestBool {

    @Function
    public static boolean returnBoolJava() {
        return false;
    }

    @Function
    public static boolean BooleanAndJava(boolean a, boolean b) {
        return a && b;
    }

    @Function
    public static boolean BooleanOrJava(boolean a, boolean b) {
        return a || b;
    }

    @Function
    public static boolean BooleanXorJava(boolean a, boolean b) {
        return (a && !b) || (!a && b);
    }
}
