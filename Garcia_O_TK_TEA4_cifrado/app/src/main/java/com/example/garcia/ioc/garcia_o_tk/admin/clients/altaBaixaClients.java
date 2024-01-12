package com.example.garcia.ioc.garcia_o_tk.admin.clients;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;
import com.example.garcia.ioc.garcia_o_tk.R;
import com.example.garcia.ioc.garcia_o_tk.admin.AdminAct;
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;
import org.json.JSONException;
import org.json.JSONObject;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

/**
 * author: Claudio Garcia Otero
 * Classe amb l'activity que permet donar d'alta o de baixa un usari
 */

public class altaBaixaClients extends AppCompatActivity {

    private EditText name, rol, pass, userName, nameDel;

    private JSONObject client = new JSONObject();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_alta_baixa_clients);
        //Rebem el TOKEN de l'activity perfilAdmin.
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        //Variables per trobar els editText dels valors inserits.
        name = findViewById(R.id.editText_New_Name);
        rol = findViewById(R.id.editText_New_Rol);
        pass = findViewById(R.id.editText_New_Pswd);
        userName = findViewById(R.id.editText_New_Nick);
        nameDel = findViewById(R.id.editText_userDelete);
        //Botó per donar d'alta un usari nou.
        Button btnAlta = findViewById(R.id.button_alta);
        btnAlta.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Variables que recullen en strings els valors inserits.
                String nameS = name.getText().toString();
                String rolS = rol.getText().toString();
                String userNameS = userName.getText().toString();
                String passS = pass.getText().toString();
                //Si els editText no hi son buits, omplim el JSONobject.
                if (!nameS.isEmpty() && !rolS.isEmpty() && !userNameS.isEmpty() && !passS.isEmpty()) {
                    try {
                        client.put("name", nameS);
                        client.put("pswd_app", passS);
                        client.put("rol_user", rolS);
                        client.put("user_name", userNameS);

                    } catch (JSONException e) {
                        e.printStackTrace();
                        return;
                    }
                    //Executem en un altre fil que no es el principal la connexió a l'API.
                    ExecutorService executor = Executors.newSingleThreadExecutor();
                    executor.execute(() -> {
                        //Cridem el métode per inserir clients, passem per parametre l'objecte JSON que hem creat.
                        try {
                            ApiRequests.insertClient(altaBaixaClients.this, authToken, client, new ApiRequests.ApiListener() {
                                @Override
                                public void onLoginSuccess(String token, Context context) {
                                    runOnUiThread(() -> Toast.makeText(altaBaixaClients.this, "Nou usuari registrat correctament.", Toast.LENGTH_LONG).show());
                                }

                                @Override
                                public void onLoginError(String error, Context context) {
                                    runOnUiThread(() -> Toast.makeText(altaBaixaClients.this, "No s'ha pogut crear un nou usuari.", Toast.LENGTH_LONG).show());
                                }
                            });
                        } catch (Exception e) {
                            throw new RuntimeException(e);
                        }
                    });
                } else {
                    Toast.makeText(altaBaixaClients.this, "Per poder crear un usari s'han d'emplenar tots els camps.", Toast.LENGTH_LONG).show();
                }
            }
        });
        //Botó por esborrar un usuari.
        Button deleteBtn = findViewById(R.id.button_delete);
        deleteBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String delName = nameDel.getText().toString();
                //Si l'editText del camp no es buit procedim a esborrar
                if (!delName.isEmpty()){
                    ExecutorService executor = Executors.newSingleThreadExecutor();
                    executor.execute(() -> {
                        //Cridem el métode per esborrar clients, passem per parametre el nom de l'usuari a esborrar.
                        ApiRequests.deleteClient(altaBaixaClients.this, authToken, delName, new ApiRequests.ApiListener() {
                            @Override
                            public void onLoginSuccess(String token, Context context) {
                                Toast.makeText(altaBaixaClients.this, "S'ha eliminat correctament l'usuari.", Toast.LENGTH_LONG).show();
                                Intent intent = getIntent();
                                String authToken = intent.getStringExtra("TOKEN");
                                Intent intent2 = new Intent(getApplicationContext(), AdminAct.class);
                                intent2.putExtra("TOKEN", authToken);
                                startActivity(intent2);
                                finish();
                            }

                            @Override
                            public void onLoginError(String error, Context context) {
                                runOnUiThread(() -> Toast.makeText(altaBaixaClients.this, "Aquest usuari no existeix.", Toast.LENGTH_LONG).show());
                            }
                        });
                    });

                }else{
                    Toast.makeText(altaBaixaClients.this, "Introdueix un usuari per esborrar.", Toast.LENGTH_LONG).show();
               }
           }
        });
    }

    public void close(View view) {
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        finish();
        Intent intent2 = new Intent(getApplicationContext(), AdminAct.class);
        intent2.putExtra("TOKEN", authToken);
        startActivity(intent2);
    }


}