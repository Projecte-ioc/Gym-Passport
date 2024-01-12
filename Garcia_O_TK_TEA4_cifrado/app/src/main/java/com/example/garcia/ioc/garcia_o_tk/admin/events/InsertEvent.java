package com.example.garcia.ioc.garcia_o_tk.admin.events;

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
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

/**
 * author: Claudio García
 * Classe que representa un activity per inserir o modificar events.
 */

public class InsertEvent extends AppCompatActivity {

    private EditText name, date, hour, num, loc, id;
    private String namS, dateS, hourS, numS, locS, done, minute, duration, idS;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_insert_event);
        //Obtenim el TOKEN de la MainActivity.
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");

        name = findViewById(R.id.editTextNameEvent);
        date = findViewById(R.id.editTextDate);
        hour = findViewById(R.id.editTextHour);
        num = findViewById(R.id.editTextAtt);
        loc = findViewById(R.id.editTextLoc);
        id = findViewById(R.id.editId);


        //Localitzem el botó que servirà per inserir events.
        Button btnIns = findViewById(R.id.button_insert_event);
        btnIns.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Obtenim les dades dels editText corresponents, hi ha algunes dades que seràn per defecte per agilitzar el proces.
                namS = name.getText().toString();
                dateS = date.getText().toString();
                hourS = hour.getText().toString();
                numS = num.getText().toString();
                locS = loc.getText().toString();
                done = "false";
                minute = "15";
                duration = "45";
                //Només es realitzará la solicitud si cap camp es buit.
                if ((!namS.isEmpty()) && (!dateS.isEmpty()) & (!hourS.isEmpty()) && (!numS.isEmpty()) && (!locS.isEmpty())) {
                    //Creem l'objecte JSON amb les dades obtingudes.
                    JSONObject jsonBody = new JSONObject();
                    try {
                        jsonBody.put("date", dateS);
                        jsonBody.put("done", done);
                        jsonBody.put("hour", hourS);
                        jsonBody.put("minute", minute);
                        jsonBody.put("duration", duration);
                        jsonBody.put("name", namS);
                        jsonBody.put("qty_max_attendes", numS);
                        jsonBody.put("whereisit", locS);
                        Log.d("js-events", jsonBody.toString());
                    } catch (JSONException e) {
                        e.printStackTrace();
                    }
                    ExecutorService executor = Executors.newSingleThreadExecutor();
                    executor.execute(() -> {
                        try {
                            ApiRequests.insertEvent(InsertEvent.this, authToken, jsonBody, new ApiRequests.ApiListener() {
                                @Override
                                public void onLoginSuccess(String token, Context context) {
                                    Toast.makeText(InsertEvent.this, "S'ha creat un nou event.", Toast.LENGTH_LONG).show();
                                    Intent intent = getIntent();
                                    String authToken = intent.getStringExtra("TOKEN");
                                    finish();
                                    Intent intent2 = new Intent(getApplicationContext(), EventsAdmin.class);
                                    intent2.putExtra("TOKEN", authToken);
                                    startActivity(intent2);


                                }

                                @Override
                                public void onLoginError(String error, Context context) {
                                    Toast.makeText(InsertEvent.this, "Error en la creació de l'event.", Toast.LENGTH_LONG).show();

                                }
                            });
                        } catch (Exception e) {
                            throw new RuntimeException(e);
                        }
                    });

                } else {
                    Toast.makeText(InsertEvent.this, "Has d'emplenar tots els camps.", Toast.LENGTH_LONG).show();
                }
            }
        });
        //Localitzem el botó per modificar events.
        Button btnMd = findViewById(R.id.button_modify_event);
        btnMd.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Si es prem el botó desactivem l'opció d'inserir events.
                btnIns.setEnabled(false);
                TextView a = findViewById(R.id.textViewId);
                TextView b = findViewById(R.id.editId);
                //Fem visible l'editText d'id, camp necessari per modifcar l'event.
                a.setVisibility(View.VISIBLE);
                b.setVisibility(View.VISIBLE);
                Toast.makeText(InsertEvent.this, "Introdueix les noves dades.", Toast.LENGTH_LONG).show();
                btnMd.setText("Envia l'event modificat.");
                btnMd.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        //Obtenim les noves dades.
                        idS = id.getText().toString();
                        namS = name.getText().toString();
                        dateS = date.getText().toString();
                        hourS = hour.getText().toString();
                        numS = num.getText().toString();
                        locS = loc.getText().toString();
                        done = "false";
                        minute = "15";
                        duration = "45";
                        //Si cap camp es buit realitzarem la solicitud.
                        if ((!idS.isEmpty())&&(!namS.isEmpty()) && (!dateS.isEmpty()) & (!hourS.isEmpty()) && (!numS.isEmpty()) && (!locS.isEmpty())) {
                            JSONObject jsonBody = new JSONObject();
                            try {
                                jsonBody.put("event_id",idS);
                                jsonBody.put("date", dateS);
                                jsonBody.put("done", done);
                                jsonBody.put("hour", hourS);
                                jsonBody.put("minute", minute);
                                jsonBody.put("duration", duration);
                                jsonBody.put("name", namS);
                                jsonBody.put("qty_max_attendes", numS);
                                jsonBody.put("whereisit", locS);
                                Log.d("js-events", jsonBody.toString());
                            } catch (JSONException e) {
                                e.printStackTrace();
                            }
                            ExecutorService executor = Executors.newSingleThreadExecutor();
                            executor.execute(() -> {
                                try {
                                    ApiRequests.modEvent(InsertEvent.this, authToken, idS, jsonBody, new ApiRequests.ApiListener() {
                                        @Override
                                        public void onLoginSuccess(String token, Context context) {
                                            Toast.makeText(InsertEvent.this, "Event modificat correctament.", Toast.LENGTH_LONG).show();
                                            Intent intent = getIntent();
                                            String authToken = intent.getStringExtra("TOKEN");
                                            finish();
                                            Intent intent2 = new Intent(getApplicationContext(), EventsAdmin.class);
                                            intent2.putExtra("TOKEN", authToken);
                                            startActivity(intent2);

                                        }

                                        @Override
                                        public void onLoginError(String error, Context context) {
                                            Toast.makeText(InsertEvent.this, "L'id de l'event a modificar no existeix.", Toast.LENGTH_LONG).show();
                                        }
                                    });
                                } catch (Exception e) {
                                    throw new RuntimeException(e);
                                }
                            });
                        }

                    }
                });
            }
        });


    }
    /*
        Métode que finalitza l'execució d'aquesta activity i torna al menú principal d'events si es prem el botó corresponent.
     */

    public void clsed(View view) {
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        finish();
        Intent intent2 = new Intent(getApplicationContext(), EventsAdmin.class);
        intent2.putExtra("TOKEN", authToken);
        startActivity(intent2);
    }
}