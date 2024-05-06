package com.example.proj;

import java.math.BigDecimal;
import org.postgresql.pljava.annotation.Function;

public class TestArray {
    @Function
    public static int sumArrayIntJava(int[] a) {
        return a[0] + a[1] + a[2];
    }

    @Function
    public static BigDecimal sumArrayNumJava(BigDecimal[] a) {
        return a[0].add(a[1]).add(a[2]);
    }

    @Function
    public static String sumArrayTextJava(String[] a) {
        return a[0] + a[1] + a[2] + a[3];
    }
}
