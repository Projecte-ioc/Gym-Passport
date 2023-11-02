import flask
import jwt
import psycopg2
from flask import Flask, request, jsonify
from werkzeug.security import generate_password_hash

import utils

app = Flask(__name__)
db = utils.Connexion()


def register(id, name, rol, user, pswd, cursor, connection):
    user_name_exists = db.get_elements_filtered(user, "users_data", "user_name", '*')
    if not user_name_exists:
        try:
            cursor.execute("INSERT INTO users_data (name, rol_user, pswd_app, gym_id, user_name) VALUES ("
                           "%s, %s, %s, %s, %s)", (name,
                                                   rol, pswd, id, user
                                                   ))
        except psycopg2.Error as e:
            print(e)

    return 'Usuario ya existe'


def validate_rol_user(token):
    data = db.get_elements_of_token(token).get_json(force=True)
    rol_user = data.get('rol_user')
    gym_name = data.get('gym_name')
    user_name = data.get('user_name')
    id = db.get_elements_filtered(gym_name.replace(' ', '-'), "gym", "name", "id")

    return rol_user, id[0][0], user_name


@app.route('/consultar_clientes_gym', methods=['GET'])
def select_all_clients_gym():
    connection, cursor = db.get_connection_to_db()
    token = request.headers.get('Authorization')
    rol_user, id, _ = validate_rol_user(token)
    if rol_user == "admin":
        clients_of_my_gym = f"SELECT * FROM users_data WHERE gym_id = {id}"
        cursor.execute(clients_of_my_gym)
        results = cursor.fetchall()
        connection.close()
        return jsonify(results)
    return jsonify({'message': 'No tens permisos per a consultar aquestes dades'}), 401


@app.route('/insert_client', methods=['POST'])
def insert_individual_client():
    connection, cursor = db.get_connection_to_db()
    token = request.headers.get('Authorization')
    rol_user, id, _ = validate_rol_user(token)
    data = request.get_json(force=True)
    try:
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
        if rol_user == 'admin':
            register(id, name, rol, user, pswd, cursor, connection)

            connection.commit()
            cursor.close()
            connection.close()

            return jsonify({'message': 'Usuario registrado correctamente'})
    except psycopg2.Error as e:
        return jsonify(
            {'message': f'el usuario no se ha podido registrar porque ya existe o por falta de permisos {e}'}), 500
    finally:
        cursor.close()
        connection.close()


@app.route('/insert_some_clients', methods=['POST'])
def insert_diferents_clients():
    connection, cursor = db.get_connection_to_db()
    token = request.headers.get('Authorization')
    rol_user, id, _ = validate_rol_user(token)
    data = request.get_json(force=True)
    if rol_user == 'admin':
        try:
            for item in data:
                name = item['name']
                rol = item['rol_user']
                user = item['user_name']
                pswd = generate_password_hash(item['pswd_app'], method='pbkdf2', salt_length=16)
                register(id, name, rol, user, pswd, cursor, connection)
                connection.commit()
            return jsonify({'message': 'Usuario registrado correctamente'}), 200
        except psycopg2.Error as e:
            return jsonify({'message': f'Error {e}'}), 401
        finally:
            cursor.close()
            connection.close()
    return jsonify({'message': 'el usuario no se ha podido registrar porque ya existe o por falta de permisos'}), 500


@app.route('/update_data_client', methods=['PUT'])
def update_client_data():
    """
    Estructura JSON:
    {
        "name": "",
        "pswd_app": "",
        "rol_user": "",
        "user_name": ""
    }
    """
    connection, cursor = db.get_connection_to_db()
    token = request.headers.get('Authorization')
    rol_user, id, user_name = validate_rol_user(token)
    data = request.get_json(force=True)
    try:
        if isinstance(data, dict):
            new_name = data.get('name')
            new_rol = data.get('rol_user')
            user = data.get('user_name')
            if len(data.get('pswd_app')) < 20:
                new_pswd = generate_password_hash(data.get('pswd_app'), method='pbkdf2', salt_length=16)
            else:
                new_pswd = data.get('pswd_app')
        else:
            for item in data:
                new_name = item['name']
                new_rol = item['rol_user']
                user = item['user_name']
                if len(data.item['pswd_app']) < 20:
                    new_pswd = generate_password_hash(data.item['pswd_app'], method='pbkdf2', salt_length=16)
                else:
                    new_pswd = data.item['pswd_app']

        if rol_user == 'admin' or rol_user != 'admin' and user_name == user:
            if not user or (not new_name and not new_rol and not new_pswd):
                return "Faltan datos requeridos", 400
            cursor.execute("SELECT COUNT(*) FROM users_data WHERE user_name = %s", (user,))
            count = cursor.fetchone()[0]
            if count == 0:
                connection.close()
                return "Faltan datos requeridos", 404
            # Realizar la actualización en la base de datos
            update_query = "UPDATE users_data SET"
            update_values = []

            if new_name:
                update_query += " name = %s,"
                update_values.append(new_name)

            if new_rol:
                update_query += " rol_user = %s,"
                update_values.append(new_rol)

            if new_pswd:
                update_query += " pswd_app = %s,"
                update_values.append(new_pswd)

            update_query = update_query.rstrip(',') + " WHERE user_name = %s"
            update_values.append(user)

            cursor.execute(update_query, tuple(update_values))
            return jsonify({'message': 'Datos actualizados correctamente'}), 200
        else:
            return jsonify({'message': 'No es poden actualitzar les dades per falta de permisos'}), 401
    except psycopg2.Error as e:
        print(e)
        return jsonify({'message': f'Error {e}'}), 401
    finally:
        connection.commit()
        connection.close()


# todo delete

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=2000)
