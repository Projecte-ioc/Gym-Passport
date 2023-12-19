import datetime
import os

import jwt
import psycopg2
from flask import Flask, request, jsonify

from database_models_tea2 import User, GymEvent, List_user_events
from utils_tea_2 import Connexion

app = Flask(__name__)
db = Connexion()


#  __keys_events__ = ['id', 'name', 'whereisit', 'qty_max_attendes', 'qty_got_it', 'user_id',
#                      'gym_id', 'done','date', 'hour', 'minute', 'duration']

# __key_list_events__=['id_gym', 'id_user', 'id_event', 'rating_event']
def insert_simple(id_gym, id_user, id_event, connection, cursor):
    """
    Aquesta funció insereix a un usuari a la llista d'usuaris apuntats al event.
    :param id_gym:
    :param id_user:
    :param id_event:
    :param connection:
    :param cursor:
    :return:
    """
    List_user_events.id_gym = id_gym
    List_user_events.id_user = id_user
    List_user_events.id_event = id_event
    List_user_events.rating_event = 0
    try:
        cursor.execute(
            f"INSERT INTO {List_user_events.__table_name__} (id_gym, id_user, id_event, rating_event) VALUES ("
            "%s, %s, %s, %s)", (List_user_events.id_gym,
                                List_user_events.id_user, List_user_events.id_event, List_user_events.rating_event
                                ))
        connection.commit()
    except psycopg2.Error:
        print(psycopg2.Error)
    finally:
        cursor.close()
        connection.close()


def delete_simple(id_event, cursor, connection):
    try:
        cursor.execute(
            f"DELETE FROM {List_user_events.__table_name__} WHERE id_event = %s", (id_event,))
        connection.commit()
    except psycopg2.Error as e:
        print(e)
    finally:
        cursor.close()
        connection.close()


@app.route('/obtener_eventos', methods=['GET'])
def get_all_events():
    try:
        # Obtén los parámetros de paginación de la URL
        page = request.args.get('page', default=1, type=int)
        per_page = request.args.get('per_page', default=5, type=int)

        token = request.headers.get('Authorization')
        jwe = db.decipher_content(token)
        rol_user, id, user_name, gym_name = db.validate_rol_user(jwe)

        # Calcula el índice de inicio y fin para la paginación
        start_index = (page - 1) * per_page
        end_index = start_index + per_page

        # Realiza la consulta con paginación
        results = db.get_elements_filtered(id, GymEvent.__table_name__,
                                           'gym_id',
                                           '*',
                                           start=start_index, end=end_index)

        if results:
            results_dict_list = [dict(zip(User.__keys_user__, [str(cell) if isinstance(cell, datetime.date) else cell for cell in row])) for row in results]
            results_dict = {'results': results_dict_list}
            token = jwt.encode(results_dict, os.getenv('SK'), algorithm='HS256')
            results_dict_cipher = db.cipher_content(token)
            return jsonify({"jwe": results_dict_cipher}), 200
        else:
            return jsonify({'message': 'No es possible recuperar les dades'}), 404

    except Exception as e:
        return jsonify({'error': str(e)}), 500


@app.route('/filtrar_eventos', methods=['POST'])
def get_filtered_events():
    token = request.headers.get('Authorization')
    jwe = db.decipher_content(token)
    rol_user, id, user_name, gym_name = db.validate_rol_user(jwe)
    user_name_params = db.decipher_content(request.args.get('user_name'))
    id_gym_user_params = db.get_elements_filtered(user_name_params, User.__table_name__, 'user_name', 'gym_id')
    id_user = db.get_elements_filtered(user_name_params, User.__table_name__, 'user_name', 'id')
    if id_gym_user_params[0][0] == id:
        results = db.get_elements_filtered(id_user[0][0], GymEvent.__table_name__, 'user_id', '*')
        results_dict_list = [dict(zip(GymEvent.__keys_events__, [str(cell) if isinstance(cell, datetime.date) else cell for cell in row])) for row in results]
        results_dict = {'results': results_dict_list}
        token_result = jwt.encode(results_dict, os.getenv('SK'), algorithm='HS256')
        result_dict_cipher = db.cipher_content(token_result)
        return jsonify({"jwe": result_dict_cipher}), 200
    else:
        return jsonify({'message': 'Error al recuperar les dades solicitades'}), 404


@app.route('/insertar_evento', methods=['POST'])
def insert_event():
    '''
    Insereix un event nou.
    ESTRUCTURA JSON:
    {
        'date':'',
        'done': false,
        'hour': 10,
        'minute':15,
        'duration':45,
        'name': "",
        'qty_max_attendes':20,
        'whereisit': ""
    }
    :return: 200 if ok, 500 some insert error
    '''
    connection, cursor = db.get_connection_to_db()
    token = request.headers.get('Authorization')
    jwe = db.decipher_content(token)
    rol_user, id, user_name, gym_name = db.validate_rol_user(jwe)
    data = request.get_json(force=True)
    data_dcf = db.get_elements_of_token(db.decipher_content(data.get('jwe')))
    if isinstance(data_dcf, dict):
        GymEvent.date = data_dcf.get('date')
        GymEvent.done = data_dcf.get('done')
        GymEvent.gym_id = id
        GymEvent.hour = data_dcf.get('hour')
        GymEvent.minute = data_dcf.get('minute')
        GymEvent.duration = data_dcf.get('duration')
        GymEvent.name = data_dcf.get('name')
        GymEvent.qty_got_it = 0
        GymEvent.qty_max_attendes = data_dcf.get('qty_max_attendes')
        user_id = db.get_elements_filtered(user_name, User.__table_name__, 'user_name', 'id')
        GymEvent.user_id = user_id[0][0]
        GymEvent.whereisit = data_dcf.get('whereisit')
    else:
        for item in data_dcf:
            GymEvent.date = item['date']
            GymEvent.done = item['done']
            GymEvent.gym_id = id
            GymEvent.hour = item['hour']
            GymEvent.minute = item['minute']
            GymEvent.duration = item['duration']
            GymEvent.name = item['name']
            GymEvent.qty_got_it = 0
            GymEvent.qty_max_attendes = item['qty_max_attendes']
            user_id = db.get_elements_filtered(user_name, User.__table_name__, 'user_name', 'id')
            GymEvent.user_id = user_id[0][0]
            GymEvent.whereisit = item['whereisit']
    try:
        cursor.execute(
            f"INSERT INTO {GymEvent.__table_name__} (name, whereisit, qty_max_attendes, qty_got_it, user_id, gym_id, "
            f"done, date, hour, minute, duration) VALUES ("
            "%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)", (GymEvent.name,
                                                            GymEvent.whereisit, GymEvent.qty_max_attendes,
                                                            GymEvent.qty_got_it,
                                                            GymEvent.user_id, GymEvent.gym_id,
                                                            GymEvent.done, GymEvent.date,
                                                            GymEvent.hour, GymEvent.minute,
                                                            GymEvent.duration
                                                            ))
        connection.commit()
        return jsonify({'message': 'Esdeveniment enregistrat correctament'}), 201
    except psycopg2.Error as e:
        print(e)
        return jsonify({'message': 'Error al inserir'}), 500
    finally:
        cursor.close()
        connection.close()


@app.route('/reserva_evento', methods=['PATCH'])
def got_it_place():
    """
    Reserva plaça a l'event
    /reserva_evento?event_id=
    :return:
    """
    event_id = request.args.get('event_id')
    print(type(event_id))
    token = request.headers.get('Authorization')
    jwe = db.decipher_content(token)
    connex, cursor = db.get_connection_to_db()
    rol_user, id, user_name, gym_name = db.validate_rol_user(jwe)
    query = f"SELECT qty_got_it, qty_max_attendes FROM {GymEvent.__table_name__} WHERE id = %s AND gym_id = %s"
    cursor.execute(query, (event_id, id))
    result = cursor.fetchone()
    if result[0] is None:
        qty_got_it_now = 0
    else:
        qty_got_it_now = result[0]
    qty_max = result[1]
    print(str(qty_got_it_now))
    print(str(qty_max))
    if result:
        GymEvent.qty_got_it = qty_got_it_now + 1
    if qty_max == qty_got_it_now:
        return jsonify({'message': 'No queden places disponibles'}), 401
    update_query = f"UPDATE {GymEvent.__table_name__} SET qty_got_it = %s WHERE id = %s"
    cursor.execute(update_query, (GymEvent.qty_got_it, event_id))
    user_id = db.get_elements_filtered(user_name, User.__table_name__, "user_name", "id")
    user_id_exact = user_id[0][0]
    insert_simple(id_gym=id, id_user=user_id_exact, id_event=event_id, connection=connex, cursor=cursor)
    return jsonify({'message': 'Has reservat plaça correctament!'}), 201


@app.route('/eliminar_reserva_evento', methods=['PATCH'])
def delete_reservation():
    """
        Reserva plaça a l'event
        /eliminar_reserva_evento?event_id=
        :return:
        """
    event_id = request.args.get('event_id')
    token = db.decipher_content(request.headers.get('Authorization'))
    connex, cursor = db.get_connection_to_db()
    rol_user, id, user_name, gym_name = db.validate_rol_user(token)
    query = f"SELECT qty_got_it, qty_max_attendes FROM {GymEvent.__table_name__} WHERE id = %s AND gym_id = %s"
    cursor.execute(query, (event_id, id))
    result = cursor.fetchone()
    qty_got_it_now = result[0]
    qty_max = result[1]
    print(str(qty_got_it_now))
    print(str(qty_max))
    if result:
        if qty_got_it_now == 1:
            GymEvent.qty_got_it = 0
        else:
            GymEvent.qty_got_it = qty_got_it_now - 1
    if qty_max == qty_got_it_now:
        return jsonify({'message': 'No queden places disponibles'}), 401
    update_query = f"UPDATE {GymEvent.__table_name__} SET qty_got_it = %s WHERE id = %s"
    cursor.execute(update_query, (GymEvent.qty_got_it, event_id))
    user_id = db.get_elements_filtered(user_name, User.__table_name__, "user_name", "id")
    user_id_exact = user_id[0][0]
    delete_query = f"DELETE FROM  {List_user_events.__table_name__} WHERE id_user = %s"
    cursor.execute(delete_query, (user_id_exact,))
    connex.commit()
    connex.close()
    return jsonify({'message': 'Has alliberat la plaça correctament!'}), 201


@app.route('/eliminar_evento', methods=['DELETE'])
def delete_event_and_suscriptions():
    event_id = request.args.get('event_id')
    token = db.decipher_content(request.headers.get('Authorization'))
    connex, cursor = db.get_connection_to_db()
    rol_user, id, user_name, gym_name = db.validate_rol_user(token)
    query = f"SELECT qty_got_it FROM {GymEvent.__table_name__} WHERE id = %s AND gym_id = %s"
    cursor.execute(query, (event_id, id))
    result = cursor.fetchone()
    qty_got_it_now = result[0]
    user_id_on_users = f"SELECT id FROM {User.__table_name__} WHERE user_name = %s"
    cursor.execute(user_id_on_users, (user_name,))
    userid_users_table = cursor.fetchone()
    user_id_on_events = f"SELECT user_id FROM {GymEvent.__table_name__} WHERE id = %s"
    cursor.execute(user_id_on_events, (event_id,))
    userid_events_table = cursor.fetchone()
    if result and qty_got_it_now > 0:
        if rol_user == 'admin' or userid_users_table[0] == userid_events_table[0]:
            query_delete_reservation = f"DELETE FROM {List_user_events.__table_name__} WHERE id_event = %s"
            cursor.execute(query_delete_reservation, (event_id,))
            query_delete_event = f"DELETE FROM {GymEvent.__table_name__} WHERE id = %s"
            cursor.execute(query_delete_event, (event_id,))
            connex.commit()
            connex.close()
            return jsonify({'message': 'Registre esborrat correctament.'}), 201
        else:
            return jsonify({'message': 'No tens permissos per esborrar.'}), 401
    else:
        if rol_user == 'admin' or userid_users_table == userid_events_table:
            query_delete_event = f"DELETE FROM {GymEvent.__table_name__} WHERE id = %s"
            cursor.execute(query_delete_event, (event_id,))
            connex.commit()
            connex.close()
            return jsonify({'message': 'Registre esborrat correctament.'}), 201
        else:
            return jsonify({'message': 'No tens permissos per esborrar.'}), 401


@app.route('/modificar_evento', methods=['PATCH'])
def update_event():
    """{
    'name': "",
    'whereisit': "",
    'qty_max_attendes': "",
    'date': "",
    'hour': "",
    'minute':""
    }"""
    event_id = request.args.get("event_id")
    data = request.get_json(force=True)
    data_dcf = db.get_elements_of_token(db.decipher_content(data.get('jwe')))
    token = request.headers.get('Authorization')
    jwe = db.decipher_content(token)
    rol_user, id, user_name, gym_name = db.validate_rol_user(jwe)
    connection, cursor = db.get_connection_to_db()
    user_id_on_users = f"SELECT id FROM {User.__table_name__} WHERE user_name = %s"
    cursor.execute(user_id_on_users, (user_name,))
    userid_users_table = cursor.fetchone()
    user_id_on_events = f"SELECT user_id FROM {GymEvent.__table_name__} WHERE id = %s"
    cursor.execute(user_id_on_events, (event_id,))
    userid_events_table = cursor.fetchone()
    if rol_user == 'admin' or userid_users_table == userid_events_table:
        GymEvent.name = data_dcf.get('name')
        GymEvent.whereisit = data_dcf.get('whereisit')
        GymEvent.qty_max_attendes = data_dcf.get('qty_max_attendes')
        GymEvent.date = data_dcf.get('date')
        GymEvent.hour = data_dcf.get('hour')
        GymEvent.minute = data_dcf.get('minute')

        update_query = f"UPDATE {GymEvent.__table_name__} SET "
        update_values = []
        if GymEvent.name:
            update_query += " name = %s,"
            update_values.append(GymEvent.name)

        if GymEvent.whereisit:
            update_query += " whereisit = %s,"
            update_values.append(GymEvent.whereisit)

        if GymEvent.qty_max_attendes:
            update_query += " schedule = %s,"
            update_values.append(GymEvent.qty_max_attendes)

        if GymEvent.date:
            update_query += " schedule = %s,"
            update_values.append(GymEvent.date)

        if GymEvent.hour:
            update_query += " schedule = %s,"
            update_values.append(GymEvent.hour)

        if GymEvent.minute:
            update_query += " schedule = %s,"
            update_values.append(GymEvent.minute)

        update_query = update_query.rstrip(',') + " WHERE id = %s"
        update_values.append(event_id)

        cursor.execute(update_query, tuple(update_values))
        connection.commit()
        connection.close()
        return jsonify({'message': 'Datos actualizados correctamente'}), 200
    else:
        return jsonify({'message': 'No es poden actualitzar les dades per falta de permisos'}), 401


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=6000)
