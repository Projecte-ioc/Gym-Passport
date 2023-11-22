package com.example.garcia.ioc.garcia_o_tk.admin;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import com.example.garcia.ioc.garcia_o_tk.R;
import java.util.List;

/**
 * author: Claudio Garcia Otero
 * Adaptador per poder carregar amb l'XML dynamic_cardview el recyclerView-
 */
public class ClientsAdapter extends RecyclerView.Adapter<ClientsAdapter.ViewHolder> {



    private List<ClientsInfo> clients;
    public ClientsAdapter(List<ClientsInfo> clients) {
        this.clients = clients;
    }
    public void setClients(List<ClientsInfo> clients) {
        this.clients = clients;
    }


        @NonNull
        @Override
        public ClientsAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            // Inflem el recyclerView amb el disseny del cardView.
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.dynamic_cardview, parent, false);
            return new ClientsAdapter.ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(@NonNull ClientsAdapter.ViewHolder holder, int position) {
            //Instanciem l'objecte clientsInfo per poder accedir a les dades.
            ClientsInfo cliente = clients.get(position);
            //Assignem els textView's corresponents a les dades que volem mostrar.
            holder.textViewNom.setText(cliente.getName());
            holder.textViewUserName.setText(cliente.getUserName());
            holder.textViewRol.setText(cliente.getRole());

        }

        @Override
        public int getItemCount() {
            return clients.size();
        }

        // Métode per instanciar les posicions dels textView's dins el layout de disseny del cardView.
        public static class ViewHolder extends RecyclerView.ViewHolder {
            public TextView textViewNom, textViewRol, textViewUserName;
            public ViewHolder(View itemView) {
                super(itemView);
                textViewNom = itemView.findViewById(R.id.textViewName);
                textViewUserName = itemView.findViewById(R.id.textViewUserName);
                textViewRol = itemView.findViewById(R.id.textViewRol);
            }
        }



}
