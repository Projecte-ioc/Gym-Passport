import flask
import jwt
import psycopg2
from flask import Flask, request, jsonify
from werkzeug.security import generate_password_hash

import utils

app = Flask(__name__)
db = utils.Connexion()


def register(id):
    connection, cursor = db.get_connection_to_db()
    data = request.get_json(force=True)
    if isinstance(data, dict):
        name = data.get('name')
        rol = data.get('rol_user')
        user = data.get('user_name')
        pswd = generate_password_hash(data.get('pswd_app'), method='pbkdf2', salt_length=16)
    else:
        for item in data:
            name = item['name']
            rol = item['rol_user']
            user = item['user_name']
            pswd = generate_password_hash(item['pswd_app'], method='pbkdf2', salt_length=16)

    user_name_exists = db.get_elements_filtered(user, "users_data", "user_name", '*')
    if not user_name_exists:
        try:
            cursor.execute("INSERT INTO users_data (name, rol_user, pswd_app, gym_id, user_name) VALUES ("
                           "%s, %s, %s, %s, %s)", (name,
                                                   rol, pswd, id, user
                                                   ))
            connection.commit()
            return jsonify({'message': 'Usuario registrado correctamente'})
        except psycopg2.Error as e:
            return jsonify({'message': f'Error al registrar el usuario {e}'}), 500
        finally:
            cursor.close()
            connection.close()
    return jsonify({'message': 'Usuario ya existe'})


def validate_rol_user(token):
    data = db.get_elements_of_token(token).get_json(force=True)
    rol_user = data.get('rol_user')
    gym_name = data.get('gym_name')
    id = db.get_elements_filtered(gym_name.replace(' ', '-'), "gym", "name", "id")

    return rol_user, id


@app.route('/consultar_clientes_gym', methods=['GET'])
def select_all_clients_gym():
    connection, cursor = db.get_connection_to_db()
    token = request.headers.get('Authorization')
    rol_user, id = validate_rol_user(token)
    if rol_user == "admin":
        clients_of_my_gym = f"SELECT * FROM users_data WHERE gym_id = {id}"
        cursor.execute(clients_of_my_gym)
        results = cursor.fetchall()
        connection.close()
        return jsonify(results)
    return jsonify({'message': 'No tens permisos per a consultar aquestes dades'}), 401


@app.route('/insert_client', methods=['POST'])
def insert_individual_client():
    token = request.headers.get('Authorization')
    rol_user, id = validate_rol_user(token)
    if rol_user == 'admin':
        register(id)
    return jsonify({'message': 'el usuario no se ha podido registrar porque ya existe o por falta de permisos'}), 500


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=2000)
