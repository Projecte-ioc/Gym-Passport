package com.example.garcia.ioc.garcia_o_tk.admin.events;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.garcia.ioc.garcia_o_tk.R;

import java.util.List;

/**
 * author: Claudio Garcia.
 * Adaptador extensió d'un recyclerView, serveix per carregar el layout amb les dades de l'objecte EventsInfo en una llista de cardviews.
 */

public class EventsAdapter extends RecyclerView.Adapter<EventsAdapter.ViewHolder>  {

    private List<EventsInfo> events;
    public EventsAdapter(List<EventsInfo> events) {
        this.events = events;
    }
    public void setEvents(List<EventsInfo> events) {
        this.events = events;
    }


    @NonNull
    @Override
    public EventsAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        // Inflem el recyclerView amb el disseny del cardView.
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.dynamic_cardevents, parent, false);
        return new EventsAdapter.ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull EventsAdapter.ViewHolder holder, int position) {
        //Instanciem l'objecte clientsInfo per poder accedir a les dades.
        EventsInfo event = events.get(position);
        //Assignem els textView's corresponents a les dades que volem mostrar.
        holder.textEventname.setText(event.getEvName());
        holder.textDate.setText(event.getDate());
        holder.textHour.setText("Hora: "+event.getHour());
        holder.textAttendes.setText("Asistents: "+event.getAttendes());
        holder.textLocation.setText("Ubicació: "+event.getLocation());

    }

    @Override
    public int getItemCount() {
        return events.size();
    }

    // Métode per instanciar les posicions dels textView's dins el layout de disseny del cardView.
    public static class ViewHolder extends RecyclerView.ViewHolder {
        public TextView textEventname, textDate,textHour, textAttendes,textLocation;
        public ViewHolder(View itemView) {
            super(itemView);
            textEventname = itemView.findViewById(R.id.textViewEventname);
            textDate = itemView.findViewById(R.id.textViewDate);
            textHour = itemView.findViewById(R.id.textViewHour);
            textAttendes = itemView.findViewById(R.id.textViewAttendes);
            textLocation = itemView.findViewById(R.id.textViewlocation);
        }
    }

}
