package com.example.garcia.ioc.garcia_o_tk.admin.events;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import com.example.garcia.ioc.garcia_o_tk.R;

import java.util.List;

/**
 * author: Claudio Garcia.
 * Adaptador extensió de baseAdapter, serveix per carregar el layout amb les dades de l'objecte EventsInfoDel en una llista.
 */


public class EventsAdapterDel extends BaseAdapter {
    private final Context context;
    private final List<EventsInfoDel> eventList;

    public EventsAdapterDel(Context context, List<EventsInfoDel> eventList) {
        this.context = context;
        this.eventList = eventList;
    }
    @Override
    public int getCount() {
        return eventList.size();
    }

    @Override
    public Object getItem(int position) {
        return eventList.get(position);
    }

    @Override
    public long getItemId(int position) {
        return position ;
    }

    @Override
    public View getView(int position, View view, ViewGroup viewGroup) {
        if (view == null) {
            LayoutInflater inflater = LayoutInflater.from(context);
            view = inflater.inflate(R.layout.item_event, viewGroup, false);
        }

        // Obtenim la posició de l'event.
        EventsInfoDel event = eventList.get(position);
        // Configurem les views corresponents.
        TextView idTextView = view.findViewById(R.id.idTextView);
        TextView nameTextView = view.findViewById(R.id.nameTextView);
        TextView dateTextView = view.findViewById(R.id.dateTextView);

        // inserim les dades.
        idTextView.setText(String.valueOf(event.getId()));
        nameTextView.setText(event.getName());
        dateTextView.setText(event.getDate());

        return view;
    }
}
