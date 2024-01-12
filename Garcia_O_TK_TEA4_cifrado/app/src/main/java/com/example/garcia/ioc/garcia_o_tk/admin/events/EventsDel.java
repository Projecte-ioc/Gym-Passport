package com.example.garcia.ioc.garcia_o_tk.admin.events;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.ListView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.example.garcia.ioc.garcia_o_tk.R;
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;
import com.example.garcia.ioc.garcia_o_tk.encription.JWTDecoder;
import com.nimbusds.jwt.JWT;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.ParseException;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

/**
 * author: Claudio Garcia.
 * activity per eliminar events.
 */

public class EventsDel extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_events_del);
        //Obtenim el TOKEN de l'activitat principal
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");

        Button del = findViewById(R.id.button_del);
        //Desactivem el botó per que no es pugui seleccionar d'entrada.
        del.setEnabled(false);
        //Executem la solicitud en un altre fil.
        ExecutorService executor = Executors.newSingleThreadExecutor();
        executor.execute(() -> {
            try {
                ApiRequests.requestEvents(EventsDel.this, authToken, new ApiRequests.ApiListener() {
                    @Override
                    public void onLoginSuccess(String token, Context context) {
                        //Tornem al fil de la interfície per fer servir la resposta.
                        runOnUiThread(() -> {
                            try {
                                //Decodfiquem el TOKEN.
                                JWT jwt = JWTDecoder.decodeJWT(token);

                                if (jwt != null) {
                                    //List JSON per afegir cada objecte JSON
                                    List<JSONObject> eventsJsonList = new ArrayList<>();
                                    //Obtenim les claims del token i les enmagatzemem en un String.
                                    String claimsJsonString = jwt.getJWTClaimsSet().toString();
                                    Log.d("Claims list", claimsJsonString);
                                    try {
                                        // Convertir el String JSON a un JSONObject
                                        JSONObject claimsJsonObject = new JSONObject(claimsJsonString);

                                        JSONArray jsonArray = claimsJsonObject.getJSONArray("results");

                                        // Iterem sobre l'array per obtenir cada fila de dades.
                                        for (int i = 0; i < jsonArray.length(); i++) {
                                            JSONObject jsonObject = jsonArray.getJSONObject(i);
                                            //Creem un objecte JSON amb les claims.
                                            JSONObject eventJson = new JSONObject();
                                            eventJson.put("id", jsonObject.getString("id"));
                                            eventJson.put("name", jsonObject.getString("name"));
                                            eventJson.put("date", jsonObject.getString("date"));

                                            Log.d("Client JSON", eventJson.toString());
                                            //Afegim l'objecte al List.
                                            eventsJsonList.add(eventJson);
                                        }
                                    } catch (JSONException e) {
                                        e.printStackTrace();
                                    }
                                    try {
                                        //List de l'objecte EventsInfoDel, servira per carregar el listItem del layout amb les dades.
                                        List<EventsInfoDel> eventsList = new ArrayList<>();

                                        for (JSONObject eventJson : eventsJsonList) {
                                            int id = eventJson.getInt("id");
                                            String name = eventJson.getString("name");
                                            String date = eventJson.getString("date");

                                            EventsInfoDel event = new EventsInfoDel(id, name, date);
                                            eventsList.add(event);
                                        }

                                        EventsAdapterDel eventAdapter = new EventsAdapterDel(EventsDel.this, eventsList);
                                        // Obtenim el listView del layout.
                                        ListView listView = findViewById(R.id.listEv);
                                        // Configurem l'adaptador en el lsitView
                                        listView.setAdapter(eventAdapter);
                                        // Fem cliclable cada fila del ListView
                                        listView.setOnItemClickListener((parent, view, i, id) -> {
                                            // de la fila clicada obtenim l'id de l'event
                                            EventsInfoDel selectedEvent = eventsList.get(i);
                                            int eventId = selectedEvent.getId();
                                            //Un cop seleccionat un event, activem el botó per esborrar.
                                            del.setEnabled(true);
                                            Toast.makeText(EventsDel.this, "S'ha seleccionat l'event amb id: "+eventId+".", Toast.LENGTH_LONG).show();
                                            del.setOnClickListener(view1 -> {
                                                //Generem un objecte JSON amb el valor de l'id.
                                                String id1 = Integer.toString(eventId);
                                                ExecutorService executor1 = Executors.newSingleThreadExecutor();
                                                executor1.execute(() -> ApiRequests.deleteEvent(EventsDel.this, authToken, id1, new ApiRequests.ApiListener() {
                                                    @Override
                                                    public void onLoginSuccess(String token1, Context context1) {
                                                        Toast.makeText(EventsDel.this, "S'ha eliminat l'event amb id: "+eventId+".", Toast.LENGTH_LONG).show();
                                                        Intent intent1 = getIntent();
                                                        String authToken1 = intent1.getStringExtra("TOKEN");
                                                        Intent intent2 = new Intent(getApplicationContext(), EventsAdmin.class);
                                                        intent2.putExtra("TOKEN", authToken1);
                                                        startActivity(intent2);
                                                        finish();
                                                    }

                                                    @Override
                                                    public void onLoginError(String error, Context context1) {
                                                        Toast.makeText(EventsDel.this, "No s'ha pogut eliminar l'event.", Toast.LENGTH_LONG).show();

                                                    }
                                                }));
                                            });
                                        });

                                    } catch (JSONException e) {
                                        e.printStackTrace();

                                    }

                                } else {
                                    Log.e("Error decoding JWT", "JWT es nul");
                                }
                            } catch (ParseException e) {
                                e.printStackTrace();
                            }

                        });
                    }

                    @Override
                    public void onLoginError(String error, Context context) {

                        runOnUiThread(() -> Log.e("Client: ",error));
                    }
                });
            } catch (Exception e) {
                throw new RuntimeException(e);
            }
        });

    }
    /*
        Métode que finalitza l'execució d'aquesta activity i torna al menú principal d'events si es prem el botó corresponent.
     */
    public void closd(View view) {
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        finish();
        Intent intent2 = new Intent(getApplicationContext(), EventsAdmin.class);
        intent2.putExtra("TOKEN", authToken);
        startActivity(intent2);
    }
}