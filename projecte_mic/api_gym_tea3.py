
from flask import Flask, request, jsonify

from database_models_tea2 import User, Gym
from utils_tea_2 import Connexion

app = Flask(__name__)
db = Connexion()


@app.route('/consultar_clientes_gym', methods=['GET'])
def select_all_clients_gym():
    connection, cursor = db.get_connection_to_db()
    token = request.headers.get('Authorization')
    rol_user, id, _, _ = db.validate_rol_user(token)
    if rol_user == "admin":
        clients_of_my_gym = f"SELECT * FROM {User.__table_name__} WHERE gym_id = {id}"
        cursor.execute(clients_of_my_gym)
        results = cursor.fetchall()
        keys = ['id', 'name', 'role', 'password', 'gym_id', 'user_name', 'log']
        results_dict = [dict(zip(keys, row)) for row in results]
        connection.close()
        return jsonify(results_dict)
    return jsonify({'message': 'No tens permisos per a consultar aquestes dades'}), 401


@app.route('/update_gym', methods=['PATCH'])
def update_gym_data():
    '''
    ESTRUCTURA JSON amb les dades a actualitzar, sino estan es perque es va quedar que no es poden modificar:
    {
        address: ,
        phone_number: ,
        schedule: ,
    }
    '''
    data = request.get_json(force=True)
    token = request.headers.get('Authorization')
    rol_user, id, user_name, gym_name = db.validate_rol_user(token)
    connection, cursor = db.get_connection_to_db()
    if rol_user == 'admin':
        name = gym_name.replace(' ', '-')
        new_address = data.get('address')
        new_phone_number = data.get('phone_number')
        new_schedule = []
        for item in data.get('schedule'):
            new_schedule.append(item)
        update_query = f"UPDATE {Gym.__table_name__} SET "
        update_values = []
        if new_address:
            update_query += " address = %s,"
            update_values.append(new_address)

        if new_phone_number:
            update_query += " phone_number = %s,"
            update_values.append(new_phone_number)

        if new_schedule:
            update_query += " schedule = %s,"
            update_values.append(new_schedule)

        update_query = update_query.rstrip(',') + " WHERE name = %s"
        update_values.append(name)

        cursor.execute(update_query, tuple(update_values))
        connection.commit()
        connection.close()
        return jsonify({'message': 'Datos actualizados correctamente'}), 200
    else:
        return jsonify({'message': 'No es poden actualitzar les dades per falta de permisos'}), 401


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=2000)
