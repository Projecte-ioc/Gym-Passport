import psycopg2
from flask import Flask, request, jsonify

from database_models_tea2 import User, GymEvent
from utils_tea_2 import Connexion

app = Flask(__name__)
db = Connexion()


#  __keys_events__ = ['id', 'name', 'whereisit', 'qty_max_attendes', 'qty_got_it', 'user_id',
#                      'gym_id', 'done','date', 'hour']


@app.route('/obtener_eventos', methods=['GET'])
def get_all_events():
    token = request.headers.get('Authorization')
    rol_user, id, user_name, gym_name = db.validate_rol_user(token)
    results = db.get_elements_filtered(id, GymEvent.__table_name__, 'gym_id', '*')
    if results:
        results_dict = [dict(zip(GymEvent.__keys_events__, row)) for row in results]
        return jsonify(results_dict), 200
    else:
        return jsonify({'message': 'No es possible recuperar les dades'}), 404


@app.route('/filtrar_eventos', methods=['POST'])
def get_filtered_events():
    token = request.headers.get('Authorization')
    rol_user, id, user_name, gym_name = db.validate_rol_user(token)
    user_name_params = request.args.get('user_name')
    id_gym_user_params = db.get_elements_filtered(user_name_params, User.__table_name__, 'user_name', 'gym_id')
    id_user = db.get_elements_filtered(user_name_params, User.__table_name__, 'user_name', 'id')
    if id_gym_user_params[0][0] == id:
        results = db.get_elements_filtered(id_user[0][0], GymEvent.__table_name__, 'user_id', '*')
        results_dict = [dict(zip(GymEvent.__keys_events__, row)) for row in results]
        return jsonify(results_dict), 200
    else:
        return jsonify({'message': 'Error al recuperar les dades solicitades'}), 404


@app.route('/insertar_evento', methods=['POST'])
def insert_event():
    '''
    ESTRUCTURA JSON:
    {
        'date':'',
        'done': false,
        'hour': 10,
        'name': "",
        'qty_max_attendes':20,
        'whereisit': ""
    }
    :return: 200 if ok, 500 some insert error
    '''
    connection, cursor = db.get_connection_to_db()
    token = request.headers.get('Authorization')
    rol_user, id, user_name, gym_name = db.validate_rol_user(token)
    data = request.get_json(force=True)
    if isinstance(data, dict):
        GymEvent.date = data.get('date')
        GymEvent.done = data.get('done')
        GymEvent.gym_id = id
        GymEvent.hour = data.get('hour')
        GymEvent.name = data.get('name')
        GymEvent.qty_got_it = 0
        GymEvent.qty_max_attendes = data.get('qty_max_attendes')
        user_id = db.get_elements_filtered(user_name, User.__table_name__, 'user_name', 'id')
        GymEvent.user_id = user_id[0][0]
        GymEvent.whereisit = data.get('whereisit')
    else:
        for item in data:
            GymEvent.date = item['date']
            GymEvent.done = item['done']
            GymEvent.gym_id = id
            GymEvent.hour = item['hour']
            GymEvent.name = item['name']
            GymEvent.qty_got_it = 0
            GymEvent.qty_max_attendes = item['qty_max_attendes']
            user_id  = db.get_elements_filtered(user_name, User.__table_name__, 'user_name', 'id')
            GymEvent.user_id = user_id[0][0]
            GymEvent.whereisit = item['whereisit']
    try:
        cursor.execute(
            f"INSERT INTO {GymEvent.__table_name__} (name, whereisit, qty_max_attendes, qty_got_it, user_id, gym_id, "
            f"done, date, hour) VALUES ("
            "%s, %s, %s, %s, %s, %s, %s, %s, %s)", (GymEvent.name,
                                                    GymEvent.whereisit, GymEvent.qty_max_attendes,
                                                    GymEvent.qty_got_it,
                                                    GymEvent.user_id, GymEvent.gym_id,
                                                    GymEvent.done, GymEvent.date,
                                                    GymEvent.hour
                                                    ))
        connection.commit()
        return jsonify({'message': 'Esdeveniment enregistrat correctament'}), 201
    except psycopg2.Error as e:
        print(e)
        return jsonify({'message': 'Error al inserir'}), 500
    finally:
        cursor.close()
        connection.close()


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=6000)
