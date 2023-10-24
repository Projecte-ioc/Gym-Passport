package com.example.garcia.ioc.garcia_o;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;



/**
 * author: Claudio Garcia Otero
 * Classe amb l'activitat principal de la APP.
 */
public class MainActivity extends AppCompatActivity {


    private Button btn;
    private EditText txtUser, txtPsw;

    private String user, userPsw;
    //Variable amb la URL de l'API de prova.
    private final String apiUrl = "http://10.0.2.2:3000/clientes";



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        //Obtenim els valors que l'usuari introdueix als camps
        //Nom i Contrasenya.
        txtUser = findViewById(R.id.editTextLog);
        txtPsw = findViewById(R.id.editTextLogPsw);
        //Variable del botó que farà el login.
        btn = findViewById(R.id.button_log);


        btn.setOnClickListener(new View.OnClickListener() {

            public void onClick(View v) {
                user = txtUser.getText().toString();
                userPsw = txtPsw.getText().toString();
                //Classe on es troben els mètodes per conectar-se.
                Connections conn = new Connections(apiUrl);
                //Funció que serveix per autenticar el usuari amb l'objcete JSON.
                conn.logFrom(user, userPsw, new Connections.Callback() {
                    @Override
                    //Segons la resposta obtinguda iniciem un activity o un altre.
                    //En cas de que l'usuari no coincideixi llançarem un missatge de error amb un toast.
                    public void onResponseReceived(int tipoRespuesta) {
                        if (tipoRespuesta == 1) {
                            Intent intent = new Intent(getApplicationContext(), AdminAct.class);
                            startActivity(intent);
                        } else if (tipoRespuesta == 2) {
                            Intent intent = new Intent(getApplicationContext(), UserAct.class);
                            startActivity(intent);
                        }else{
                            Toast.makeText(getApplicationContext(),"El usuari introdüit no existeix o les dades son incorrectes.",Toast.LENGTH_LONG).show();
                        }

                    }

                    @Override
                    public void onErrorResponse(String errorMessage) {
                        txtUser.setText("Error: " + errorMessage);

                    }

                    @Override
                    public Context getContext() {
                        return MainActivity.this;
                    }
                });





            }
        });


    }




}