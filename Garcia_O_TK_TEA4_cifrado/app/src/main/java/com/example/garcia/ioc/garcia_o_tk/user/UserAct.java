package com.example.garcia.ioc.garcia_o_tk.user;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.widget.Button;

import androidx.appcompat.app.AppCompatActivity;

import com.example.garcia.ioc.garcia_o_tk.R;
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;

/**
 * author: Claudio Garcia.
 * Classe que representa el menú principal de l'usuari normal, conte boto que porta l'activity corresponent.
 */

public class UserAct extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_user);

        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");

        Button btnPerfil=findViewById(R.id.button_perfil);
        btnPerfil.setOnClickListener(view -> {
            //Iniciem una altra activity i tornem a pasar el TOKEN, servirà per poder fer modificacions a les dades del perfil.
            Intent intent1 = new Intent(getApplicationContext(), PerfilUser.class);
            intent1.putExtra("TOKEN", authToken);
            startActivity(intent1);
            finish();

        });

        Button btnClose = findViewById(R.id.button_logout);
        btnClose.setOnClickListener(view -> {
            try {
                ApiRequests.logoutRequest(UserAct.this, authToken, new ApiRequests.ApiListener() {
                    @Override
                    public void onLoginSuccess(String token, Context context) {
                        finish();
                    }

                    @Override
                    public void onLoginError(String error, Context context) {
                        Log.i("Logout", "no");

                    }
                });
            } catch (Exception e) {
                throw new RuntimeException(e);
            }
        });

    }



}