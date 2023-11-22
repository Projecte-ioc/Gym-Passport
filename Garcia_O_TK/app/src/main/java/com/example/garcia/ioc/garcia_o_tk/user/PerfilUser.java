package com.example.garcia.ioc.garcia_o_tk.user;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

import com.example.garcia.ioc.garcia_o_tk.R;
import com.example.garcia.ioc.garcia_o_tk.admin.GymInfo;
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;
import com.example.garcia.ioc.garcia_o_tk.connections.JWTDecoder;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class PerfilUser extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_perfil_user);

        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");

        ExecutorService executor = Executors.newSingleThreadExecutor();

        executor.execute(() -> {
            ApiRequests.getProfileInfo(PerfilUser.this, authToken, new ApiRequests.ApiListener() {
                @Override
                public void onLoginSuccess(String token) {

                    UserInfo userInfo = JWTDecoder.decodeUserInfo(token);
                    Log.d("ProfileUser:", "Token del perfil: " + userInfo);

                    String userName = userInfo.getUserName();
                    String userRole = userInfo.getUserRole();
                    String gymName = userInfo.getGymName();
                    String name = userInfo.getName();

                    Log.i("ProfileInfo", "Nombre de usuario: " + userName);
                    Log.i("ProfileInfo", "Rol de usuario: " + userRole);
                    Log.i("ProfileInfo", "Nombre del gimnasio: " + gymName);
                    Log.i("ProfileInfo", "Nombre del gimnasio: " + name);
                }

                @Override
                public void onLoginError(String error) {

                }
            });
        });
    }
}