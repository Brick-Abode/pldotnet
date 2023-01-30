package com.example.proj;

import java.sql.SQLException;
import org.postgresql.pljava.annotation.Function;

import java.util.logging.Logger;

public class TestRecursive {
    @Function
    public static int fibbbJava(int n) throws SQLException {
        if (n <= 1) {
            return n;
        }
        return fibbbJava(n - 1) + fibbbJava(n - 2);
    }

    @Function
    public static int factJava(int n) throws SQLException {
        int ret = 1;
        if (n <= 1) {
            return ret;
        } else {
            return n*factJava(n - 1);
        }
    }

    @Function
    public static double naturalJava(double n) throws SQLException {
        if (n < 0) {
            return 0;
        } else if (n == 1) {
            return 1;
        } else {
            return naturalJava(n - 1);
        }
    }

}