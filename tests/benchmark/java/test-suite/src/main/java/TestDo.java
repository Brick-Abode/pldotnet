package com.example.proj;

import java.sql.SQLException;
import org.postgresql.pljava.annotation.Function;

import java.util.logging.Logger;

public class TestDo {
    @Function
    public static int doJava() throws SQLException {
        int sumResult = 3 + 7;
        Logger.getAnonymousLogger().info("Compiled SUm: " + sumResult);
        return 1;
    }
}
