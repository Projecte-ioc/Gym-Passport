package com.example.garcia.ioc.garcia_o_tk;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.example.garcia.ioc.garcia_o_tk.admin.AdminAct;
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;
import com.example.garcia.ioc.garcia_o_tk.connections.JWTDecoder;
import com.example.garcia.ioc.garcia_o_tk.user.UserAct;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;


/**
 * author: Claudio Garcia Otero
 * Classe amb l'activitat principal de la APP, encarregada de fer el login.
 */
public class MainActivity extends AppCompatActivity {


    private Button btn;
    EditText txtUser, txtPsw;
    private String user, userPsw, userRole;

    //Missatje que es llençarà amb el Toast si hi ha errors amb el login.
    static final String MESSAGE="Error en les credencials o usuari no registrat.";



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
            /**
             * Captura l'event del clic al botó per iniciar sessió.
             *
             * @param v La vista sobre la qual es fa clic, en aquest cas, un botó.
             */

            public void onClick(View v) {
                user = txtUser.getText().toString();
                userPsw = txtPsw.getText().toString();

                //S'ha d'executar la sol·licitud en un fil a part, si no es així l'aplicació es bloqueja.
                ExecutorService executor = Executors.newSingleThreadExecutor();
                executor.execute(() -> {
                    try {
                        ApiRequests.loginRequest(MainActivity.this, user, userPsw, new ApiRequests.ApiListener() {
                            @Override
                            public void onLoginSuccess(String token, Context context) {

                                runOnUiThread(() -> {

                                    userRole = String.valueOf(JWTDecoder.getRoleFromToken(token));
                                    Log.i("usuario",userRole);
                                    //Segons si l'usuari es adminsitrador, obrim una activitat o altre.
                                    if(userRole.equals("admin")){
                                        Intent intent = new Intent(getApplicationContext(), AdminAct.class);
                                        intent.putExtra("TOKEN", token);
                                        startActivity(intent);

                                    } else if (userRole.equals("normal")) {
                                        Intent intent = new Intent(getApplicationContext(), UserAct.class);
                                        intent.putExtra("TOKEN", token);
                                        startActivity(intent);

                                    }

                                });
                            }

                            @Override
                            public void onLoginError(String error, Context context) {

                                runOnUiThread(() -> {
                                    // Toast amb el missatje.
                                    Toast.makeText(MainActivity.this,MESSAGE,Toast.LENGTH_LONG).show();
                                });
                            }
                        });
                    } catch (Exception e) {
                        throw new RuntimeException(e);
                    }
                });
            }
        });


    }




}