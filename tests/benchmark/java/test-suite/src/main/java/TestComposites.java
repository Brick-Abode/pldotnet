package com.example.proj;

import java.sql.SQLException;
import java.sql.ResultSet;
import org.postgresql.pljava.ResultSetProvider;
import org.postgresql.pljava.annotation.Function;
import static org.postgresql.pljava.annotation.Function.Effects.IMMUTABLE;

import org.postgresql.pljava.annotation.Function;

public class TestComposites {
    @Function
    public static int helloPersonAgeJava(ResultSet per) throws SQLException {
        return per.getInt(2);
    }

    
    @Function
    public static boolean helloPersonJava(ResultSet per, ResultSet receiver) throws SQLException {
        int n = 1;
        int age = per.getInt(2);

        receiver.updateString(1, per.getString(1));
        receiver.updateInt(2, age + n);
        receiver.updateDouble(3, per.getDouble(3));
        receiver.updateDouble(4, per.getDouble(4));
        receiver.updateDouble(5, per.getDouble(5));
        receiver.updateBoolean(6, per.getBoolean(6));

        return true;
    }
}
