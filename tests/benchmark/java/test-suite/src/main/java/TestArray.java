package com.example.proj;

import org.postgresql.pljava.annotation.Function;

public class TestArray {
    @Function
    public static int sumArrayIntJava(int[] a) {
        return a[0] + a[1] + a[2];
    }

    @Function
    public static double sumArrayNumJava(double[] a) {
        return a[0] + a[1] + a[2];
    }

    @Function
    public static String sumArrayTextJava(String[] a) {
        return a[0] + a[1] + a[2] + a[3];
    }
}
