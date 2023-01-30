package com.example.proj;

import java.sql.SQLException;
import org.postgresql.pljava.annotation.Function;

import java.util.logging.Logger;

public class TestNumeric {
    @Function
    public static double get_sumJava(double a, double b) throws SQLException {
        return a + b;
    }

    @Function
    public static double getbigNumJava(double a) throws SQLException {
        return a;
    }
}