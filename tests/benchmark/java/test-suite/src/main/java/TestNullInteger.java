package com.example.proj;

import java.sql.SQLException;
import org.postgresql.pljava.annotation.Function;

import java.util.logging.Logger;

public class TestNullInteger {
    @Function
    public static Integer returnNullIntJava() throws SQLException {
        return null;
    }

    @Function
    public static Short returnNullSmallIntJava() throws SQLException {
        return null;
    }

    @Function
    public static Long returnNullBigIntJava() throws SQLException {
        return null;
    }

    @Function
    public static Integer sumNullArgIntJava(Integer a, Integer b) throws SQLException {
        return (a == null ? 0 : a) + (b == null ? 0 : b);
    }

    @Function
    public static Short sumNullArgSmallIntJava(Short a, Short b) throws SQLException {
        return (short)((a == null ? (short)0 : a) + (b == null ? (short)0 : b));
    }

    @Function
    public static Long sumNullArgBigIntJava(Long a, Long b) throws SQLException {
        return (long)(a == null ? (long)0 : a) + (b == null ? (long)0 : b);
    }

    @Function
    public static Integer checkedSumNullArgIntJava(Integer a, Integer b) throws SQLException {
        if (a == null  || b == null) {
            return null;
        } else {
            return a + b;
        }
    }

    @Function
    public static Short checkedSumNullArgSmallIntJava(Short a, Short b) throws SQLException {
        if (a == null  || b == null) {
            return null;
        } else {
            return (short)(a + b);
        }
    }

    @Function
    public static Long checkedSumNullArgBigIntJava(Long a, Long b) throws SQLException {
        if (a == null  || b == null) {
            return null;
        } else {
            return (long)(a + b);
        }
    }

    @Function
    public static Long checkedSumNullArgMixedJava(Integer a, Short b, Long c) throws SQLException {
        if (a == null  || b == null || c == null) {
            return null;
        } else {
            return (long)a + (long)b + c;
        }
    }
}
