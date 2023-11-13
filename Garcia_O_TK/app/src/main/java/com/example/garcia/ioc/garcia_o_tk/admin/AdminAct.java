package com.example.garcia.ioc.garcia_o_tk.admin;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import com.example.garcia.ioc.garcia_o_tk.R;

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

            }
        });


    }


    //Button que tanca l'activitat, com que l'API encara es de prova
    //no es desconecta del servidor, però en un futur serà la seva funció.
    public void close(View view) {
        finish();
    }
}