package com.example.proj;

import java.sql.SQLException;
import org.postgresql.pljava.annotation.Function;

import java.util.logging.Logger;

public class TestNullBool {
    @Function
    public static Boolean returnNullBoolJava() throws SQLException {
        return null;
    }

    @Function
    public static Boolean BooleanNullAndJava(Boolean a, Boolean b) throws SQLException {
        try {
            return a && b;
        } catch (NullPointerException ex) {
            return null;
        }
    }

    @Function
    public static Boolean BooleanNullOrJava(Boolean a, Boolean b) throws SQLException {
        try {
            return a || b;
        } catch (NullPointerException ex) {
            return null;
        }
    }

    @Function
    public static Boolean BooleanNullXorJava(Boolean a, Boolean b) throws SQLException {
        try {
            return (a && !b) || (!a && b);
        } catch (NullPointerException ex) {
            return null;
        }
    }
}
