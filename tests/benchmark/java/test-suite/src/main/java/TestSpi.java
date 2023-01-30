
package com.example.proj;

import java.sql.ResultSet;
import java.sql.Connection;
import java.sql.Statement;
import java.sql.DriverManager;

import org.postgresql.pljava.annotation.Function;

public class TestSpi {

    @Function
    public static int returnCompositeSumJava() throws java.sql.SQLException {
        int sum = 0;
        Connection conn = DriverManager.getConnection("jdbc:default:connection");
        Statement stmt = conn.createStatement();
        ResultSet rs = stmt.executeQuery("SELECT 1 as c, 2 as b");
        try {
            while ( rs.next() ) {
                sum += rs.getInt(1) + rs.getInt(2);
            }
        } finally {
            try { rs.close(); } catch (Exception ignore) { }
        }
        return sum;
    }

    @Function
    public static boolean checkTypesJava() throws java.sql.SQLException {
        int sum = 0;
        Connection conn = DriverManager.getConnection("jdbc:default:connection");
        Statement stmt = conn.createStatement();
        ResultSet rs = stmt.executeQuery("SELECT * from pldotnettypes");
        try {
            while ( rs.next() ) {
                if(
                    rs.getBoolean(1) != true
                    || rs.getShort(2) != (short)1
                    || rs.getInt(3) != 32767
                    || rs.getLong(4) != 9223372036854775707L
                    || rs.getFloat(5) != 1.4f
                    || rs.getDouble(6) != 10.5000000000055
                    || rs.getFloat(7) != 1.2f
                    || !rs.getString(8).equals("StringSample;")
                    ){
                        return false;
                }
            }
            rs.close();
        } catch (Exception e) {
            return false;
        }
        return true;
    }

    @Function
    public static String getUsersWithBalanceJava(float searchBalance) throws java.sql.SQLException {
        int sum = 0;
        Connection conn = DriverManager.getConnection("jdbc:default:connection");
        Statement stmt = conn.createStatement();
        ResultSet rs = stmt.executeQuery("SELECT * from usersavings");

        String res = "User(s) found with "+searchBalance+" account balance";
        try {
            while ( rs.next() ) {
                long ssnum = rs.getLong(1);
                String name = rs.getString(2);
                String sname = rs.getString(3);
                double userBalance = rs.getDouble(4);
                if(searchBalance == userBalance) {
                    res += ", "+name+" "+sname+" (Social Security Number "+ssnum+")";
                }
            }
            
        } finally {
            try { rs.close(); } catch (Exception ignore) { }
        }
        res += ".";
        return res;
    }

    @Function
    public static String getUserDescriptionJava(long ssnumParam) throws java.sql.SQLException {
        int sum = 0;
        Connection conn = DriverManager.getConnection("jdbc:default:connection");
        Statement stmt = conn.createStatement();
        ResultSet rs = stmt.executeQuery("SELECT * from usersavings WHERE ssnum="+ssnumParam);

        String res = "No user found";
        try {
            while ( rs.next() ) {
                long ssnum = rs.getLong(1);
                String name = rs.getString(2);
                String sname = rs.getString(3);
                double balance = rs.getDouble(4);
                if(ssnum == ssnumParam) {
                    res = name+" "+sname+", Social security Number "+ssnum+", has "+balance+" account balance.";
                }
            }
            
        } finally {
            try { rs.close(); } catch (Exception ignore) { }
        }
        return res;
    }
}