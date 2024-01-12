package com.example.garcia.ioc.garcia_o_tk.admin.perfil;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.example.garcia.ioc.garcia_o_tk.R;
import com.example.garcia.ioc.garcia_o_tk.admin.AdminAct;
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;
import com.example.garcia.ioc.garcia_o_tk.encription.JWTDecoder;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;


/**
 * author: Claudio Garcia Otero
 * Classe per veure les dades del nostre gym com a administrador.
 */
public class PerfilAdmin extends AppCompatActivity {

    private EditText  dir, num, time;

    private TextView dirN, numN, timeN;

    private String actualDir, actualNum, actualTime;

    private boolean enabled = false;

    @SuppressLint("MissingInflatedId")
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_perfil_admin);
        //Rebrem el TOKEN de AdminAct, servirà per pasar com autenticació al header de la sol·licitud.
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        Log.d("tk",authToken);
        //En un principi les modificacions de les dades no hi son actives.
        TextView name = findViewById(R.id.text_header);
        name.setEnabled(false);
        dir = findViewById(R.id.editTextAddress);
        dir.setEnabled(false);
        num = findViewById(R.id.editTextPhone);
        num.setEnabled(false);
        time = findViewById(R.id.editTextTime);
        time.setEnabled(false);

        Button btnMod = findViewById(R.id.button_modify);
        //Executem en un altre fil la connexió al servidor.
        ExecutorService executor = Executors.newSingleThreadExecutor();
        executor.execute(() -> {

            if (authToken != null) {
                // Métode que es conecta a l'API del perfil del gimnàs.
                try {
                    ApiRequests.getProfileInfo(PerfilAdmin.this, authToken, new ApiRequests.ApiListener() {
                        @Override
                        public void onLoginSuccess(String token, Context context) {
                            // Instanciem un objecte GymInfo que conté els constructors dels Strings que composen el TOKEN obtingut.
                            // Decodifiquem el TOKEN amb el métode decodeUserGym de la classe JWTDecoder.
                            GymInfo gymInfo = JWTDecoder.decodeUserGymInfo(token);
                            // Verifiquen que hem pogut decodificar el TOKEN.
                            if (gymInfo != null) {
                                // Accedim a les dades que volem mostrar.
                                String gymName = gymInfo.getGymName();
                                String gymAddress = gymInfo.getGymAddress();
                                String gymPhoneNumber = gymInfo.getGymPhoneNumber();
                                List gymSchedule = gymInfo.getGymScheduleString();
                                String schedule = gymSchedule.toString();
                                // Situem els String's obtinguts als editText corresponents.
                                name.setText(gymName.toUpperCase());
                                dir.setText(gymAddress);
                                num.setText(gymPhoneNumber);
                                time.setText(schedule);

                            } else {

                                Log.e("DecodeError:", "Error en decodificar la informació del TOKEN");
                            }
                        }

                        @Override
                        public void onLoginError(String error, Context context) {

                            Log.e("ProfileError", "Error en obtenir la informació del perfil: " + error);
                        }
                    });
                } catch (Exception e) {
                    throw new RuntimeException(e);
                }
            } else {
                // Manejar el caso cuando no se recibe un token válido
                Log.e("InvalidTk", "El TOKEN obtingut no es vàlid.");
            }
        });
        // Botó per modificar les dades-
        btnMod.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                // En fer click si no estan habilitats els camps desitjats els habilitem.
                if(!enabled) {
                    Toast.makeText(PerfilAdmin.this, "Introdueix les noves dades.", Toast.LENGTH_LONG).show();
                    dir.setEnabled(true);
                    num.setEnabled(true);
                    time.setEnabled(true);
                    time.getText().clear();
                    btnMod.setText("Envia.");
                    enabled = true;
                } else{
                    // Obtenim les noves dades introduides.
                    dirN = findViewById(R.id.editTextAddress);
                    numN = findViewById(R.id.editTextPhone);
                    timeN = findViewById(R.id.editTextTime);
                    actualDir = dirN.getText().toString();
                    actualNum = numN.getText().toString();
                    actualTime = timeN.getText().toString();
                    List<String> timeList = new ArrayList<>();
                    timeList.add(actualTime);
                    timeN.setText(actualTime);
                    actualTime = timeN.getText().toString();
                    // Només si totes les dades s'han emplenat, procedim a realitzar una sol·licitud de modificació de dades.
                    if((!actualDir.isEmpty()) && (!actualNum.isEmpty()) &&(!actualTime.isEmpty())) {
                        runOnUiThread(new Runnable() {
                            @Override
                            public void run() {
                                // Métode per actualitzar les dades del gimnàs.
                                try {
                                    ApiRequests.updateGymData(PerfilAdmin.this, authToken, actualDir, actualNum, timeList, new ApiRequests.ApiListener() {
                                        @Override
                                        public void onLoginSuccess(String token, Context context) {
                                            // Si tot es correcte tornarem al menú principal. Hem de tronar el TOKEN d'autenticació.
                                            Toast.makeText(PerfilAdmin.this, "Dades modificades correctament.", Toast.LENGTH_LONG).show();
                                            finish();
                                            Intent intent = new Intent(getApplicationContext(), AdminAct.class);
                                            intent.putExtra("TOKEN", authToken);
                                            startActivity(intent);
                                            // Desactivem l'edició dels camps.
                                            enabled = false;

                                        }
                                        @Override
                                        public void onLoginError(String error, Context context) {

                                            Toast.makeText(PerfilAdmin.this, "Error en la modificació de dades.", Toast.LENGTH_LONG).show();
                                            enabled = false;
                                            btnMod.setText("Modifica les dades.");
                                        }
                                    });
                                } catch (Exception e) {
                                    throw new RuntimeException(e);
                                }
                            }
                        });

                    } else {
                        Toast.makeText(PerfilAdmin.this,"Hi ha almenys un camp incomplet.",Toast.LENGTH_LONG).show();
                        enabled = false;
                        btnMod.setText("Modifica les dades.");

                }}
            }
        });
    }
    // Botó per tornar al menú principal sense modificar res.
    public void close(View view) {
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        finish();
        Intent intent2 = new Intent(getApplicationContext(), AdminAct.class);
        intent2.putExtra("TOKEN", authToken);
        startActivity(intent2);
    }
}