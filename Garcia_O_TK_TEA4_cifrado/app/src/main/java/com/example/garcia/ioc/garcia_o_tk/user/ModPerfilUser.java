package com.example.garcia.ioc.garcia_o_tk.user;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.example.garcia.ioc.garcia_o_tk.R;
import com.example.garcia.ioc.garcia_o_tk.admin.AdminAct;
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

/*
    author:Claudio Garcia.
    classe per modificar algunes dades del perfil d'usuari normal.
 */
public class ModPerfilUser extends AppCompatActivity {

    private EditText name, pass;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_mod_perfil_user);
        //Obtenim del menú principal el TOKEN d'autenticació i l'user name.
        Intent intent = getIntent();
        String uName = intent.getStringExtra("Name");
        String authToken = intent.getStringExtra("TOKEN");
        //Localitzem els editText al layout.
        name = findViewById(R.id.editTextNewNamePerfilUser);
        pass = findViewById(R.id.editTextNewPass);
        //Asignem els valors.
        String nameS = name.getText().toString();
        String pasS = pass.getText().toString();


        Button btnMd = findViewById(R.id.button_send);
        btnMd.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                name = findViewById(R.id.editTextNewNamePerfilUser);
                pass = findViewById(R.id.editTextNewPass);

                String nameS = name.getText().toString();
                String pasS = pass.getText().toString();
                //El rol d'usuari no el pot modificar un usuari normal, aixi que posem el valor per defecte.
                String rol = "normal";
                //Creem un objecte JSON amb les noves dades.
                JSONObject jsonObject = new JSONObject();
                try {
                    jsonObject.put("name", nameS);
                    jsonObject.put("pswd_app", pasS);
                    jsonObject.put("rol_user", rol);
                    jsonObject.put("user_name", uName);
                } catch (JSONException e) {
                    throw new RuntimeException(e);
                }
                Log.i("jsmod: ", jsonObject.toString());
                //Si els camps no son buits, cridem el métode que modifica les dades.
                if (!nameS.isEmpty() && !pasS.isEmpty()) {
                    //Executem en un altre fil que no es el principal.
                    ExecutorService executor = Executors.newSingleThreadExecutor();
                    executor.execute(() -> {

                        try {
                            ApiRequests.updateClient(ModPerfilUser.this, authToken, jsonObject, new ApiRequests.ApiListener() {
                                @Override
                                public void onLoginSuccess(String token, Context context) {
                                    //Si la solicitud ha tingut exit, mostreme l toast i tornem al menú principal.
                                    Toast.makeText(ModPerfilUser.this, "Dades modificades correctament.", Toast.LENGTH_LONG).show();
                                    finish();
                                    Intent intent = new Intent(getApplicationContext(), UserAct.class);
                                    intent.putExtra("TOKEN", authToken);
                                    startActivity(intent);
                                }

                                @Override
                                public void onLoginError(String error, Context context) {
                                    Toast.makeText(ModPerfilUser.this, "No s'han pogut modificar les dades.", Toast.LENGTH_LONG).show();

                                }
                            });
                        } catch (Exception e) {
                            throw new RuntimeException(e);
                        }
                    });

                } else {
                    Toast.makeText(ModPerfilUser.this, "S'han demplenar tots els camps.", Toast.LENGTH_LONG).show();
                }
            }
        });
    }
    /*
        Métode que finalitza l'execució d'aquesta activity i torna al menú principal si es prem el botó corresponent.
     */
    public void clse(View view) {
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        finish();
        Intent intent2 = new Intent(getApplicationContext(), UserAct.class);
        intent2.putExtra("TOKEN", authToken);
        startActivity(intent2);
    }
}