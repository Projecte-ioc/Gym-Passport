package com.example.garcia.ioc.garcia_o_tk.user;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;

import com.example.garcia.ioc.garcia_o_tk.R;
import com.example.garcia.ioc.garcia_o_tk.admin.AdminAct;
import com.example.garcia.ioc.garcia_o_tk.admin.PerfilAdmin;
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;

public class UserAct extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_user);

        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");

        Button btnPerfil=findViewById(R.id.button_perfil);
        btnPerfil.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Iniciem una altra activity i tornem a pasar el TOKEN, servirà per poder fer modificacions a les dades del perfil.
                Intent intent = new Intent(getApplicationContext(), PerfilUser.class);
                intent.putExtra("TOKEN", authToken);
                startActivity(intent);
                finish();

            }
        });

        Button btnClose = findViewById(R.id.button_logout);
        btnClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                ApiRequests.logoutRequest(UserAct.this, authToken, new ApiRequests.ApiListener() {
                    @Override
                    public void onLoginSuccess(String token) {
                        finish();
                    }

                    @Override
                    public void onLoginError(String error) {
                        Log.i("Logout", "no");

                    }
                });
            }
        });

    }



}