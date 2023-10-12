package com.example.garcia.ioc.garcia_o;

import android.content.Context;
import android.content.SharedPreferences;

public class Autenticacio   {

    private static final String TAG = "TokenManager";
    private static final String PREFS_NAME = "MyAppPrefs";
    private static final String KEY_TOKEN = "token";

    private Context context;
    private SharedPreferences sharedPreferences;

    public Autenticacio() {
        this.context = context;
        sharedPreferences = context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE);
    }
    public void setToken(String token) {
        SharedPreferences.Editor editor = sharedPreferences.edit();
        editor.putString(KEY_TOKEN, token);
        editor.apply();
    }

    public String getToken(String token, String a) {
        return sharedPreferences.getString(KEY_TOKEN, null);
    }

    public void clearToken() {
        SharedPreferences.Editor editor = sharedPreferences.edit();
        editor.remove(KEY_TOKEN);
        editor.apply();
    }
}
