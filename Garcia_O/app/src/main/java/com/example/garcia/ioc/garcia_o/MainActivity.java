package com.example.garcia.ioc.garcia_o;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;



public class MainActivity extends AppCompatActivity {

    private EditText logByName;
    private EditText logByPsw;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    public void viewsByUser(View view) {

        logByName=findViewById(R.id.editTextLog);
        logByPsw=findViewById(R.id.editTextLogPsw);
        String un=logByName.getText().toString();
        String dos=logByPsw.getText().toString();


        if ((un.equalsIgnoreCase("a"))){
            Intent intent= new Intent(this, AdminAct.class);
            startActivity(intent);
        } else{
            Intent intent= new Intent(this, UserAct.class);
            startActivity(intent);
        }

    }


}