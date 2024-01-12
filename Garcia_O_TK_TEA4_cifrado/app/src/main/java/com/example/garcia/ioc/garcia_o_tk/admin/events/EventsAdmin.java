package com.example.garcia.ioc.garcia_o_tk.admin.events;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import com.example.garcia.ioc.garcia_o_tk.R;
import com.example.garcia.ioc.garcia_o_tk.admin.AdminAct;

/**
 * author: Claudio García.
 * Classe que representa el menú dels events.
 */

public class EventsAdmin extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_events_admin);
        //Obtenim el TOKEN de la MainActivity.
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        //Botó que ens porta a l'acivity per veure el llistat d'events.
        Button btnList=findViewById(R.id.button_events_list);
        btnList.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Iniciem una altra activity i tornem a pasar el TOKEN.
                Intent intent = new Intent(getApplicationContext(), EventsList.class);
                intent.putExtra("TOKEN", authToken);
                startActivity(intent);
                finish();

            }
        });
        //Botó que ens porta a l'acivity per inserir o modificar events.
        Button btnIns=findViewById(R.id.button_n_event);
        btnIns.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Iniciem una altra activity i tornem a pasar el TOKEN, servirà per poder fer modificacions a les dades del perfil.
                Intent intent = new Intent(getApplicationContext(), InsertEvent.class);
                intent.putExtra("TOKEN", authToken);
                startActivity(intent);
                finish();

            }
        });
        //Botó que ens porta a l'acivity per eliminar events.
        Button btnDel=findViewById(R.id.button_del_event);
        btnDel.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Iniciem una altra activity i tornem a pasar el TOKEN, servirà per poder fer modificacions a les dades del perfil.
                Intent intent = new Intent(getApplicationContext(), EventsDel.class);
                intent.putExtra("TOKEN", authToken);
                startActivity(intent);
                finish();

            }
        });

    }

    public void closed(View view) {
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        finish();
        Intent intent2 = new Intent(getApplicationContext(), AdminAct.class);
        intent2.putExtra("TOKEN", authToken);
        startActivity(intent2);
    }

}