package com.example.proj;

import java.math.BigDecimal;
import java.sql.SQLException;
import org.postgresql.pljava.annotation.Function;

import java.util.logging.Logger;

public class TestNumeric {
    @Function
    public static BigDecimal get_sumJava(BigDecimal a, BigDecimal b) throws SQLException {
        return a.add(b);
    }

    @Function
    public static BigDecimal getbigNumJava(BigDecimal a) throws SQLException {
        return a;
    }
}