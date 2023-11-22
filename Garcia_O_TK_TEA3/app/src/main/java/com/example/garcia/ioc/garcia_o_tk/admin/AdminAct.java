package com.example.garcia.ioc.garcia_o_tk.admin;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;

import com.example.garcia.ioc.garcia_o_tk.R;
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;

/**
 * author: Claudio Garcia Otero
 * Classe amb les opcions per a un usuari administrador.
 */
public class AdminAct extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_admin);
        //Obtenim el TOKEN de la MainActivity.
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        //Botó que ens porta a veure la info de perfil del gimnàs.
        Button btnPerfil=findViewById(R.id.button_perfil);
        btnPerfil.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Iniciem una altra activity i tornem a pasar el TOKEN, servirà per poder fer modificacions a les dades del perfil.
                Intent intent = new Intent(getApplicationContext(), PerfilAdmin.class);
                intent.putExtra("TOKEN", authToken);
                startActivity(intent);
                finish();

            }
        });
        //Botó que ens porta a veure els usuaris del gimnàs.
        Button btnClients = findViewById(R.id.button_clients);
        btnClients.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Iniciem una altra activity i tornem a pasar el TOKEN, servirà per poder llistar els clients.
                Intent intent = new Intent(getApplicationContext(), Clients.class);
                intent.putExtra("TOKEN", authToken);
                startActivity(intent);
                finish();

            }
        });
        Button btnMod = findViewById(R.id.button_mod);
        btnMod.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Iniciem una altra activity i tornem a pasar el TOKEN, servirà per poder fer modificacions a les dades de client.
                Intent intent = new Intent(getApplicationContext(), altaBaixaClients.class);
                intent.putExtra("TOKEN", authToken);
                startActivity(intent);
                finish();

            }
        });
        // Botó per fer logout del server.
        Button btnClose = findViewById(R.id.button_logout);
        btnClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                ApiRequests.logoutRequest(AdminAct.this, authToken, new ApiRequests.ApiListener() {
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