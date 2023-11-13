package com.example.garcia.ioc.garcia_o_tk.user;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.view.View;

import com.example.garcia.ioc.garcia_o_tk.R;

public class UserAct extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_user);
    }


    //Button que tanca l'activitat, com que l'API encara es de prova
    //no es desconecta del servidor, però en un futur serà la seva funció.
     public void close(View view) {
        finish();
    }
}