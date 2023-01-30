package com.example.proj;

import java.sql.SQLException;
import org.postgresql.pljava.annotation.Function;

import java.util.logging.Logger;

public class TestFloats {
    @Function
    public static float returnRealJava() throws SQLException {
        return 1.50055f;
    }

    @Function
    public static float sumRealJava(float a, float b) throws SQLException {
        return a + b;
    }

    @Function
    public static double returnDoubleJava() throws SQLException {
        return 11.0050000000005;
    }

    @Function
    public static double sumDoubleJava(double a, double b) throws SQLException {
        return a + b;
    }
}