package com.example.proj;

import java.sql.SQLException;
import org.postgresql.pljava.annotation.Function;

import java.util.logging.Logger;

public class TestMixedTypes {
    @Function
    public static String ageTestJava(String name, int age, String lname) throws SQLException {
        String res = "";
        if (age < 18) {
            return "Hey "+name+" "+lname+"! Dude you are still a kid.";
        } else if(age >= 10 && age < 40) {
            return "Hey "+name+" "+lname+"! You are in the mood!";
        } else {
            return "Hey "+name+" "+lname+"! You are getting experienced!";
        }
    }
}